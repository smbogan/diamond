using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class ViewGraph
    {

        Dictionary<string, int> paths = new Dictionary<string, int>();

        Dictionary<int, string> reversePaths = new Dictionary<int, string>();

        List<Tuple<int, int>> maps = new List<Tuple<int, int>>();

        int current = 0;

        public ViewGraph(IEnumerable<ViewField> fields)
        {
            foreach(var e in fields)
            {
                string name = Rename(e.Name);

                int index;

                if(!paths.TryGetValue(name, out index))
                {
                    index = current;
                    current++;

                    paths[name] = index;
                    reversePaths[index] = name;
                }
            }

            foreach(var e in fields)
            {
                string name = Rename(e.Name);

                int sourceIndex = paths[name];

                foreach(var v in e.DependentVariables)
                {
                    var nameV = Rename(v);

                    int dependentIndex;

                    if(!paths.TryGetValue(nameV, out dependentIndex))
                    {
                        dependentIndex = current;
                        current++;

                        paths[nameV] = dependentIndex;
                        reversePaths[dependentIndex] = nameV;
                    }

                    maps.Add(new Tuple<int, int>(sourceIndex, dependentIndex));
                }
            }
        }

        public IEnumerable<string> GetReverseDependencies(string name)
        {
            List<int> checkedList = new List<int>();

            var n = Rename(name);

            int index;

            if (!paths.TryGetValue(n, out index))
            {
                yield break;
            }

            checkedList.Add(index);

            foreach (var dependent in maps.Where(t => t.Item1 == index))
            {
                checkedList.Add(dependent.Item2);

                string pathName = reversePaths[dependent.Item2];

                foreach(var subDependent in GetReverseDependencies(pathName))
                {
                    int subDepIndex = paths[subDependent];

                    if(!checkedList.Contains(subDepIndex))
                    {
                        checkedList.Add(subDepIndex);
                        yield return subDependent;
                    }
                }

                yield return pathName;
            }
        }

        public IEnumerable<string> GetDependents(string name)
        {
            List<int> checkedList = new List<int>();

            var n = Rename(name);

            int index;

            if (!paths.TryGetValue(n, out index))
            {
                yield break;
            }

            checkedList.Add(index);

            foreach (var dependent in maps.Where(t => t.Item2 == index))
            {
                checkedList.Add(dependent.Item1);

                string pathName = reversePaths[dependent.Item1];

                foreach (var subDependent in GetDependents(pathName))
                {
                    int subDepIndex = paths[subDependent];

                    if (!checkedList.Contains(subDepIndex))
                    {
                        checkedList.Add(subDepIndex);
                        yield return subDependent;
                    }
                }

                yield return pathName;
            }
        }

        private string Rename(string key)
        {
            return (new string(key.Where(c =>
                  (c >= 'a' && c <= 'z')
                  || (c >= 'A' && c <= 'Z')
                  || (c >= '0' && c <= '9')).ToArray()));
        }
    }
}
