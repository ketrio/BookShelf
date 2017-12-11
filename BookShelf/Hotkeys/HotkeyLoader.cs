using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookShelf.Hotkeys
{
    public static class HotkeyLoader
    {
        public static RoutedCommand SaveCommand { get; set; } = new RoutedCommand();
        public static RoutedCommand LoadCommand { get; set; } = new RoutedCommand();

        static HotkeyLoader()
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
}
