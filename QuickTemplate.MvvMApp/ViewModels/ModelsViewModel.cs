//@BaseCode
//MdStart
using QuickTemplate.MvvMApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuickTemplate.MvvMApp.ViewModels
{
    public abstract class ModelsViewModel<TModel, TEntity> : ModelViewModel<TModel, TEntity>
        where TModel : Models.ModelObject, new()
        where TEntity : Logic.Models.ModelObject, new()
    {
        #region fields
        private string _modelFilter = string.Empty;
        private TModel? _selectedModel;
        private List<TModel> _models = new();
        #endregion fields

        #region properties
        public BaseViewModel OtherViewModel { get; private set; }
        public TModel[] Models => _models.ToArray();
        public string ModelFilter
        {
            get => _modelFilter;
            set
            {
                _modelFilter = value;
                OnPropertyChanged(nameof(Models));
            }
        }
        public TModel? SelectedModel
        {
            get => _selectedModel;
            set
            {
                _selectedModel = value;
                OnPropertyChanged(nameof(CommandEditModel));
                OnPropertyChanged(nameof(CommandDeleteModel));
            }
        }
        protected abstract ModelView<TModel, TEntity>? ModelView { get; }
        #endregion properties

        #region commands
        public ICommand CommandAddModel => RelayCommand.Create((p) => AddModel(), p => ModelView != null);
        public ICommand CommandEditModel => RelayCommand.Create(p => EditModel(), p => ModelView != null && SelectedModel != null);
        public ICommand CommandDeleteModel => RelayCommand.Create(p => DeleteModel(), p => ModelView != null && SelectedModel != null);
        #endregion commands

        #region constructions
        protected ModelsViewModel(BaseViewModel otherViewModel)
        {
            OtherViewModel = otherViewModel;
            OnPropertiesChanged(nameof(Models));
        }
        #endregion constructions

        #region overrides
        internal override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (propertyName == nameof(Models))
            {
                Task.Run(LoadModelsAsync);
            }
            else
            {
                OnOtherPropertyChanged(propertyName);
            }
        }
        internal virtual void OnOtherPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OtherViewModel.OnPropertyChanged($"{typeof(TModel).Name}_{propertyName}");
        }
        #endregion overrides

        #region methods
        protected abstract Func<TEntity, TModel> ConvertTo { get; }
        protected abstract Predicate<TEntity> LoadPredicate { get; }
        protected virtual async void AddModel()
        {
            var view = ModelView;

            if (view != null && OtherViewModel.Window != null)
            {
                await view.ShowDialog(OtherViewModel.Window);
                OnPropertyChanged(nameof(Models));
            }
        }
        protected virtual async void EditModel()
        {
            var view = ModelView;

            if (view != null && OtherViewModel.Window != null)
            {
                if (view.ViewModel != null)
                {
                    view.ViewModel!.Model = SelectedModel!;
                }
                await view.ShowDialog(OtherViewModel.Window);
                OnPropertyChanged(nameof(Models));
            }
        }
        protected virtual async void DeleteModel()
        {
            var result = await MessageBox.ShowAsync(Window, $"Soll der Eintrag '{SelectedModel}' gelöscht werden?", "Löschen", MessageBox.MessageBoxButtons.YesNo);

            if (result == MessageBox.MessageBoxResult.Yes)
            {
                bool error = false;
                string errorMessage = string.Empty;

                Task.Run(async () =>
                {
                    try
                    {
                        using var ctrl = CreateController();

                        await ctrl.DeleteAsync(SelectedModel!.Id).ConfigureAwait(false);
                        await ctrl.SaveChangesAsync().ConfigureAwait(false);
                    }
                    catch (System.Exception ex)
                    {
                        error = true;
                        errorMessage = ex.Message;
                    }
                }).Wait();

                if (error)
                {
                    await MessageBox.ShowAsync(Window, errorMessage, "Löschen", MessageBox.MessageBoxButtons.Ok);
                }
                else
                {
                    OnPropertyChanged(nameof(Models));
                }
            }
            #endregion methods
        }
        protected virtual async Task LoadModelsAsync()
        {
            using var ctrl = CreateController();
            var items = await ctrl.GetAllAsync().ConfigureAwait(false);
            var result = items.Where(e => LoadPredicate(e));

            _models.Clear();
            _models.AddRange(result.Select(e => ConvertTo(e)));
            SelectedModel = SelectedModel != null ? _models.FirstOrDefault(m => m.Id == SelectedModel.Id) : null;
            OnOtherPropertyChanged(nameof(Models));
            OnOtherPropertyChanged(nameof(SelectedModel));
        }
    }
}
//MdEnd