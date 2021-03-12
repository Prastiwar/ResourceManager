using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Serialization;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestTabViewModel : SimpleCategorizedTabViewModel<QuestModel>
    {
        public QuestTabViewModel(ViewModelContext context) : base(context) { }

        protected override string RelativePath => "quests";

        protected override async Task<EditorResults> OpenEditorAsync(SimpleIdentifiableData model)
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
            EditorResults results = await base.OpenEditorAsync(model);
            JsonConvert.DefaultSettings = cachedSettings;
            return results;
        }

        protected override QuestModel CreateNewExactModel(SimpleIdentifiableData model) => new QuestModel() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as SimpleCategorizedData).Category
        };
    }
}
