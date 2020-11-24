using Prism.Mvvm;
using Prism.Navigation;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        public ViewModelBase(SessionContext context) => Context = context;

        public SessionContext Context { get; }

        public virtual void Destroy() { }
    }
}
