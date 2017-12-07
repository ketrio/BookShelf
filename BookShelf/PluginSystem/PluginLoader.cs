using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookShelf.PluginSystem
{
    static class PluginLoader
    {
        public static List<Type> Load(string path)
        {
            if (!Directory.Exists(path)) throw new ArgumentException();

            var types = Directory.GetFiles(path, "*.dll")
                .Select(dllPath => Assembly.Load(AssemblyName.GetAssemblyName(dllPath)))
                .Select(assembly => assembly.GetTypes()).SelectMany(item => item).Distinct().ToArray();
            var targetTypes = types.Where(type => typeof(BookPlugin).IsAssignableFrom(type));

            return new List<Type>(targetTypes);
        }
    }
}