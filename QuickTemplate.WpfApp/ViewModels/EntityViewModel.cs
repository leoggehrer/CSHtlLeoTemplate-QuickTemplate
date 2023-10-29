//@BaseCode
//MdStart
namespace QuickTemplate.WpfApp.ViewModels
{
    using CommonBase.Contracts;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    
    /// <summary>
    /// Base class for view models representing an entity.
    /// </summary>
    /// <typeparam name="T">The type of entity</typeparam>
    /// <remarks>
    /// This class provides common functionality for view models
    /// that are used to manipulate and display data for a specific entity.
    /// </remarks>
    /// <summary>
    /// Gets or sets the entity associated with this view model.
    /// </summary>
    /// <summary>
    /// Gets the identifier of the entity.
    /// </summary>
    /// <summary>
    /// Gets the command for saving changes to the entity.
    /// </summary>
    /// <summary>
    /// Gets the command for closing the view model.
    /// </summary>
    /// <summary>
    /// Creates a new instance of the EntityViewModel class.
    /// </summary>
    /// <summary>
    /// Creates a new instance of the EntityViewModel class with the specified entity.
    /// </summary>
    /// <param name="entity">The entity to associate with this view model</param>
    /// <summary>
    /// Creates a data access controller for the entity.
    /// </summary>
    /// <returns>A data access controller for the entity</returns>
    /// <summary>
    /// Called when the properties of the view model change.
    /// </summary>
    /// <summary>
    /// Performs any actions that should be done before loading the entity.
    /// </summary>
    /// <summary>
    /// Loads the entity with the specified identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity to load</param>
    /// <returns>A task that represents the asynchronous load operation</returns>
    /// <summary>
    /// Performs any actions that should be done after loading the entity.
    /// </summary>
    /// <summary>
    /// Performs any actions that should be done before saving changes to the entity.
    /// </summary>
    /// <summary>
    /// Saves the changes made to the entity.
    /// </summary>
    /// <summary>
    /// Performs any actions that should be done after saving changes to the entity.
    /// </summary>
    /// <summary>
    /// Performs any actions that should be done before deleting the entity.
    /// </summary>
    /// <summary>
    /// Deletes the entity with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete</param>
    /// <summary>
    /// Performs any actions that should be done after deleting the entity.
    /// </summary>
    public abstract partial class EntityViewModel<T> : BaseViewModel
    where T : IIdentifyable, new()
    {
        #region fields
        private ICommand? cmdSave = null;
        private ICommand? cmdClose = null;
        private T entity = new();
        #endregion fields
        
        #region properties
        ///<summary>
        /// Gets or sets the value of the Entity property.
        ///</summary>
        ///<value>
        /// The current value of the Entity property.
        ///</value>
        public T Entity
        {
            get => entity ??= new T();
            set
            {
                entity = value;
                OnPropertiesChanged();
            }
        }
        /// <summary>
        /// Gets the Id of the entity.
        /// </summary>
        /// <remarks>
        /// The Id property provides the unique identifier of the entity.
        /// </remarks>
        /// <returns>The unique Id of the entity.</returns>
        public IdType Id
        {
            get { return Entity.Id; }
        }
        /// <summary>
        /// Gets or sets the command to save data.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the command used to close the window.
        /// Returns a command that executes the Close method of the window it belongs to.
        /// </summary>
        /// <value>The command used to close the window.</value>
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
        /// <summary>
        /// Creates a controller object for accessing data of type T.
        /// </summary>
        /// <typeparam name="T">The type of data to be accessed.</typeparam>
        /// <returns>An implementation of IDataAccess&lt;T&gt; that provides methods to access data of type T.</returns>
        public abstract IDataAccess<T> CreateController();
        /// <summary>
        /// Notifies when the properties have changed.
        /// </summary>
        protected virtual void OnPropertiesChanged()
        {
            OnPropertyChanged(nameof(Id));
        }
        /// <summary>
        /// This method is called before a load operation.
        /// </summary>
        protected void BeforeLoad() { }
        /// <summary>
        /// Loads the entity asynchronously based on the provided id.
        /// </summary>
        /// <param name="id">The id of the entity to load.</param>
        /// <returns>A task representing the asynchronous loading operation.</returns>
        /// <remarks>
        /// This method retrieves the entity with the specified id using the controller.
        /// Before loading, the <see cref="BeforeLoad"/> method is invoked.
        /// If the entity is found, its properties are copied to the current entity object using the <see cref="Entity.CopyFrom"/> method.
        /// Finally, the <see cref="OnPropertiesChanged"/> and <see cref="AfterLoad"/> methods are called.
        /// </remarks>
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
        /// <summary>
        /// Represents the method that is called after the load operation has completed.
        /// </summary>
        protected virtual void AfterLoad() { }
        /// <summary>
        /// This method is executed before saving any changes to the data.
        /// </summary>
        protected virtual void BeforeSave() { }
        /// <summary>
        /// Saves the changes made to the entity.
        /// </summary>
        /// <remarks>
        /// The method saves the changes made to the entity by either updating an existing entity or inserting a new entity into the database.
        /// </remarks>
        /// <seealso cref="BeforeSave"/>
        /// <seealso cref="AfterSave"/>
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
        /// <summary>
        /// This method is called after the save operation has been completed.
        /// </summary>
        protected virtual void AfterSave() { }
        /// <summary>
        /// This method is called before the deletion process starts and can be overridden in derived classes to perform necessary actions or validations.
        /// </summary>
        /// <remarks>
        /// Place any additional information or usage notes here.
        /// </remarks>
        protected virtual void BeforeDelete() { }
        /// <summary>
        /// Deletes an entry with the specified ID from the database.
        /// </summary>
        /// <param name="id">The ID of the entry to delete.</param>
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
        /// <summary>
        /// This method is called after a delete operation has been performed.
        /// </summary>
        protected virtual void AfterDelete() { }
        #endregion methods
    }
}
//MdEnd

