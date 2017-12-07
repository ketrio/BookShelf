using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookShelf.PluginSystem
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginInfo : Attribute
    {
        public string Name { get; }
        public string Description { get; }
        public string Author { get; }
        public string Version { get; }

        public PluginInfo(string name, string description, string author, string version)
        {
            Name = name;
            Description = description;
            Author = author;
            Version = version;
        }
    }

    public interface IPlugin : IDisposable
    {
        Application App { get; }

        void Impact();
    }

    public abstract class BookPlugin : IPlugin
    {
        public Application App { get; }

        public BookPlugin(Application app)
        {
            App = app;

            if (GetType().IsDefined(typeof(PluginInfo), false) == false)
            {
                throw new InvalidOperationException();
            }
        }

        public abstract void Dispose();
        public abstract void Impact();
    }
}
