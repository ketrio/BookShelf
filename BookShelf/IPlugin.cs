using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShelf
{
    [AttributeUsage(AttributeTargets.Class)]
    class PluginInfoAttribute: Attribute
    {
        string Name;
        string Author;
        string Description;
    }
    
    interface IPlugin : IDisposable
    {
        object Owner { set; }
        bool Enabled { get; set; }

        void Run();
    }
}
