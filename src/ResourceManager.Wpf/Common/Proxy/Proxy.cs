using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ResourceManager.Wpf
{
    public class Proxy<T> : INotifyPropertyChanged
    {
        public Proxy(int index, IList<T> source)
        {
            Index = index;
            Source = source;
        }

        public T Value {
            get => Source[Index];
            set {
                if (!Source[Index].Equals(value))
                {
                    Source[Index] = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Index { get; private set; }
        public IList<T> Source { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
