﻿using BookShelf.PluginSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GreenAndBlackPlugin
{
    [PluginInfo("Theme", "A plugin that allows you to change built-in color palette.", "Vlad Gutkovsky", "0.1")]
    public class Class1 : BookPlugin
    {
        public Class1(Application app) : base(app)
        {

        }

        public override void Dispose()
        {

        }

        public override void Impact()
        {
            var dict = new ResourceDictionary();
            
            var greenList = new Style(typeof(Frame));
            greenList.Setters.Add(new Setter(Frame.BackgroundProperty, Brushes.PeachPuff));

            var black = new Style(typeof(TextBlock));
            black.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.DarkBlue));
            
            dict.Add(typeof(Frame), greenList);
            dict.Add(typeof(TextBlock), black);

            Application.Current.Resources = dict;
        }
    }
}
