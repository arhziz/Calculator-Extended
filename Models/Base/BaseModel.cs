using PropertyChanged;
using System.ComponentModel;

namespace Calculator_Extended
{
    [AddINotifyPropertyChangedInterface]
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => {};
    }
}
