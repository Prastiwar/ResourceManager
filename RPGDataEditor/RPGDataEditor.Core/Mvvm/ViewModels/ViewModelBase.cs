using Prism.Mvvm;
using Prism.Navigation;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        public ViewModelBase(ViewModelContext context)
        {
            Context = context;
            Session = context.Session;
        }

        public ViewModelContext Context { get; }

        public ISessionContext Session { get; }

        public virtual void Destroy() { }
    }
}
