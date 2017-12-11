using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BookShelf.ConfigHelpers
{
    public class PluginDirectories : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            List<string> myConfigObject = new List<string>();

            foreach (XmlNode childNode in section.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Comment) continue;
                foreach (XmlAttribute attrib in childNode.Attributes)
                {
                    myConfigObject.Add(attrib.Value);
                }
            }
            return myConfigObject;
        }
    }

    public class IconPathSection : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (XmlNode childNode in section.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Comment ||
                    childNode.Attributes["icon"].Value == "" ||
                    childNode.Attributes["location"].Value == "") continue;

                dictionary.Add(childNode.Attributes["icon"].Value, childNode.Attributes["location"].Value);
            }
            return dictionary;
        }
    }
}
