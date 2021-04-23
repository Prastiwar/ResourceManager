﻿using RPGDataEditor.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class TalkLineAutoTemplate : AutoTemplate<TalkLine>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            StackPanel panel = new StackPanel() {
                Orientation = Orientation.Horizontal
            };
            panel.Children.Add(new AutoControl() { PropertyName = nameof(TalkLine.Text) });
            panel.Children.Add(new AutoControl() { PropertyName = nameof(TalkLine.SoundId) });
            return panel;
        }
    }
}
