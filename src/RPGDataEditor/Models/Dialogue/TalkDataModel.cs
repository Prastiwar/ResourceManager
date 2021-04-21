using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class TalkDataModel
    {
        public int TalkRange { get; set; }

        public IList<int> InitationDialogues { get; private set; }
    }
}
