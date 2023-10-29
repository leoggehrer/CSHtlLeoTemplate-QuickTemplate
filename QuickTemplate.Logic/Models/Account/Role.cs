//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using QuickTemplate.Logic.Entities;
    using TEntity = Entities.Account.Role;
    using TModel = Models.Account.Role;

    /// <summary>
    /// Represents a role model.
    /// </summary>
    public partial class Role : VersionExtendedModel, Contracts.Account.IRole
    {
        /// <summary>
        /// Initializes the static members of the <see cref="Role"/> class.
        /// </summary>
        static Role()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// Called when the class is being constructed.
        /// </summary>
        /// <remarks>
        /// This method is responsible for any initialization tasks that need to be performed before the class can be used.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the Role class.
        /// </summary>
        /// <remarks>
        /// This constructor calls the Constructing and Constructed methods.
        /// </remarks>
        public Role()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// It can be implemented by subclasses to initialize any required data.
        /// </summary>
        /// <remarks>
        /// This method is empty by default and can be overridden by subclasses.
        /// </remarks>
        /// <seealso cref="ConstructingClass"/>
        /// <seealso cref="OtherConstructingMethod"/>
        /// <returns>
        /// This method does not return a value.
        /// </returns>
        partial void Constructing();
        ///<summary>
        /// This method is called when the object is constructed.
        /// It can be overridden in a partial class.
        ///</summary>
        ///<remarks>
        /// This method is a placeholder that can be implemented in a partial class to perform additional initialization tasks when the object is constructed.
        ///</remarks>
        partial void Constructed();
        /// <summary>
        /// Gets or sets the source TEntity object.
        /// </summary>
        new internal TEntity Source
        {
            get => (TEntity)(_source ??= new TEntity());
            set => _source = value;
        }
#if GUID_OFF
        /// <summary>
        /// Gets or sets the GUID of the source.
        /// </summary>
        /// <value>The GUID of the source.</value>
        public Guid Guid
        {
            get => Source.Guid;
            set => Source.Guid = value;
        }
#endif
        /// <summary>
        /// Gets or sets the Designation property.
        /// </summary>
        /// <remarks>
        /// This property represents the designation of an object.
        /// </remarks>
        public String Designation
        {
            get => Source.Designation;
            set => Source.Designation = value;
        }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public String? Description
        {
            get => Source.Description;
            set => Source.Description = value;
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
                Guid = other.Guid;
                Designation = other.Designation;
                Description = other.Description;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// Allows custom logic to be executed before the properties of the current entity
        /// are copied from another entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="other">The other entity from which the properties are being copied.</param>
        /// <param name="handled">A boolean value indicating whether the operation has been handled.</param>
        partial void BeforeCopyProperties(TEntity other, ref bool handled);
        /// <summary>
        /// This method is called after copying the properties of the specified entity to the current entity.
        /// </summary>
        /// <param name="other">The entity whose properties were copied.</param>
        /// <remarks>
        /// This is a partial method, which means it can be implemented by the developer in partial classes to perform additional logic after copying properties.
        /// </remarks>
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
                Guid = other.Guid;
                Designation = other.Designation;
                Description = other.Description;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// Executed before copying the properties of <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="other">The other model object from which properties are to be copied.</param>
        /// <param name="handled">A reference to a boolean value indicating whether the execution is already handled.</param>
        /// <remarks>
        /// This method is called just before copying the properties of <paramref name="other"/>.
        /// It provides a chance to handle the execution before the copy process begins.
        /// </remarks>
        partial void BeforeCopyProperties(TModel other, ref bool handled);
        /// <summary>
        /// Performs additional actions after the properties of the current object have been copied from the specified object of type TModel.
        /// </summary>
        /// <param name="other">The object from which the properties are copied.</param>
        /// <remarks>
        /// This method is intended to be overridden in partial classes that require additional logic after copying properties.
        /// </remarks>
        partial void AfterCopyProperties(TModel other);
        
        /// <summary>
        /// Determines whether the current Role object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        /// <remarks>
        /// This method overrides the Equals method of the Object class.
        /// Two Role objects are considered equal if they have the same type and the same Id.
        /// </remarks>
        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is Models.Account.Role other)
            {
                result = IsEqualsWith(GetType().Name, other.GetType().Name) && Id == other.Id;
            }
            return result;
        }
        public override int GetHashCode()
        {
#if ROWVERSION_ON
            return HashCode.Combine(Guid, Designation, Description, RowVersion, Id);
#else
            return HashCode.Combine(Guid, Designation, Description, Id);
#endif
        }
        
        /// <summary>
        /// Creates a new instance of type TModel.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to create.</typeparam>
        /// <returns>A new instance of type TModel.</returns>
        /// <remarks>This method performs the necessary operations before and after creating the model.</remarks>
        public static TModel Create()
        {
            BeforeCreate();
            var result = new TModel();
            AfterCreate(result);
            return result;
        }
        /// <summary>
        /// Creates a new instance of the specified model <typeparamref name="TModel"/> using the properties of the provided object <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to create.</typeparam>
        /// <param name="other">The object from which the properties will be copied to the model.</param>
        /// <returns>A new instance of the model <typeparamref name="TModel"/> with properties copied from the provided object.</returns>
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
        /// Creates a new instance of the <typeparamref name="TModel"/> class by copying the properties from another instance of the <typeparamref name="TModel"/> class.
        /// </summary>
        /// <param name="other">The instance of the <typeparamref name="TModel"/> class to copy the properties from.</param>
        /// <returns>A new instance of the <typeparamref name="TModel"/> class with the copied properties.</returns>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        public static TModel Create(TModel other)
        {
            BeforeCreate(other);
            var result = new TModel();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Creates an instance of the specified model type based on the provided entity.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to create.</typeparam>
        /// <typeparam name="TEntity">The type of the entity to use as the source.</typeparam>
        /// <param name="other">The entity object to use as the source.</param>
        /// <returns>An instance of the specified model type.</returns>
        /// <remarks>
        /// This method invokes the <see cref="BeforeCreate"/> method passing the provided entity object as an argument.
        /// Then, it creates an instance of the specified model type and sets its Source property with the provided entity.
        /// After that, it invokes the <see cref="AfterCreate"/> method passing the created model object as the first argument
        /// and the provided entity object as the second argument.
        /// Finally, it returns the created model object.
        /// </remarks>
        internal static TModel Create(TEntity other)
        {
            BeforeCreate(other);
            var result = new TModel();
            result.Source = other;
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Invoked before the creation of the object.
        /// This method allows for custom logic to be executed before creating the object.
        /// </summary>
        static partial void BeforeCreate();
        /// <summary>
        /// This method is called after creating a new instance of the provided model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="instance">The newly created instance of the model.</param>
        /// <remarks>
        /// Implement this method to perform any necessary logic or operations right after the creation of a new instance.
        /// </remarks>
        static partial void AfterCreate(TModel instance);
        /// <summary>
        /// Executes the code before the "Create" method is called.
        /// </summary>
        /// <param name="other">An object that can be used as a parameter for the code execution.</param>
        /// <remarks>
        /// This method is a partial method, meaning its implementation can be provided by the consuming module. If no implementation is provided, this method will have no effect.
        /// </remarks>
        static partial void BeforeCreate(object other);
        /// <summary>
        /// This method is called after creating an instance of the specified model.
        /// </summary>
        /// <param name="instance">The newly created instance of the model.</param>
        /// <param name="other">An optional object parameter.</param>
        static partial void AfterCreate(TModel instance, object other);
        /// <summary>
        /// This method is a partial implementation that is called before the creation of a new instance of a certain model.
        /// </summary>
        /// <param name="other">The other instance of the model that will be used before creation.</param>
        /// <remarks>
        /// This method is typically used as a hook to apply additional logic or modifications to the model instance before it is created.
        /// It takes in an existing instance of the model that can be used to perform certain operations or checks, if necessary,
        /// before the actual creation of the new instance.
        /// </remarks>
        static partial void BeforeCreate(TModel other);
        /// <summary>
        /// This method is called after creating an instance of <typeparamref name="TModel"/> and another instance of <typeparamref name="TModel"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="instance">The instance of <typeparamref name="TModel"/> that was created.</param>
        /// <param name="other">Another instance of <typeparamref name="TModel"/>.</param>
        /// <remarks>
        /// This method can be used to perform additional actions or modifications on the newly created instance and the other instance of <typeparamref name="TModel"/>.
        /// </remarks>
        static partial void AfterCreate(TModel instance, TModel other);
        /// <summary>
        /// Executed before creating the TEntity object.
        /// </summary>
        /// <param name="other">The TEntity object being created</param>
        // Implementation details...
        static partial void BeforeCreate(TEntity other);
        /// <summary>
        /// Method called after creating a new instance of a generic type TModel.
        /// </summary>
        /// <param name="instance">The new instance of type TModel.</param>
        /// <param name="other">An entity of type TEntity.</param>
        /// <remarks>
        /// This method is executed after a new instance of TModel is created.
        /// It can be used to perform additional operations or update related entities.
        /// </remarks>
        static partial void AfterCreate(TModel instance, TEntity other);
    }
}
#endif
//MdEnd
