//@BaseCode
//MdStart
using System.ComponentModel;
using System.Windows;

namespace QuickTemplate.WpfApp.ViewModels
{
    /// <summary>
    /// Represents a base view model that implements the INotifyPropertyChanged interface.
    /// </summary>
    public abstract partial class BaseViewModel : INotifyPropertyChanged
    {
        #region fields
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion fields
        
        #region properties
        /// <summary>
        /// Gets or sets the Window object.
        /// </summary>
        /// <value>
        /// The Window object.
        /// </value>
        public Window? Window { get; set; }
        /// <summary>
        /// Raises the property changed event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion properties
    }
}
//MdEnd

