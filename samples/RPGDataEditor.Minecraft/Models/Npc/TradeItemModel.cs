﻿using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Minecraft.Models
{
    public class TradeItemModel : Core.Models.TradeItemModel
    {
        private string nbt;
        public string Nbt {
            get => nbt;
            set => SetProperty(ref nbt, value);
        }
    }
}