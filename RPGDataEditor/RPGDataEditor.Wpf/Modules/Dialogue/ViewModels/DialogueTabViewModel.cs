using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : TabViewModel
    {
        public DialogueTabViewModel(ViewModelContext context) : base(context) { }

        private ObservableCollection<DialogueModel> currentCategoryDialogues;
        public ObservableCollection<DialogueModel> CurrentCategoryDialogues {
            get => currentCategoryDialogues;
            protected set => SetProperty(ref currentCategoryDialogues, value);
        }

        private ObservableCollection<DialogueModel> dialogues;
        public ObservableCollection<DialogueModel> Dialogues {
            get => dialogues;
            protected set => SetProperty(ref dialogues, value);
        }

        private ObservableCollection<string> dialogueCategories;
        public ObservableCollection<string> DialogueCategories {
            get => dialogueCategories;
            protected set => SetProperty(ref dialogueCategories, value);
        }

        private ICommand addDialogueCommand;
        public ICommand AddDialogueCommand => addDialogueCommand ??= new DelegateCommand<string>(CreateDialogue);

        private ICommand removeDialogueCommand;
        public ICommand RemoveDialogueCommand => removeDialogueCommand ??= new DelegateCommand<DialogueModel>(RemoveDialogue);

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand => addCategoryCommand ??= new DelegateCommand(CreateCategory);

        private ICommand removeCategoryCommand;
        public ICommand RemoveCategoryCommand => removeCategoryCommand ??= new DelegateCommand<string>(RemoveCategory);

        private ICommand showCategoryCommand;
        public ICommand ShowCategoryCommand => showCategoryCommand ??= new DelegateCommand<string>(ShowCategory);

        private ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<DialogueModel>(OpenEditor);

        protected const string RelativePath = "dialogues";

        private async void OpenEditor(DialogueModel dialogue)
        {
            DialogueModel copiedDialogue = (DialogueModel)dialogue.DeepClone();
            bool save = await Context.DialogService.ShowModelDialogAsync(copiedDialogue);
            if (save)
            {
                //dialogue.CopyValues(copiedDialogue);
                //string json = JsonConvert.SerializeObject(dialogue);
                //await Context.Session.SaveJsonFileAsync(GetRelativeFilePath(dialogue), json);
            }
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            Dialogues = new ObservableCollection<DialogueModel>();
            DialogueCategories = new ObservableCollection<string>();
            string[] jsons = new string[0];
            try
            {
                jsons = await Session.LoadJsonsAsync(RelativePath);
            }
            catch (System.Exception ex)
            {
                Context.SnackbarService.Enqueue("Failed to load jsons, you can try again by refreshing tab");
            }
            foreach (string json in jsons)
            {
                try
                {
                    DialogueModel dialogue = JsonConvert.DeserializeObject<DialogueModel>(json);
                    Dialogues.Add(dialogue);
                }
                catch (System.Exception ex)
                {
                    Context.SnackbarService.Enqueue("Found invalid json");
                }
            }
            DialogueCategories.AddRange(Dialogues.Select(x => x.Category).Distinct());
        }

        protected virtual string GetRelativeFilePath(DialogueModel dialogue) => RelativePath + "/" + dialogue.GetId() + ".json";

        public void ShowCategory(string category)
        {
            CurrentCategoryDialogues = new ObservableCollection<DialogueModel>();
            CurrentCategoryDialogues.AddRange(Dialogues.Where(x => string.Compare(x.Category, category) == 0));
        }

        private async void RemoveDialogue(DialogueModel dialogue)
        {
            bool removed = Dialogues.Remove(dialogue);
            if (removed)
            {
                string relativeFilePath = GetRelativeFilePath(dialogue);
                bool deleted = await Session.DeleteFileAsync(relativeFilePath);
                if (deleted)
                {
                    CurrentCategoryDialogues.Remove(dialogue);
                }
                else
                {
                    Dialogues.Add(dialogue);
                }
            }
        }

        private async void CreateDialogue(string category)
        {
            DialogueModel newDialogue = new DialogueModel();
            int nextId = Dialogues.Count > 0 ? Dialogues.Max(x => x.GetId()) + 1 : 0;
            newDialogue.SetId(nextId);
            newDialogue.Title = "New dialogue";
            newDialogue.Category = category;
            string json = JsonConvert.SerializeObject(newDialogue);
            string relativeFilePath = GetRelativeFilePath(newDialogue);
            bool created = await Session.SaveJsonFileAsync(relativeFilePath, json);
            if (created)
            {
                Dialogues.Add(newDialogue);
                CurrentCategoryDialogues.Add(newDialogue);
            }
        }

        public void CreateCategory()
        {
            string newName = "New Category";
            int i = 1;
            while (DialogueCategories.IndexOf(newName) != -1)
            {
                newName = $"New Category ({i})";
                i++;
            }
            DialogueCategories.Add(newName);
        }

        public async Task<bool> RenameCategoryAsync(string oldCategory, string newCategory)
        {
            if (DialogueCategories.IndexOf(newCategory) != -1)
            {
                return false;
            }
            int oldCategoryIndex = DialogueCategories.IndexOf(oldCategory);
            bool removed = DialogueCategories.Remove(oldCategory);
            if (!removed)
            {
                return false;
            }
            DialogueCategories.Insert(oldCategoryIndex, newCategory);
            foreach (DialogueModel dialogue in Dialogues.Where(x => string.Compare(x.Category, oldCategory) == 0))
            {
                string relativeFilePath = GetRelativeFilePath(dialogue);
                dialogue.Category = newCategory;
                string json = JsonConvert.SerializeObject(dialogue);
                bool saved = await Session.SaveJsonFileAsync(relativeFilePath, json);
            }
            return true;
        }

        public async void RemoveCategory(string category)
        {
            bool removed = DialogueCategories.Remove(category);
            if (removed)
            {
                foreach (DialogueModel dialogue in Dialogues.Where(x => string.Compare(x.Category, category) == 0))
                {
                    string relativeFilePath = GetRelativeFilePath(dialogue);
                    bool deleted = await Session.DeleteFileAsync(relativeFilePath);
                }
            }
        }
    }
}
