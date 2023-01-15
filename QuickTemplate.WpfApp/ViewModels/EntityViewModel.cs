//@BaseCode
//MdStart
namespace QuickTemplate.WpfApp.ViewModels
{
    using QuickTemplate.Logic.Contracts;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public abstract partial class EntityViewModel<T> : BaseViewModel
        where T : Logic.Entities.EntityObject, new()
    {
        #region fields
        private ICommand? cmdSave = null;
        private ICommand? cmdClose = null;
        #endregion fields

        #region properties
        public IdType Id
        {
            get { return Entity.Id; }
        }
        protected T Entity { get; set; } = new();
        public virtual ICommand CommandSave
        {
            get
            {
                return RelayCommand.CreateCommand(ref cmdSave, p =>
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
                return RelayCommand.CreateCommand(ref cmdClose, p =>
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
        public virtual void Load(IdType id)
        {
            using var ctrl = CreateController();
            var entity = ctrl.GetByIdAsync(id).Result;

            if (entity != null)
            {
                Entity.CopyFrom(entity);
                OnPropertiesChanged();
            }
        }
        public virtual void Save()
        {
            var error = false;

            Task.Run(async () =>
            {
                using var ctrl = CreateController();

                try
                {
                    if (Entity.Id != default(IdType))
                    {
                        var dbEntity = await ctrl.GetByIdAsync(Entity.Id);

                        if (dbEntity != null)
                        {
                            dbEntity.CopyFrom(this);
                            dbEntity = await ctrl.UpdateAsync(dbEntity);
                            Entity.CopyFrom(dbEntity);
                        }
                    }
                    else
                    {
                        var dbEntity = new T();

                        dbEntity.CopyFrom(this);
                        dbEntity = await ctrl.InsertAsync(dbEntity);
                        Entity.CopyFrom(dbEntity);
                    }
                    var count = await ctrl.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    error = true;
                    MessageBox.Show(ex.Message, "Save", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }).Wait();

            if (error == false)
            {
                Window?.Close();
            }
        }
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
                        await ctrl.DeleteAsync(id);
                        await ctrl.SaveChangesAsync();
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
        #endregion methods
    }
}
//MdEnd
