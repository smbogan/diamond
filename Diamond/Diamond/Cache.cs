using Diamond.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class Cache
    {
        private Dictionary<ResourceIdentifier, Table> Tables = new Dictionary<ResourceIdentifier, Table>();

        private Dictionary<ResourceIdentifier, ViewDescriptor> ViewTemplates = new Dictionary<ResourceIdentifier, ViewDescriptor>();

        private Dictionary<ResourceIdentifier, View> Views = new Dictionary<ResourceIdentifier, View>();


        private Repository Repository { get; set; }

        public Cache(Repository repository)
        {
            Repository = repository;
        }

        public bool Exists(ResourceIdentifier identifier)
        {
            return Repository.Exists(identifier);
        }

        public ViewDescriptor GetViewDescriptor(ResourceIdentifier identifier)
        {
            ViewDescriptor result;

            if(ViewTemplates.TryGetValue(identifier, out result))
            {
                return result;
            }

            using (var stream = Repository.ReadFile(identifier))
            {
                result = new ViewDescriptor(stream);

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

        public void SaveView(ResourceIdentifier identifier)
        {
            View view;

            if(!Views.TryGetValue(identifier, out view))
            {
                return; //nothing to save
            }

            using (MemoryStream ms = new MemoryStream())
            {
                view.Write(ms);
                ms.Position = 0;
                Repository.WriteFile(identifier, ms);
            }
        }

        public void SaveTable(ResourceIdentifier resourceIdentifier, Table newTable)
        {
            Tables[resourceIdentifier] = newTable;

            SaveTable(resourceIdentifier);
        }

        public void Clear()
        {
            Tables.Clear();
            ViewTemplates.Clear();
            Views.Clear();
        }

        public void SaveView(ResourceIdentifier resourceIdentifier, View newView)
        {
            Views[resourceIdentifier] = newView;

            SaveView(resourceIdentifier);
        }
    }
}
