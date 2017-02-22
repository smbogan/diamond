using Diamond.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Templates
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PageAttribute : Attribute
    {
        readonly ResourceType resourceType;

        public PageAttribute(ResourceType resourceType)
        {
            this.resourceType = resourceType;
        }

        public ResourceType ResourceType
        {
            get { return resourceType; }
        }

        public static ResourceType GetPage(Type type)
        {
            object attribute = type.GetCustomAttributes(false).Where(a => a.GetType() == typeof(PageAttribute)).FirstOrDefault();

            if(attribute == null)
            {
                return ResourceType.Unknown;
            }

            return (attribute as PageAttribute).ResourceType;
        }
    }
}
