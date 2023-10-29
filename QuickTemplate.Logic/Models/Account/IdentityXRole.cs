//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using TEntity = Entities.Account.IdentityXRole;
    using TModel = Models.Account.IdentityXRole;
    /// <summary>
    /// Represents an identity to role model.
    /// </summary>
    internal partial class IdentityXRole : VersionExtendedModel
    {
        /// <summary>
        /// Documentation for the static constructor of the IdentityXRole class.
        /// </summary>
        /// <remarks>
        /// This static constructor is executed when the class is first accessed or an instance of the class is created.
        /// </remarks>
        static IdentityXRole()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// A partial method called when the class is being constructed.
        /// </summary>
        /// <remarks>
        /// This method is intended to be implemented in a partial class. It is called automatically during class construction.
        /// This method can also be left unimplemented, and it will be removed during compilation if not used.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        /// <summary>
        /// Represents a new instance of the IdentityXRole class.
        /// </summary>
        /// <remarks>
        /// This constructor calls the Constructing and Constructed methods.
        /// </remarks>
        public IdentityXRole()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is constructed.
        /// </summary>
        partial void Constructed();
        /// <summary>
        /// Gets or sets the source entity.
        /// </summary>
        /// <value>
        /// The source entity.
        /// </value>
        new internal TEntity Source
        {
            get => (TEntity)(_source ??= new TEntity());
            set => _source = value;
        }
        /// <summary>
        /// Gets or sets the ID type of the identity.
        /// </summary>
        public IdType IdentityId
        {
            get => Source.IdentityId;
            set => Source.IdentityId = value;
        }
        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        public IdType RoleId
        {
            get => Source.RoleId;
            set => Source.RoleId = value;
        }
        /// <summary>
        /// Gets or sets the identity of the account.
        /// </summary>
        /// <value>
        /// The account identity.
        /// </value>
        public Models.Account.Identity? Identity
        {
            get => Source.Identity != null ? Models.Account.Identity.Create(Source.Identity) : null;
            set => Source.Identity = value?.Source;
        }
        internal void CopyProperties(TEntity other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Id = other.Id;
#if ROWVERSION_ON
                RowVersion = other.RowVersion;
#endif
                IdentityId= other.IdentityId;
                RoleId= other.RoleId;
                Identity = other.Identity != null ? Models.Account.Identity.Create((object)other.Identity) : null;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// This method is called before copying the properties of the entity.
        /// </summary>
        /// <param name="other">The entity from which the properties are being copied.</param>
        /// <param name="handled">A flag indicating whether the method has been handled or not.</param>
        /// <remarks>
        /// This method allows performing custom logic before the properties of the entity are copied.
        /// It is invoked when the CopyProperties method is called.
        /// Only the properties that are not explicitly handled in this method will be copied.
        /// By setting the 'handled' parameter to true, the method indicates that the property copy has been handled and
        /// the default copy operation should not be performed.
        /// </remarks>
        /// <seealso cref="CopyProperties(TEntity)"/>
        partial void BeforeCopyProperties(TEntity other, ref bool handled);
        /// <summary>
        /// Performs additional operations after copying the properties of another entity.
        /// </summary>
        /// <param name="other">The other entity from which the properties are copied.</param>
        /// <remarks>
        /// This method can be overridden in a partial class to perform specific actions after copying properties.
        /// It is executed immediately after the properties are copied from <paramref name="other"/>.
        /// </remarks>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        partial void AfterCopyProperties(TEntity other);
        
        internal void CopyProperties(TModel other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Id = other.Id;
#if ROWVERSION_ON
                RowVersion = other.RowVersion;
#endif
                IdentityId = other.IdentityId;
                RoleId = other.RoleId;
                Identity = other.Identity != null ? Models.Account.Identity.Create((object)other.Identity) : null;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// Performs additional actions before copying the properties from the specified model.
        /// </summary>
        /// <param name="other">The model from which the properties are being copied.</param>
        /// <param name="handled">A reference to a boolean indicating whether the before copy properties process has been handled.</param>
        /// <remarks>
        /// This method can be partially implemented in a class that is a partial type of another class. It allows performing additional actions specific to the class before copying the properties from another model.
        /// </remarks>
        partial void BeforeCopyProperties(TModel other, ref bool handled);
        /// <summary>
        /// Performs additional operations after copying properties from another TModel object.
        /// </summary>
        /// <param name="other">The TModel object from which to copy the properties.</param>
        /// <remarks>
        /// This method is called after the properties of another TModel object are copied to the current object.
        /// Implement this method to perform any additional operations or modifications based on the copied properties.
        /// </remarks>
        partial void AfterCopyProperties(TModel other);
        
        /// <summary>
        /// Determines whether the current instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// True if the current instance is equal to the specified object; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method compares the current instance with the specified object by checking if they are of the same type and share the same id.
        /// </remarks>
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
        /// Computes the hash code for the current instance.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        /// <remarks>
        /// The hash code is calculated by combining the hash codes of the <see cref="IdentityId"/> and <see cref="RoleId"/> properties.
        /// </remarks>
        public override int GetHashCode()
        {
            return HashCode.Combine(IdentityId, RoleId);
        }
        
        ///<summary>
        /// Creates a new instance of the specified model, and performs pre- and post-create operations.
        ///</summary>
        ///<typeparam name="TModel">The type of the model to create.</typeparam>
        ///<returns>A new instance of the specified model.</returns>
        public static TModel Create()
        {
            BeforeCreate();
            var result = new TModel();
            AfterCreate(result);
            return result;
        }
        /// <summary>
        /// Creates a new instance of the specified generic model type <typeparamref name="TModel"/>.
        /// </summary>
        /// <param name="other">The object from which the values will be copied to the new model instance.</param>
        /// <returns>A new instance of the specified generic model type, created by copying values from the provided <paramref name="other"/> object.</returns>
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
        /// Creates a new instance of the specified model and copies the properties from the provided model.
        /// </summary>
        /// <param name="other">The model to copy properties from.</param>
        /// <returns>A new instance of the specified model.</returns>
        public static TModel Create(TModel other)
        {
            BeforeCreate(other);
            var result = new TModel();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Creates an instance of the specified <typeparamref name="TModel"/> type by initializing it with the given <paramref name="other"/> object.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to be created.</typeparam>
        /// <typeparam name="TEntity">The type of the object to initialize the model with.</typeparam>
        /// <param name="other">The object used to initialize the model.</param>
        /// <returns>An instance of the specified <typeparamref name="TModel"/> type with its <c>Source</c> property set to the <paramref name="other"/> object.</returns>
        /// <remarks>
        /// This method triggers the <see cref="BeforeCreate"/> and <see cref="AfterCreate"/> methods allowing for additional pre- and post-processing logic respectively.
        /// </remarks>
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
        /// This method is called before the create operation is performed.
        /// </summary>
        /// <remarks>
        /// This method is intended to be implemented by partial classes in order to provide custom logic before a create operation is executed.
        /// </remarks>
        static partial void BeforeCreate();
        /// <summary>
        /// This method is called after an instance of the model is created.
        /// </summary>
        /// <param name="instance">The instance of the model that was created.</param>
        static partial void AfterCreate(TModel instance);
        /// <summary>
        /// Executes before the creation of an object.
        /// </summary>
        /// <param name="other">The other object.</param>
        /// <remarks>
        /// This method is called before an object is created. This allows any necessary
        /// preparations or validations to be performed before the creation occurs.
        /// </remarks>
        static partial void BeforeCreate(object other);
        /// <summary>
        /// This method is called after creating a new instance of a TModel object.
        /// </summary>
        /// <param name="instance">The newly created TModel instance.</param>
        /// <param name="other">Additional information or object passed. Can be null.</param>
        static partial void AfterCreate(TModel instance, object other);
        /// <summary>
        /// This method is a partial method that is called before creating an instance of the specified model.
        /// </summary>
        /// <typeparam name="TModel">The type of model.</typeparam>
        /// <param name="other">The other model to be created.</param>
        /// <remarks>
        /// Use this method to perform custom operations or validations before creating an instance of the specified model.
        /// This method will be executed only if there is an implementation in the partial class.
        /// </remarks>
        static partial void BeforeCreate(TModel other);
        /// <summary>
        /// Executed after creating a new instance of the TModel class.
        /// </summary>
        /// <typeparam name="TModel">The type of model.</typeparam>
        /// <param name="instance">The newly created instance of TModel.</param>
        /// <param name="other">Another instance of TModel.</param>
        static partial void AfterCreate(TModel instance, TModel other);
        /// <summary>
        /// This method is called before creating a new entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="other">The other entity of type <typeparamref name="TEntity"/> that is being created.</param>
        /// <remarks>
        /// Use this method to perform any necessary operations or validations before creating the entity.
        /// </remarks>
        static partial void BeforeCreate(TEntity other);
        /// <summary>
        /// Executes after the creation of an instance of the specified model <paramref name="instance"/>
        /// and an instance of the specified entity <paramref name="other"/>.
        /// </summary>
        /// <param name="instance">The instance of the specified model.</param>
        /// <param name="other">The instance of the specified entity.</param>
        /// <remarks>
        /// This method is intended to be overridden in derived classes.
        /// It is executed after the creation of an instance of the specified model <paramref name="instance"/>
        /// and an instance of the specified entity <paramref name="other"/>.
        /// </remarks>
        /// <seealso cref="TModel"/>
        /// <seealso cref="TEntity"/>
        static partial void AfterCreate(TModel instance, TEntity other);
    }
}
#endif
//MdEnd

