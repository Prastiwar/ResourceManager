using MediatR;
using RPGDataEditor.Mvvm.Navigation;
using RPGDataEditor.Mvvm.Services;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm.Commands
{
    public class ShowDialogHandler : IRequestHandler<ShowDialogQuery, IDialogResult>
    {
        public ShowDialogHandler(IDialogService service) => Service = service;

        public IDialogService Service { get; }

        public Task<IDialogResult> Handle(ShowDialogQuery request, CancellationToken cancellationToken) => Service.ShowDialogAsync(request.Name, request.Parameters);
    }
}
