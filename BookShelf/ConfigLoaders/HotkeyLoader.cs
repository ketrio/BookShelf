using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookShelf.ConfigLoaders
{
    public static class HotkeyConfigLoader
    {
        public static RoutedCommand SaveCommand { get; set; } = new RoutedCommand();
        public static RoutedCommand LoadCommand { get; set; } = new RoutedCommand();

        static HotkeyConfigLoader()
        {
            KeyGestureConverter converter = new KeyGestureConverter();
            var saveHotkey = ConfigurationManager.AppSettings["SaveHotkey"];
            var loadHotkey = ConfigurationManager.AppSettings["LoadHotkey"];

            if (saveHotkey != null)
                SaveCommand.InputGestures.Add(converter.ConvertFrom(saveHotkey) as InputGesture);
            if (loadHotkey != null)
                LoadCommand.InputGestures.Add(converter.ConvertFrom(loadHotkey) as InputGesture);
        }
    }

    public static class IconConfigLoader
    {
        public static string File { get; set; } = @"pack://application:,,,/Images/file.png";
        public static string Add { get; set; } = @"pack://application:,,,/Images/library.png";
        public static string Plugins { get; set; } = @"pack://application:,,,/Images/ext.png";
        public static string Edit { get; set; } = @"pack://application:,,,/Images/edit.png";
        public static string Delete { get; set; } = @"pack://application:,,,/Images/delete.png";
        public static string Carousel { get; set; } = @"pack://application:,,,/Images/carousel.png";

        static IconConfigLoader()
        {
            var section = ConfigurationManager.GetSection("iconPathSection") as Dictionary<string, string>;
            if (section == null) return;

            if (section.TryGetValue("file", out string file)) File = file;
            if (section.TryGetValue("add", out string add)) Add = add;
            if (section.TryGetValue("plugins", out string plugins)) Plugins = plugins;
            if (section.TryGetValue("edit", out string edit)) Edit = edit;
            if (section.TryGetValue("delete", out string delete)) Delete = delete;
            if (section.TryGetValue("carousel", out string carousel)) Carousel = carousel;
        }
    }
}
