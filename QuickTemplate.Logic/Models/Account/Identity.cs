//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using TEntity = Entities.Account.Identity;
    using TModel = Models.Account.Identity;
    /// <summary>
    /// Represents an identity model.
    /// </summary>
    public partial class Identity : VersionExtendedModel, Contracts.Account.IIdentity
    {
        /// <summary>
        /// Represents the static <see cref="Identity"/> class.
        /// </summary>
        static Identity()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// Represents a partial method called when the class is being constructed.
        /// </summary>
        /// <remarks>
        /// This method is only an implementation option in a partial class or structure.
        /// It is called during the class construction process, allowing you to perform
        /// any initialization logic specific to this partial class.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the <see cref="Identity"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor invokes the <see cref="Constructing"/> method and the <see cref="Constructed"/> method
        /// to perform necessary initialization tasks.
        /// </remarks>
        public Identity()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of an object.
        /// </summary>
        /// <remarks>
        /// This method is intended to be overridden in derived classes to perform specific actions
        /// during the construction phase of the object's lifecycle.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// This method is a partial method that can be implemented in a partial class.
        /// It is called when an object is constructed.
        /// </summary>
        /// <remarks>
        /// This method can be used for performing any initialization tasks required when an object is created.
        /// It is an optional method and may or may not be implemented.
        /// </remarks>
        partial void Constructed();
        /// <summary>
        /// Gets or sets the source entity.
        /// </summary>
        /// <value>
        /// The source entity.
        /// </value>
        new internal TEntity Source
        {
            get => (TEntity)(_source ??= new Entities.Account.SecureIdentity());
            set => _source = value;
        }
#if GUID_OFF
        /// <summary>
        /// Gets or sets the unique identifier of the object.
        /// </summary>
        /// <value>
        /// The unique identifier of the object.
        /// </value>
        public Guid Guid
        {
            get => Source.Guid;
            set => Source.Guid = value;
        }
#endif
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public System.String Name
        {
            get => Source.Name;
            set => Source.Name = value;
        }
        /// <summary>
        /// Gets or sets the email associated with the source object.
        /// </summary>
        /// <value>A string value representing an email address.</value>
        public System.String Email
        {
            get => Source.Email;
            set => Source.Email = value;
        }
        ///<summary>
        /// Gets or sets the timeout value in minutes.
        ///</summary>
        ///<value>
        /// The timeout value in minutes.
        ///</value>
        public System.Int32 TimeOutInMinutes
        {
            get => Source.TimeOutInMinutes;
            set => Source.TimeOutInMinutes = value;
        }
        /// <summary>
        /// Gets or sets the number of times the access to the source has failed.
        /// </summary>
        public System.Int32 AccessFailedCount
        {
            get => Source.AccessFailedCount;
            set => Source.AccessFailedCount = value;
        }
        /// <summary>
        /// Gets or sets the state of the module.
        /// </summary>
        /// <value>The state of the module.</value>
        public Modules.Common.State State
        {
            get => Source.State;
            set => Source.State = value;
        }
        /// <summary>
        /// Gets the roles associated with the account.
        /// </summary>
        /// <value>
        /// An array of <see cref="Contracts.Account.IRole"/> representing the roles associated with the account.
        /// </value>
        public Contracts.Account.IRole[] Roles => Source.Roles;
        
        /// <summary>
        /// Checks if the specified GUID exists as a role in the Source.
        /// </summary>
        /// <param name="guid">The GUID of the role to check.</param>
        /// <returns>True if the role exists in the Source; otherwise, false.</returns>
        public bool HasRole(Guid guid) => Source.HasRole(guid);
        
        /// <summary>
        /// Copies the properties of the given entity to the current instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="other">The entity from which to copy the properties.</param>
        /// <remarks>
        /// This method is called internally to copy the properties of the given entity to the current instance.
        /// It triggers the <see cref="BeforeCopyProperties"/> method before copying the properties and
        /// the <see cref="AfterCopyProperties"/> method after completing the copy operation.
        /// If the <see cref="BeforeCopyProperties"/> method does not handle the copy operation, the properties are copied.
        /// The properties that are copied are Id, Guid, Name, Email, TimeOutInMinutes, AccessFailedCount, and State.
        /// The method also calls the <see cref="CopyExendedProperties"/> method to copy any extended properties not included in the default ones.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="other"/> is null.</exception>
        internal void CopyProperties(TEntity other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Id = other.Id;
                Guid = other.Guid;
                Name = other.Name;
                Email = other.Email;
                TimeOutInMinutes = other.TimeOutInMinutes;
                AccessFailedCount = other.AccessFailedCount;
                State = other.State;
                CopyExendedProperties(other);
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// This method is called before copying properties from another entity.
        /// </summary>
        /// <param name="other">The other entity to copy properties from.</param>
        /// <param name="handled">A flag indicating if the copying process has been handled. This flag can be modified by the method.</param>
        partial void BeforeCopyProperties(TEntity other, ref bool handled);
        /// <summary>
        /// This method is called after copying the properties of the specified entity to the current entity.
        /// </summary>
        /// <param name="other">The entity from which the properties are copied.</param>
        /// <remarks>
        /// This method is intended to be overridden in partial classes. It provides a way to perform additional
        /// operations or modifications on the current entity after the properties have been copied.
        /// </remarks>
        /// <returns>Void</returns>
        partial void AfterCopyProperties(TEntity other);
        
        /// <summary>
        /// Copies the properties from the provided model object to this instance.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="other">The model object from which the properties are copied.</param>
        /// <remarks>
        /// This method invokes the "BeforeCopyProperties" hook before copying the properties from the other object.
        /// If the "handled" parameter is false after the hook invocation, the properties are copied from the other object to this instance.
        /// The properties that are copied include:
        /// - Id
        /// - Guid
        /// - Name
        /// - Email
        /// - TimeOutInMinutes
        /// - AccessFailedCount
        /// - State
        /// </remarks>
        internal void CopyProperties(TModel other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Id = other.Id;
                Guid = other.Guid;
                Name = other.Name;
                Email = other.Email;
                TimeOutInMinutes = other.TimeOutInMinutes;
                AccessFailedCount = other.AccessFailedCount;
                State = other.State;
                CopyExendedProperties(other);
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// This method is called before copying properties to the current model from another model.
        /// </summary>
        /// <param name="other">The model from which properties are being copied.</param>
        /// <param name="handled">A reference to a boolean flag indicating if the method has already been handled.</param>
        partial void BeforeCopyProperties(TModel other, ref bool handled);
        /// <summary>
        /// This method is called after copying properties from another object of type TModel.
        /// </summary>
        /// <param name="other">The other object of type TModel from which the properties are copied.</param>
        /// <remarks>
        /// Implement this method in a partial class to perform any additional operations or validation
        /// after copying the properties.
        /// This method is intended to be used in conjunction with the CopyProperties method.
        /// </remarks>
        partial void AfterCopyProperties(TModel other);
        
        /// <summary>
        /// Determines whether the current object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// True if the current object is equal to the <paramref name="obj"/> parameter; otherwise, false.
        /// </returns>
        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is TModel other)
            {
                result = IsEqualsWith(this.GetType().Name, other.GetType().Name) && Id == other.Id;
            }
            return result;
        }
        /// <summary>
        /// Overrides the GetHashCode method to calculate the hash code value for the current instance.
        /// </summary>
        /// <returns>
        /// An integer value representing the hash code of the current instance.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode(Guid, Name, Email, TimeOutInMinutes, AccessFailedCount, State);
        }
        
        /// <summary>
        /// Creates a new instance of the specified model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>A new instance of the model.</returns>
        // Create a new instance of the model
        public static TModel Create()
        {
            BeforeCreate();
            var result = new TModel();
            AfterCreate(result);
            return result;
        }
        /// <summary>
        /// Creates a new instance of the specified <typeparamref name="TModel"/> type
        /// by copying the properties from the provided <paramref name="other"/> object.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to create.</typeparam>
        /// <param name="other">The object to copy properties from.</param>
        /// <returns>A new instance of the specified <typeparamref name="TModel"/> type.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="other"/> is null.</exception>
        public static TModel Create(object other)
        {
            BeforeCreate(other);
            CommonBase.Extensions.ObjectExtensions.CheckArgument(other, nameof(other));
            var result = new TModel();
            CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Creates a new instance of the specified model with the properties copied from the provided model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to create.</typeparam>
        /// <param name="other">The model from which the properties will be copied.</param>
        /// <returns>A new instance of the specified model with copied properties.</returns>
        public static TModel Create(TModel other)
        {
            BeforeCreate(other);
            var result = new TModel();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Creates a new instance of a model object based on the provided entity object.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object to create.</typeparam>
        /// <typeparam name="TEntity">The type of the entity object.</typeparam>
        /// <param name="other">The entity object to create the model object from.</param>
        /// <returns>The newly created model object.</returns>
        internal static TModel Create(TEntity other)
        {
            BeforeCreate(other);
            var result = new TModel
            {
                Source = other
            };
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// This method is a hook that gets called before a creation operation, allowing for custom logic to be executed.
        /// </summary>
        static partial void BeforeCreate();
        /// <summary>
        /// This method is executed after a new instance of TModel is created.
        /// </summary>
        /// <typeparam name="TModel">The type of model.</typeparam>
        /// <param name="instance">The newly created instance of TModel.</param>
        static partial void AfterCreate(TModel instance);
        /// <summary>
        /// This method is a partial method that is called before the creation of an object.
        /// </summary>
        /// <param name="other">The object being created.</param>
        static partial void BeforeCreate(object other);
        /// <summary>
        /// Represents a callback method that is executed after an instance of the specified model type is created.
        /// </summary>
        /// <param name="instance">The instance of the specified model type that was created.</param>
        /// <param name="other">An object representing additional data related to the creation of the instance.</param>
        static partial void AfterCreate(TModel instance, object other);
        /// <summary>
        /// This method is called before the creation of a new TModel object.
        /// </summary>
        /// <param name="other">An object of type TModel that represents the new object being created.</param>
        /// <remarks>
        /// Use this method to perform any necessary operations or validations before the creation of a new TModel object.
        /// </remarks>
        static partial void BeforeCreate(TModel other);
        ///<summary>
        /// This method is called after an instance of the TModel class is created.
        /// It performs additional operations using the created instance and 'other' parameter.
        ///</summary>
        ///<param name="instance">The newly created instance of the TModel class.</param>
        ///<param name="other">Another instance of the TModel class.</param>
        static partial void AfterCreate(TModel instance, TModel other);
        /// <summary>
        /// This method is called before the creation of an entity to perform additional actions or validations.
        /// </summary>
        /// <param name="other">The entity object being created.</param>
        /// <remarks>
        /// This method is implemented as a partial method, allowing the user to provide custom logic to be executed before the creation of the entity.
        /// If this method is not implemented explicitly in a partial class, it becomes a no-op.
        /// </remarks>
        static partial void BeforeCreate(TEntity other);
        /// <summary>
        /// This method is called after creating a new instance of the TModel class.
        /// It provides an opportunity to perform additional operations or modifications
        /// on the created instance before it is saved to the database.
        /// </summary>
        /// <param name="instance">The newly created instance of the TModel class.</param>
        /// <param name="other">The TEntity object associated with the instance.</param>
        static partial void AfterCreate(TModel instance, TEntity other);
    }
}
#endif
//MdEnd

