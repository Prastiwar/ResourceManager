using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Models;
using RPGDataEditor.Wpf.Controls;

namespace RPGDataEditor.Minecraft.Wpf.Controls
{
    public class NpcJobView : RPGDataEditor.Wpf.Controls.NpcJobView
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("None", null),
            new TypeSource("Trader", typeof(TraderNpcJob)),
            new TypeSource("Guard", typeof(GuardNpcJob)),
        };

        protected override TypeSource[] GetSources() => sources;
    }
}
