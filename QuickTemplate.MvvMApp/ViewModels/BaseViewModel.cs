//@BaseCode
//MdStart
using ReactiveUI;
using System.Runtime.CompilerServices;

namespace QuickTemplate.MvvMApp.ViewModels
{
    public class BaseViewModel : ReactiveObject
    {
        public virtual void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            this.RaisePropertyChanged(propertyName);
        }
    }
}
//MdEnd