//@BaseCode
//MdStart
namespace QuickTemplate.WpfApp.ViewModels
{
    using CommonBase.Contracts;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public abstract partial class EntityViewModel<T> : BaseViewModel
        where T : IIdentifyable, new()
    {
        #region fields
        private ICommand? cmdSave = null;
        private ICommand? cmdClose = null;
        private T entity = new();
        #endregion fields

        #region properties
        public T Entity
        {
            get => entity ??= new T();
            set
            {
                entity = value;
                OnPropertiesChanged();
            }
        }
        public IdType Id
        {
            get { return Entity.Id; }
        }
        public virtual ICommand CommandSave
        {
            get
            {
                return RelayCommand.Create(ref cmdSave, p =>
                {
                    Save();
                },
                p => true);
            }
        }
        public virtual ICommand CommandClose
        {
            get
            {
                return RelayCommand.Create(ref cmdClose, p =>
                {
                    Window?.Close();
                },
                p => true);
            }
        }
        #endregion properties

        #region methods
        public abstract IDataAccess<T> CreateController();
        protected virtual void OnPropertiesChanged()
        {
            OnPropertyChanged(nameof(Id));
        }
        protected void BeforeLoad() { }
        public virtual async Task LoadAsync(IdType id)
        {
            using var ctrl = CreateController();
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

            BeforeLoad();
            if (entity != null)
            {
                Entity.CopyFrom(entity);
                OnPropertiesChanged();
            }
            AfterLoad();
        }
        protected virtual void AfterLoad() { }
        protected virtual void BeforeSave() { }
        public virtual void Save()
        {
            var error = false;

            BeforeSave();
            Task.Run(async () =>
            {
                using var ctrl = CreateController();

                try
                {
                    var dbEntity = default(T);

                    if (Entity.Id != default)
                    {
                        dbEntity = await ctrl.GetByIdAsync(Entity.Id).ConfigureAwait(false);

                        if (dbEntity != null)
                        {
                            dbEntity.CopyFrom(this);
                            dbEntity = await ctrl.UpdateAsync(dbEntity).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        dbEntity = new T();

                        dbEntity.CopyFrom(this);
                        dbEntity = await ctrl.InsertAsync(dbEntity).ConfigureAwait(false);
                    }
                    await ctrl.SaveChangesAsync().ConfigureAwait(false);

                    Entity.CopyFrom(dbEntity!);
                }
                catch (Exception ex)
                {
                    error = true;
                    MessageBox.Show(ex.Message, "Save", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }).Wait();
            AfterSave();

            if (error == false)
            {
                Window?.Close();
            }
        }
        protected virtual void AfterSave() { }
        protected virtual void BeforeDelete() { }
        public virtual void Delete(IdType id)
        {
            var error = false;
            var result = MessageBox.Show($"Are you sure you want to delete the entry?", "Save", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Task.Run(async () =>
                {
                    using var ctrl = CreateController();

                    try
                    {
                        BeforeDelete();
                        await ctrl.DeleteAsync(id).ConfigureAwait(false);
                        await ctrl.SaveChangesAsync().ConfigureAwait(false);
                        AfterDelete();
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        MessageBox.Show(ex.Message, "Delete", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }).Wait();
            }

            if (error == false)
            {
                OnPropertiesChanged();
            }
        }
        protected virtual void AfterDelete() { }
        #endregion methods
    }
}
//MdEnd
