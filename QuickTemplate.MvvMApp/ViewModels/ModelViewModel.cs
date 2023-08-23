//@BaseCode
//MdStart
using CommonBase.Extensions;
using QuickTemplate.MvvMApp.Views;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuickTemplate.MvvMApp.ViewModels
{
    using TGenre = Models.ModelObject;

    public abstract partial class ModelViewModel<TModel, TEntity> : BaseViewModel
        where TModel : Models.ModelObject, new()
        where TEntity : Logic.Models.ModelObject, new()
    {
        #region fields
        private TModel? _model;
        #endregion fields

        #region properties
        public TModel Model
        {
            get => _model ??= new TModel();
            set
            {
                _model = value;
                OnPropertiesChanged();
            }
        }
        public int Id
        {
            get => Model.Id;
            set => Model.Id = value;
        }
        #endregion properties

        protected virtual void OnPropertiesChanged(params string[] propertyNames)
        {
            foreach (var item in propertyNames)
            {
                OnPropertyChanged(item);
            }
        }
        #region commands
        public ICommand CommandSave => RelayCommand.Create(p => Save());
        public ICommand CommandClose => RelayCommand.Create(p => Close());
        #endregion commands

        #region methods
        public abstract BaseContracts.IDataAccess<TEntity> CreateController();
        protected virtual void BeforeSave(ref bool handled) { }
        private async void Save()
        {
            bool handled = false;

            BeforeSave(ref handled);
            if (handled == false)
            {
                bool error = false;
                string errorMessage = string.Empty;
                using var ctrl = CreateController();

                Task.Run(async () =>
                {
                    try
                    {
                        var entity = ctrl.Create();

                        entity.CopyFrom(Model);
                        if (Model.Id == 0)
                        {
                            await ctrl.InsertAsync(entity).ConfigureAwait(false);
                        }
                        else
                        {
                            await ctrl.UpdateAsync(entity).ConfigureAwait(false);
                        }
                        await ctrl.SaveChangesAsync().ConfigureAwait(false);
                    }
                    catch (System.Exception ex)
                    {
                        error = true;
                        errorMessage = ex.Message;
                    }
                }).Wait();

                AfterSave();

                if (error)
                {
                    await MessageBox.ShowAsync(Window, errorMessage, "Löschen", MessageBox.MessageBoxButtons.Ok);
                }
                else
                {
                    Close();
                }
            }
            else
            {
                AfterSave();
            }
        }
        protected virtual void AfterSave() { }
        protected virtual void Close()
        {
            Window?.Close();
        }
        #endregion methods
    }
}
//MdEnd