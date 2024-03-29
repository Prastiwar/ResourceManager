﻿using ResourceManager.Wpf.Views;

namespace ResourceManager.Wpf
{
    public static class DialogNames
    {
        public static string UpdateDialog { get; set; } = typeof(UpdateDialog).Name;
        public static string PickerDialog { get; set; } = typeof(PickerDialog).Name;
        public static string ConnectionDialog { get; set; } = typeof(ConnectionDialog).Name;
    }
}
