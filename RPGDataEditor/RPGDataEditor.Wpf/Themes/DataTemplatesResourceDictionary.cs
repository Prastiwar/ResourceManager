using RPGDataEditor.Core.Models;
using RPGDataEditor.Wpf.Controls;
using System.Collections.Generic;
using System.Windows;

namespace RPGDataEditor.Wpf.Themes
{
    public partial class DataTemplatesResourceDictionary : ResourceDictionary
    {
        protected virtual void RequirementsListCardItemTemplate_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e)
        {
            if (AttachProperties.GetData(sender as UIElement) is IList<PlayerRequirementModel> list)
            {
                e.ChangeTypeInList(list);
            }
        }
    }
}
