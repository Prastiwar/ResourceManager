using System.Collections.Generic;

namespace RPGDataEditor.Sample.Models
{
    public class TalkData
    {
        public int TalkRange { get; set; }

        public IList<int> InitationDialogues { get; set; }
    }
}
