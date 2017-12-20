using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
        public static BitmapImage File { get; set; }
        public static BitmapImage Add { get; set; }
        public static BitmapImage Plugins { get; set; }
        public static BitmapImage Edit { get; set; }
        public static BitmapImage Delete { get; set; }
        public static BitmapImage Carousel { get; set; }

        static IconConfigLoader()
        {
            var section = ConfigurationManager.GetSection("iconPathSection") as Dictionary<string, string>;
            if (section == null) return;

            if (section.TryGetValue("file", out string file)) File = new BitmapImage(new Uri(file));
            if (section.TryGetValue("add", out string add)) Add = new BitmapImage(new Uri(add));
            if (section.TryGetValue("plugins", out string plugins)) Plugins = new BitmapImage(new Uri(plugins));
            if (section.TryGetValue("edit", out string edit)) Edit = new BitmapImage(new Uri(edit));
            if (section.TryGetValue("delete", out string delete)) Delete = new BitmapImage(new Uri(delete));
            if (section.TryGetValue("carousel", out string carousel)) Carousel = new BitmapImage(new Uri(carousel));
        }
    }
}
