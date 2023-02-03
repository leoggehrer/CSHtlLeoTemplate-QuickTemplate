//@BaseCode
//MdStart
using Avalonia.Controls;

namespace QuickTemplate.MvvMApp.Views
{
    public partial class ModelView<TModel, TEntity> : Window
        where TModel : Models.ModelObject, new()
        where TEntity : Logic.Models.ModelObject, new()
    {
        internal ViewModels.ModelViewModel<TModel, TEntity>? ViewModel { get; set; }
    }
}
//MdEnd