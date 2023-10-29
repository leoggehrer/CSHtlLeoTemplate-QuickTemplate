//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using TEntity = Entities.Account.User;
    using TModel = Models.Account.User;
    
    /// <summary>
    /// Represents an user model.
    /// </summary>
    public partial class User : VersionExtendedModel
    {
        /// <summary>
        /// Initializes the <see cref="User"/> class.
        /// </summary>
        static User()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called when the class is being constructed.
        /// </summary>
        /// <remarks>
        /// Implement this method in a partial class to perform any actions or initialization steps
        /// that should be executed when the class is being constructed.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        /// <remarks>
        /// The method is marked as "partial" which means it is part of a partial class definition.
        /// It is intended to be implemented in another part of the partial class file.
        /// </remarks>
        /// <seealso cref="PartialClass"/>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor calls the <see cref="Constructing"/> method and the <see cref="Constructed"/> method.
        /// </remarks>
        public User()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        /// <remarks>
        /// Use this method to perform additional initialization tasks when constructing the object.
        /// This method is specific to the partial class and can be found in a separate file.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// This method is called after the object is constructed.
        /// </summary>
        partial void Constructed();
        /// <summary>
        /// The source entity for the property.
        /// </summary>
        new internal TEntity Source
        {
            get => (TEntity)(_source ??= new TEntity());
            set => _source = value;
        }
        /// <summary>
        /// Gets or sets the identity identifier.
        /// </summary>
        public IdType IdentityId
        {
            get => Source.IdentityId;
            set => Source.IdentityId = value;
        }
        /// <summary>
        /// Gets or sets the first name of the source object.
        /// </summary>
        public String FirstName
        {
            get => Source.FirstName;
            set => Source.FirstName = value;
        }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public String LastName
        {
            get => Source.LastName;
            set => Source.LastName = value;
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
                IdentityId = other.IdentityId;
                FirstName = other.FirstName;
                LastName = other.LastName;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// Performs actions before copying properties from another entity to the current entity.
        /// </summary>
        /// <param name="other">The other entity from which properties are being copied.</param>
        /// <param name="handled">
        /// A reference to a boolean value indicating if the action has been handled.
        /// Set to <c>true</c> if the action has been handled; otherwise, <c>false</c>.
        /// </param>
        /// <remarks>
        /// This method is implemented as a partial method,
        /// allowing custom code to be added in separate files without modifying the main class.
        /// </remarks>
        partial void BeforeCopyProperties(TEntity other, ref bool handled);
        /// <summary>
        /// Performs additional actions after copying properties from another entity.
        /// </summary>
        /// <param name="other">The other entity whose properties were copied.</param>
        partial void AfterCopyProperties(TEntity other);
        
        public void CopyProperties(TModel other)
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
                FirstName = other.FirstName;
                LastName = other.LastName;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// Performs any necessary operations before copying the properties from another model to the current model.
        /// </summary>
        /// <param name="other">The other model from which the properties will be copied.</param>
        /// <param name="handled">A value indicating whether the method has been handled. This parameter should be updated accordingly within the method implementation.</param>
        /// <remarks>
        /// This method is called before the properties are copied from one model to another. It allows performing any additional operations or validations before the copying takes place.
        /// </remarks>
        partial void BeforeCopyProperties(TModel other, ref bool handled);
        /// <summary>
        /// Performs actions after copying properties from another <typeparamref name="TModel"/> instance.
        /// </summary>
        /// <param name="other">The <typeparamref name="TModel"/> instance from which properties are copied.</param>
        /// <remarks>
        /// This method is partially implemented by derived classes to provide custom logic after copying properties.
        /// </remarks>
        /// <typeparam name="TModel">The type of the model that the properties are being copied to.</typeparam>
        /// <seealso cref="CopyPropertiesFrom(TModel)"/>
        /// <seealso cref="BeforeCopyProperties(TModel)"/>
        /// <seealso cref="AfterGetProperties"/>
        /// <seealso cref="BeforeSetProperties()"/>
        partial void AfterCopyProperties(TModel other);
        
        /// <summary>
        /// Determines whether the current User object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with the current User object.</param>
        /// <returns>true if the specified object is equal to the current User object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is Models.Account.User other)
            {
                result = IsEqualsWith(IdentityId, other.IdentityId)
                && Id == other.Id;
            }
            return result;
        }
        /// <summary>
        /// Computes a hash code by combining the hash codes of the properties of the object.
        /// </summary>
        /// <returns>A hash code for the object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(IdentityId, FirstName, LastName, Id);
        }
        
        /// <summary>
        /// Creates a new instance of a model.
        /// </summary>
        /// <typeparam name="TModel">The type of model to create.</typeparam>
        /// <returns>The created model instance.</returns>
        public static TModel Create()
        {
            BeforeCreate();
            var result = new TModel();
            AfterCreate(result);
            return result;
        }
        /// <summary>
        /// Creates an instance of TModel by copying the properties of the provided object.
        /// </summary>
        /// <typeparam name="TModel">The type of model to create.</typeparam>
        /// <param name="other">The object from which to copy the properties.</param>
        /// <returns>The created instance of TModel.</returns>
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
        /// Creates a new instance of the specified type using the properties of the provided model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="other">The model to be used for creating the new instance.</param>
        /// <returns>A new instance of the specified model type.</returns>
        public static TModel Create(TModel other)
        {
            BeforeCreate(other);
            var result = new TModel();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Creates a new instance of <typeparamref name="TModel"/> based on the given <paramref name="other"/> entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity being used.</typeparam>
        /// <typeparam name="TModel">The type of the model being created.</typeparam>
        /// <param name="other">The entity used as the basis for creating the model.</param>
        /// <returns>A new instance of <typeparamref name="TModel"/> created based on the <paramref name="other"/> entity.</returns>
        /// <remarks>
        /// <para>The <see cref="BeforeCreate(TEntity)"/> method is called before the creation of the model.</para>
        /// <para>The <paramref name="other"/> entity's properties are copied to the new instance of <typeparamref name="TModel"/>.</para>
        /// <para>The <see cref="AfterCreate(TModel, TEntity)"/> method is called after the creation of the model.</para>
        /// </remarks>
        internal static TModel Create(TEntity other)
        {
            BeforeCreate(other);
            var result = new TModel();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Executes before the creation of the object.
        /// </summary>
        static partial void BeforeCreate();
        /// <summary>
        /// Represents the method that is called after a new instance of a model is created.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="instance">The newly created instance of the model.</param>
        static partial void AfterCreate(TModel instance);
        /// <summary>
        /// This method is a partial method that is called before the execution of the Create method.
        /// It allows custom code to be executed before the object is created.
        /// </summary>
        /// <param name="other">The object that is being created.</param>
        /// <remarks>
        /// This method can be implemented by the user in a separate partial class to add custom logic.
        /// The implementation of this method is optional and not necessary for the normal functioning of the Create method.
        /// </remarks>
        static partial void BeforeCreate(object other);
        /// <summary>
        /// This method is called after creating a new object of type TModel.
        /// </summary>
        /// <param name="instance">The newly created object.</param>
        /// <param name="other">An additional parameter for specific operations.</param>
        static partial void AfterCreate(TModel instance, object other);
        /// <summary>
        /// This method is called before creating a new instance of the TModel class.
        /// </summary>
        /// <param name="other">The TModel instance that is being created</param>
        static partial void BeforeCreate(TModel other);
        /// <summary>
        /// This method is called after creating a new instance of <typeparamref name="TModel"/>.
        /// </summary>
        /// <param name="instance">The newly created instance of <typeparamref name="TModel"/>.</param>
        /// <param name="other">Another instance of <typeparamref name="TModel"/>.</param>
        /// <remarks>
        /// This method can be used to perform additional operations or customization after creating a new instance of <typeparamref name="TModel"/>.
        /// It receives the newly created instance as well as another existing instance of <typeparamref name="TModel"/> as parameters.
        /// </remarks>
        static partial void AfterCreate(TModel instance, TModel other);
        /// <summary>
        /// This method is invoked before the creation of a TEntity object.
        /// </summary>
        /// <param name="other">The TEntity object being created.</param>
        static partial void BeforeCreate(TEntity other);
        /// <summary>
        /// This method is called after creating a new instance of the <typeparamref name="TModel"/> class.
        /// </summary>
        /// <param name="instance">The newly created instance of the <typeparamref name="TModel"/> class.</param>
        /// <param name="other">An entity object that is related to the newly created instance.</param>
        static partial void AfterCreate(TModel instance, TEntity other);
    }
}
#endif
//MdEnd

