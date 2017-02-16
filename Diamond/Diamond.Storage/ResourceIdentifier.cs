﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage
{
    public class ResourceIdentifier
    {
        public string Identifier { get; private set; }

        private static Dictionary<ResourceType, string> extensions = new Dictionary<ResourceType, string>()
        {
            { ResourceType.Table, "table" },
            { ResourceType.Text, "txt" },
            { ResourceType.Script, "script" }
        };

        public ResourceType ResourceType { get; private set; }

        public ResourceIdentifier(ResourceType resourceType, params string[] identifierKeys)
        {
            foreach(var key in identifierKeys)
            {
                foreach(var c in key)
                {
                    var category = char.GetUnicodeCategory(c);

                    if(!(
                        category == System.Globalization.UnicodeCategory.LowercaseLetter
                        || category == System.Globalization.UnicodeCategory.UppercaseLetter
                        || category == System.Globalization.UnicodeCategory.DecimalDigitNumber
                        || c == ' '
                        || c == '-'
                        || c == '_'
                        ))
                    {
                        throw new ArgumentException(string.Format("The character '{0}' is not allowed in a resource identifier.", c), nameof(identifierKeys));
                    }
                }
            }

            ResourceType = resourceType;

            Identifier = string.Join("/", identifierKeys) + "." + extensions[resourceType];
        }

        public ResourceIdentifier(string identifier)
        {
            ResourceType? resourceType = null;

            foreach(var kvp in extensions)
            {
                if(identifier.EndsWith("." + kvp.Value))
                {
                    resourceType = kvp.Key;
                }
            }

            if(resourceType == null)
            {
                throw new Exception("Unknown resource type.");
            }

            ResourceType = resourceType.Value;

            foreach (var c in identifier)
            {
                var category = char.GetUnicodeCategory(c);

                if (!(
                    category == System.Globalization.UnicodeCategory.LowercaseLetter
                    || category == System.Globalization.UnicodeCategory.UppercaseLetter
                    || category == System.Globalization.UnicodeCategory.DecimalDigitNumber
                    || c == ' '
                    || c == '-'
                    || c == '_'
                    || c == '/'
                    ))
                {
                    throw new ArgumentException(string.Format("The character '{0}' is not allowed in a resource identifier.", c), nameof(identifier));
                }
            }

            Identifier = identifier;
        }

        public bool Exists(string repositoryLocation)
        {
            return File.Exists(Path.Combine(repositoryLocation, Identifier));
        }
    }
}