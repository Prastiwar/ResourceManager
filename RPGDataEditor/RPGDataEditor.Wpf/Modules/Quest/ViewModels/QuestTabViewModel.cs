using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Serialization;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestTabViewModel : CategorizedTabViewModel<QuestModel>
    //public class QuestTabViewModel : SimpleCategorizedTabViewModel<QuestModel>
    {
        public QuestTabViewModel(ViewModelContext context) : base(context) { }

        protected override string RelativePath => "quests";

        //protected override async Task OpenEditorAsync(SimpleIdentifiableData model)
        //{
        //    System.Func<JsonSerializerSettings> cachedSettings = JsonConvert.DefaultSettings;
        //    JsonConvert.DefaultSettings = () => {
        //        JsonSerializerSettings settings = cachedSettings();
        //        if (settings.ContractResolver is PropertyContractResolver resolver)
        //        {
        //            resolver.IgnoreProperty(typeof(InteractQuestTask), nameof(InteractQuestTask.Completed));
        //            resolver.IgnoreProperty(typeof(KillQuestTask), nameof(KillQuestTask.Counter));
        //            resolver.IgnoreProperty(typeof(ReachQuestTask), nameof(ReachQuestTask.Reached));
        //        }
        //        return settings;
        //    };
        //    await base.OpenEditorAsync(model);
        //    JsonConvert.DefaultSettings = cachedSettings;
        //}

        protected override async Task OpenEditorAsync(QuestModel model)
        {
            System.Func<JsonSerializerSettings> cachedSettings = JsonConvert.DefaultSettings;
            JsonConvert.DefaultSettings = () => {
                JsonSerializerSettings settings = cachedSettings();
                if (settings.ContractResolver is PropertyContractResolver resolver)
                {
                    resolver.IgnoreProperty(typeof(InteractQuestTask), nameof(InteractQuestTask.Completed));
                    resolver.IgnoreProperty(typeof(KillQuestTask), nameof(KillQuestTask.Counter));
                    resolver.IgnoreProperty(typeof(ReachQuestTask), nameof(ReachQuestTask.Reached));
                }
                return settings;
            };
            await base.OpenEditorAsync(model);
            JsonConvert.DefaultSettings = cachedSettings;
        }
    }
}
