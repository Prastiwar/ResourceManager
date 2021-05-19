using MediatR;
using RPGDataEditor.Mvvm.Navigation;

namespace RPGDataEditor.Mvvm.Commands
{
    public class ShowDialogQuery : IRequest<IDialogResult>
    {
        public ShowDialogQuery(string name, IDialogParameters parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; }
        public IDialogParameters Parameters { get; }
    }
}
