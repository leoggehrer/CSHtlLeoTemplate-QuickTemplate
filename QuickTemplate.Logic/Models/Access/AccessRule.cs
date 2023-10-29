//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Models.Access
{
    using TEntity = Entities.Access.AccessRule;
    using TModel = Models.Access.AccessRule;

    /// <summary>
    /// Represents an access rule model.
    /// </summary>
    public partial class AccessRule : VersionExtendedModel
    {
        /// <summary>
        /// Initializes the static class before the class constructor executes.
        /// </summary>
        static AccessRule()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called during the construction of the class.
        /// </summary>
        /// <remarks>
        /// This method is intended to be implemented in a partial class
        /// and should contain any code specific to the class's construction process.
        /// It is called before the class's constructor is executed.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the AccessRule class.
        /// </summary>
        /// <remarks>
        /// This constructor calls the <see cref="Constructing"/> method before initializing the object
        /// and then calls the <see cref="Constructed"/> method.
        /// </remarks>
        public AccessRule()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        /// <remarks>
        /// This method can be overridden in a partial class to provide additional initialization logic.
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is constructed.
        /// </summary>
        /// <remarks>
        /// This method is meant to contain any initialization logic that needs to be executed during the construction of the object.
        /// </remarks>
        partial void Constructed();
        /// <summary>
        /// Gets or sets the source of the property.
        /// </summary>
        new internal TEntity Source
        {
            get => (TEntity)(_source ??= new TEntity());
            set => _source = value;
        }
        /// <summary>
        /// Gets or sets the rule type of the property.
        /// </summary>
        public QuickTemplate.Logic.Modules.Access.RuleType Type
        {
            get => Source.Type;
            set => Source.Type = value;
        }
        /// <summary>
        /// Gets or sets the entity type.
        /// </summary>
        public System.String EntityType
        {
            get => Source.EntityType;
            set => Source.EntityType = value;
        }
        /// <summary>
        /// Gets or sets the relationship entity type.
        /// </summary>
        public System.String? RelationshipEntityType
        {
            get => Source.RelationshipEntityType;
            set => Source.RelationshipEntityType = value;
        }
        /// <summary>
        /// Gets or sets the PropertyName value.
        /// </summary>
        public System.String? PropertyName
        {
            get => Source.PropertyName;
            set => Source.PropertyName = value;
        }
        /// <summary>
        /// Gets or sets the value of the entity.
        /// </summary>
        public System.String? EntityValue
        {
            get => Source.EntityValue;
            set => Source.EntityValue = value;
        }
        /// <summary>
        /// Gets or sets the access type for the QuickTemplate module.
        /// </summary>
        public QuickTemplate.Logic.Modules.Access.AccessType AccessType
        {
            get => Source.AccessType;
            set => Source.AccessType = value;
        }
        /// <summary>
        /// Gets or sets the access value.
        /// </summary>
        public System.String? AccessValue
        {
            get => Source.AccessValue;
            set => Source.AccessValue = value;
        }
        /// <summary>
        /// Gets or sets a value indicating whether the object is creatable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the object is creatable; otherwise, <c>false</c>.
        /// </value>
        public System.Boolean Creatable
        {
            get => Source.Creatable;
            set => Source.Creatable = value;
        }
        /// <summary>
        /// Gets or sets a value indicating whether the source is readable.
        /// </summary>
        /// <value><c>true</c> if the source is readable; otherwise, <c>false</c>.</value>
        public System.Boolean Readable
        {
            get => Source.Readable;
            set => Source.Readable = value;
        }
        /// <summary>
        /// Gets or sets a value indicating whether the object is updatable.
        /// </summary>
        /// <value>true if the object is updatable; otherwise, false.</value>
        public System.Boolean Updatable
        {
            get => Source.Updatable;
            set => Source.Updatable = value;
        }
        /// <summary>
        /// Gets or sets a value indicating whether the object can be deleted.
        /// </summary>
        /// <value><c>true</c> if the object is deletable; otherwise, <c>false</c>.</value>
        public System.Boolean Deletable
        {
            get => Source.Deletable;
            set => Source.Deletable = value;
        }
        /// <summary>
        /// Gets or sets a value indicating whether the source is viewable.
        /// </summary>
        /// <value><c>true</c> if the source is viewable; otherwise, <c>false</c>.</value>
        public System.Boolean Viewable
        {
            get => Source.Viewable;
            set => Source.Viewable = value;
        }
        /// <summary>
        /// Copies the properties from the specified <typeparamref name="TEntity"/> object to the current instance.
        /// </summary>
        /// <param name="other">The <typeparamref name="TEntity"/> object from which to copy the properties.</param>
        internal void CopyProperties(TEntity other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Type = other.Type;
                EntityType = other.EntityType;
                RelationshipEntityType = other.RelationshipEntityType;
                PropertyName = other.PropertyName;
                EntityValue = other.EntityValue;
                AccessType = other.AccessType;
                AccessValue = other.AccessValue;
                Creatable = other.Creatable;
                Readable = other.Readable;
                Updatable = other.Updatable;
                Deletable = other.Deletable;
                Viewable = other.Viewable;
                Id = other.Id;
                CopyExendedProperties(other);
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// This method is called before copying the properties of the given entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="other">The entity whose properties are being copied.</param>
        /// <param name="handled">A flag indicating if the event is handled.</param>
        partial void BeforeCopyProperties(TEntity other, ref bool handled);
        /// <summary>
        /// This method is called after the properties of the current entity
        /// have been copied from another entity.
        /// </summary>
        /// <param name="other">The entity from which the properties have been copied.</param>
        /// <remarks>
        /// This method can be used to perform any post-processing tasks
        /// or validations after the properties have been copied.
        /// It provides a hook for additional logic that needs to be executed
        /// after copying properties.
        /// </remarks>
        partial void AfterCopyProperties(TEntity other);
        /// <summary>
        /// Copies the properties from another instance of the specified model.
        /// </summary>
        /// <param name="other">The instance of the model to copy properties from.</param>
        internal void CopyProperties(TModel other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Type = other.Type;
                EntityType = other.EntityType;
                RelationshipEntityType = other.RelationshipEntityType;
                PropertyName = other.PropertyName;
                EntityValue = other.EntityValue;
                AccessType = other.AccessType;
                AccessValue = other.AccessValue;
                Creatable = other.Creatable;
                Readable = other.Readable;
                Updatable = other.Updatable;
                Deletable = other.Deletable;
                Viewable = other.Viewable;
                Id = other.Id;
                CopyExendedProperties(other);
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// Performs actions before copying properties from the specified <paramref name="other"/> object to the current object.
        /// </summary>
        /// <typeparam name="TModel">The type of the object being copied.</typeparam>
        /// <param name="other">The other object from which properties will be copied.</param>
        /// <param name="handled">A reference to a boolean value indicating whether the action has been handled.</param>
        /// <remarks>
        /// This method is intended to allow customization of the copy properties behavior. It is executed before the properties
        /// are copied and provides an opportunity to perform any necessary setup or validation.
        /// </remarks>
        partial void BeforeCopyProperties(TModel other, ref bool handled);
        /// <summary>
        /// Runs after copying properties from another model of the same type.
        /// </summary>
        /// <param name="other">The other model from which properties are copied.</param>
        /// <remarks>
        /// This method is called after the properties of the current model are copied
        /// from another model of the same type. Implement this method to perform any
        /// additional tasks or logic after the properties have been copied.
        /// </remarks>
        partial void AfterCopyProperties(TModel other);
        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is Models.Access.AccessRule other)
            {
#if ROWVERSION_ON
                result = IsEqualsWith(RowVersion, other.RowVersion) && Id == other.Id;
#else
                result = Id == other.Id;
#endif
            }
            return result;
        }
        /// <summary>
        /// Overrides the GetHashCode method to return a unique hash code for the object.
        /// </summary>
        /// <returns>A 32-bit signed integer representing the hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode(Type, EntityType, RelationshipEntityType, PropertyName, EntityValue, AccessType, AccessValue, Creatable, Readable, Updatable, Deletable, Viewable, Id);
        }
        /// <summary>
        /// Creates a new instance of the specified type <typeparamref name="TModel"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of model to be created.</typeparam>
        /// <returns>A new instance of the specified type <typeparamref name="TModel"/>.</returns>
        /// <remarks>
        /// This method invokes the BeforeCreate() method before creating the new instance,
        /// then creates a new instance of the specified type <typeparamref name="TModel"/>,
        /// and finally calls the AfterCreate(result) method after the creation.
        /// </remarks>
        public static TModel Create()
        {
            BeforeCreate();
            var result = new TModel();
            AfterCreate(result);
            return result;
        }
        /// <summary>
        /// Creates a new instance of <typeparamref name="TModel"/> by copying the properties from the specified <paramref name="other"/> object.
        /// </summary>
        /// <param name="other">The object whose properties to copy.</param>
        /// <returns>A new instance of <typeparamref name="TModel"/> with properties copied from <paramref name="other"/>.</returns>
        public static TModel Create(object other)
        {
            BeforeCreate(other);
            var result = new TModel();
            ObjectExtensions.CopyFrom(result, other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Create a new instance of <typeparamref name="TModel"/> by copying properties from the specified instance of <typeparamref name="TModel"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="other">The instance of <typeparamref name="TModel"/> from which properties will be copied.</param>
        /// <returns>A new instance of <typeparamref name="TModel"/> with copied properties.</returns>
        public static TModel Create(TModel other)
        {
            BeforeCreate(other);
            var result = new TModel();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Creates a new instance of <typeparamref name="TModel"/> using the provided <paramref name="other"/> entity.
        /// </summary>
        /// <param name="other">The entity object to be used as a source for creating the new instance.</param>
        /// <returns>A new instance of <typeparamref name="TModel"/>.</returns>
        internal static TModel Create(TEntity other)
        {
            BeforeCreate(other);
            var result = new TModel();
            result.Source = other;
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// This method is called before the execution of the "Create" method.
        /// </summary>
        static partial void BeforeCreate();
        /// <summary>
        /// This method is called after creating a new <typeparamref name="TModel"/> instance.
        /// </summary>
        /// <param name="instance">The newly created <typeparamref name="TModel"/> instance.</param>
        /// <remarks>
        /// This method can be used to perform any additional actions or tasks after creating
        /// a new <typeparamref name="TModel"/> instance. It is called immediately after
        /// the creation process.
        /// </remarks>
        /// <typeparam name="TModel">The type of the model instance.</typeparam>
        /// <returns>None</returns>
        static partial void AfterCreate(TModel instance);
        /// <summary>
        /// Executed before the "Create" operation is performed.
        /// </summary>
        /// <param name="other">The object to be created.</param>
        static partial void BeforeCreate(object other);
        /// <summary>
        /// Method called after the creation of a TModel instance.
        /// </summary>
        /// <param name="instance">The TModel instance that was created.</param>
        /// <param name="other">An object that may be used as additional information.</param>
        /// <remarks>
        /// This method is a partial method, meaning it can be implemented in another partial class or file.
        /// </remarks>
        static partial void AfterCreate(TModel instance, object other);
        /// <summary>
        /// This method is called before creating a new instance of the <typeparamref name="TModel"/> class.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="other">The instance of the <typeparamref name="TModel"/> class to be created.</param>
        /// <remarks>
        /// Use this method to perform any necessary operations or validations before creating a new instance of the <typeparamref name="TModel"/> class.
        /// This method will be called just before the actual creation of the class instance.
        /// </remarks>
        static partial void BeforeCreate(TModel other);
        /// <summary>
        /// This method is called after an instance of TModel is created.
        /// </summary>
        /// <typeparam name="TModel">The type of the model instance.</typeparam>
        /// <param name="instance">The created instance of TModel.</param>
        /// <param name="other">Another instance of TModel.</param>
        /// <remarks>
        /// This method is a hook for performing additional logic after the creation
        /// of a TModel instance. It receives the created instance as well as an
        /// optional other instance of TModel for further processing.
        /// </remarks>
        static partial void AfterCreate(TModel instance, TModel other);
        /// <summary>
        /// This method is called before the creation of an entity of type TEntity.
        /// </summary>
        /// <param name="other">The other entity associated with the creation.</param>
        static partial void BeforeCreate(TEntity other);
        /// <summary>
        /// Executes after creating an instance of the TModel class.
        /// </summary>
        /// <param name="instance">The created instance of TModel.</param>
        /// <param name="other">The TEntity object related to the created instance.</param>
        static partial void AfterCreate(TModel instance, TEntity other);
    }
}
#endif
//MdEnd
