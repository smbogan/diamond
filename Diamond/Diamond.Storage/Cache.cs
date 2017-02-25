using Diamond.Storage.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond.Storage
{
    public class Cache
    {
        private Dictionary<ResourceIdentifier, Table> Tables = new Dictionary<ResourceIdentifier, Table>();

        private Dictionary<ResourceIdentifier, ViewTemplate> ViewTemplates = new Dictionary<ResourceIdentifier, ViewTemplate>();

        private Dictionary<ResourceIdentifier, View> Views = new Dictionary<ResourceIdentifier, View>();

        private Repository Repository { get; set; }

        public Cache(Repository repository)
        {
            Repository = repository;
        }

        public ViewTemplate GetViewTemplate(ResourceIdentifier identifier)
        {
            ViewTemplate result;

            if(ViewTemplates.TryGetValue(identifier, out result))
            {
                return result;
            }

            using (var stream = Repository.ReadFile(identifier))
            {
                result = new ViewTemplate(stream);

                ViewTemplates[identifier] = result;
            }

            return result;
        }

        public View GetView(ResourceIdentifier identifier)
        {
            View result;

            if(Views.TryGetValue(identifier, out result))
            {
                return result;
            }

            using (var stream = Repository.ReadFile(identifier))
            {
                result = new View(stream);

                Views[identifier] = result;
            }

            return result;
        }

        public Table GetTable(ResourceIdentifier identifier)
        {
            Table result;

            if (Tables.TryGetValue(identifier, out result))
            {
                return result;
            }

            using (var stream = Repository.ReadFile(identifier))
            {
                result = new Table(stream);

                Tables[identifier] = result;
            }

            return result;
        }

        public void SaveTable(ResourceIdentifier identifier)
        {
            Table result;

            if (!Tables.TryGetValue(identifier, out result))
            {
                return; //nothing to save
            }

            using (MemoryStream ms = new MemoryStream())
            {
                result.Write(ms);
                ms.Position = 0;
                Repository.WriteFile(identifier, ms);
            }
        }

        public void Clear()
        {
            Tables.Clear();
            ViewTemplates.Clear();
            Views.Clear();
        }
    }
}
