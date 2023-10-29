//@BaseCode
//MdStart
#if ACCOUNT_ON && REVISION_ON
namespace QuickTemplate.Logic.Models.Revision
{
    using System;
    /// <summary>
    /// Represents a history model.
    /// </summary>
    public partial class History : ModelObject
    {
        /// <summary>
        /// Represents the static constructor for the <see cref="History"/> class.
        /// </summary>
        static History()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called during the construction of the class.
        /// </summary>
        /// <remarks>
        /// This method can be implemented in separate partial class file to execute specific
        /// logic before the class is completely constructed.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor calls the <see cref="Constructing"/> and <see cref="Constructed"/> methods.
        /// </remarks>
        public History()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the constructing phase.
        /// </summary>
        /// <remarks>
        /// Use this method to perform any initialization logic before the object is fully constructed.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// This method is called after the object has been constructed.
        /// </summary>
        /// <remarks>
        /// This method is typically used for additional initialization tasks that need to be performed
        /// after the object has been constructed.
        /// </remarks>
        partial void Constructed();
        /// <summary>
        /// Gets or sets the source history for the Revision.
        /// </summary>
        new internal QuickTemplate.Logic.Entities.Revision.History Source
        {
            get => (QuickTemplate.Logic.Entities.Revision.History)(_source ??= new QuickTemplate.Logic.Entities.Revision.History());
            set => _source = value;
        }
        /// <summary>
        /// Gets or sets the identity ID.
        /// </summary>
        /// <value>
        /// The identity ID.
        /// </value>
        public IdType IdentityId
        {
            get => Source.IdentityId;
            set => Source.IdentityId = value;
        }
        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        /// <value>
        /// The action type.
        /// </value>
        public System.String ActionType
        {
            get => Source.ActionType;
            set => Source.ActionType = value;
        }
        /// <summary>
        /// Gets or sets the action time.
        /// </summary>
        public System.DateTime ActionTime
        {
            get => Source.ActionTime;
            set => Source.ActionTime = value;
        }
        /// <summary>
        /// Gets or sets the name of the subject.
        /// </summary>
        /// <value>
        /// The name of the subject.
        /// </value>
        public System.String SubjectName
        {
            get => Source.SubjectName;
            set => Source.SubjectName = value;
        }
        /// <summary>
        /// Gets or sets the subject ID.
        /// </summary>
        /// <remarks>
        /// The subject ID represents the unique identifier of the subject.
        /// </remarks>
        public IdType SubjectId
        {
            get => Source.SubjectId;
            set => Source.SubjectId = value;
        }
        /// <summary>
        /// Gets or sets the JSON data stored in the Source object.
        /// </summary>
        /// <value>
        /// A string representing the JSON data.
        /// </value>
        public System.String JsonData
        {
            get => Source.JsonData;
            set => Source.JsonData = value;
        }
        /// <summary>
        /// Copies the properties from another <see cref="QuickTemplate.Logic.Entities.Revision.History"/> object.
        /// </summary>
        /// <param name="other">The other <see cref="QuickTemplate.Logic.Entities.Revision.History"/> object to copy properties from.</param>
        /// <remarks>
        /// This method will invoke the <see cref="BeforeCopyProperties"/> event handler before copying the properties.
        /// If the event is not handled, the properties will be copied from the <paramref name="other"/> object
        /// to the current object's properties including:
        /// - IdentityId
        /// - ActionType
        /// - ActionTime
        /// - SubjectName
        /// - SubjectId
        /// - JsonData
        /// - Id
        /// </remarks>
        internal void CopyProperties(QuickTemplate.Logic.Entities.Revision.History other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                IdentityId = other.IdentityId;
                ActionType = other.ActionType;
                ActionTime = other.ActionTime;
                SubjectName = other.SubjectName;
                SubjectId = other.SubjectId;
                JsonData = other.JsonData;
                Id = other.Id;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// This method is called before copying the properties from another History object.
        /// </summary>
        /// <param name="other">The History object to copy properties from.</param>
        /// <param name="handled">A reference to a boolean variable indicating if the event has been handled.</param>
        partial void BeforeCopyProperties(QuickTemplate.Logic.Entities.Revision.History other, ref bool handled);
        /// <summary>
        /// This method is called after properties are copied from the given <paramref name="other"/> object.
        /// </summary>
        /// <param name="other">The <see cref="QuickTemplate.Logic.Entities.Revision.History"/> object from which properties are copied.</param>
        /// <remarks>
        /// This method can be overriden in derived classes to perform additional operations after properties are copied.
        /// </remarks>
        /// <returns>Void</returns>
        partial void AfterCopyProperties(QuickTemplate.Logic.Entities.Revision.History other);
        /// <summary>
        /// Copies the properties from another History object to the current object.
        /// </summary>
        /// <param name="other">The History object from which to copy the properties.</param>
        internal void CopyProperties(QuickTemplate.Logic.Models.Revision.History other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                IdentityId = other.IdentityId;
                ActionType = other.ActionType;
                ActionTime = other.ActionTime;
                SubjectName = other.SubjectName;
                SubjectId = other.SubjectId;
                JsonData = other.JsonData;
                Id = other.Id;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// This method is called before copying the properties from one History object to another.
        /// </summary>
        /// <param name="other">The History object to copy properties from.</param>
        /// <param name="handled">A reference to a boolean variable indicating whether the process is handled or not.</param>
        partial void BeforeCopyProperties(QuickTemplate.Logic.Models.Revision.History other, ref bool handled);
        /// <summary>
        /// Performs additional actions after the properties of the current instance have been copied from another instance.
        /// </summary>
        /// <param name="other">The other instance from which the properties have been copied.</param>
        partial void AfterCopyProperties(QuickTemplate.Logic.Models.Revision.History other);
        /// <summary>
        /// Determines whether the current instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if the specified object is equal to the current instance; otherwise, false.</returns>
        /// <remarks>
        /// This method compares the <see cref="Id"/> property of the current instance with the
        /// <see cref="Id"/> property of the specified <paramref name="obj"/> object.
        /// Returns true if the <see cref="Id"/> properties are equal; otherwise, returns false.
        /// </remarks>
        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is Models.Revision.History other)
            {
                result = Id == other.Id;
            }
            return result;
        }
        /// <summary>
        /// Computes a hash code based on the values of the specified properties.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(IdentityId, ActionType, ActionTime, SubjectName, SubjectId, JsonData, HashCode.Combine(Id));
        }
        /// <summary>
        /// Creates a new instance of QuickTemplate.Logic.Models.Revision.History.
        /// </summary>
        /// <returns>A new instance of QuickTemplate.Logic.Models.Revision.History.</returns>
        public static QuickTemplate.Logic.Models.Revision.History Create()
        {
            BeforeCreate();
            var result = new QuickTemplate.Logic.Models.Revision.History();
            AfterCreate(result);
            return result;
        }
        /// <summary>
        /// Creates a new instance of the QuickTemplate.Logic.Models.Revision.History class and copies properties from the specified object.
        /// </summary>
        /// <param name="other">The object whose properties will be copied.</param>
        /// <returns>A new instance of the QuickTemplate.Logic.Models.Revision.History class.</returns>
        /// <remarks>
        /// This method also triggers the BeforeCreate and AfterCreate methods, which can be overridden in derived classes.
        /// </remarks>
        public static QuickTemplate.Logic.Models.Revision.History Create(object other)
        {
            BeforeCreate(other);
            CommonBase.Extensions.ObjectExtensions.CheckArgument(other, nameof(other));
            var result = new QuickTemplate.Logic.Models.Revision.History();
            CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Creates a new instance of the QuickTemplate.Logic.Models.Revision.History class by copying the properties from an existing History object.
        /// </summary>
        /// <param name="other">The History object to copy the properties from.</param>
        /// <returns>A new instance of the QuickTemplate.Logic.Models.Revision.History class with copied properties.</returns>
        public static QuickTemplate.Logic.Models.Revision.History Create(QuickTemplate.Logic.Models.Revision.History other)
        {
            BeforeCreate(other);
            var result = new QuickTemplate.Logic.Models.Revision.History();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Creates a new instance of the History class based on the provided History object.
        /// </summary>
        /// <param name="other">The History object to be used as the source for creating the new instance.</param>
        /// <returns>A new instance of the History class.</returns>
        /// <remarks>
        /// This method invokes two other methods, BeforeCreate and AfterCreate, before returning the created instance.
        /// </remarks>
        internal static QuickTemplate.Logic.Models.Revision.History Create(QuickTemplate.Logic.Entities.Revision.History other)
        {
            BeforeCreate(other);
            var result = new QuickTemplate.Logic.Models.Revision.History();
            result.Source = other;
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// This method is a hook that gets called before the "Create" operation.
        /// </summary>
        static partial void BeforeCreate();
        /// <summary>
        /// This method is called after a new instance of History is created.
        /// </summary>
        /// <param name="instance">The instance of History that was created.</param>
        static partial void AfterCreate(QuickTemplate.Logic.Models.Revision.History instance);
        /// <summary>
        /// This method is called before the creation of an object using the provided parameter.
        /// </summary>
        /// <param name="other">The object being created.</param>
        static partial void BeforeCreate(object other);
        /// <summary>
        /// The AfterCreate method is called after a new instance of the History class is created.
        /// </summary>
        /// <param name="instance">The newly created instance of the History class.</param>
        /// <param name="other">An additional object parameter.</param>
        /// <remarks>
        /// This method is a partial method, meaning it is intended to be implemented in a separate file
        /// to provide custom functionality. The AfterCreate method can be used to perform any necessary
        /// actions or modifications after a History object is created.
        /// </remarks>
        static partial void AfterCreate(QuickTemplate.Logic.Models.Revision.History instance, object other);
        /// <summary>
        /// Executes before creating a new history record for a revision.
        /// </summary>
        /// <param name="other">The other history record being created.</param>
        /// <remarks>
        /// This method is called before a new history record is created for a revision.
        /// Allows additional logic to be executed before the history gets created.
        /// </remarks>
        static partial void BeforeCreate(QuickTemplate.Logic.Models.Revision.History other);
        /// <summary>
        /// This method is invoked after creating a new instance of the History class.
        /// </summary>
        /// <param name="instance">The newly created instance of the History class.</param>
        /// <param name="other">Another instance of the History class.</param>
        static partial void AfterCreate(QuickTemplate.Logic.Models.Revision.History instance, QuickTemplate.Logic.Models.Revision.History other);
        /// <summary>
        /// Triggers before the creation of a new revision history entry.
        /// </summary>
        /// <param name="other">The other revision history entry that is being created.</param>
        /// <remarks>
        /// This method allows for custom logic to be executed before a new revision history entry is created.
        /// Use this method to perform any necessary actions or validations before the creation of the entry.
        /// </remarks>
        static partial void BeforeCreate(QuickTemplate.Logic.Entities.Revision.History other);
        /// <summary>
        /// This method is called after creating an instance of the <see cref="QuickTemplate.Logic.Models.Revision.History"/> class.
        /// </summary>
        /// <param name="instance">The newly created instance of the <see cref="QuickTemplate.Logic.Models.Revision.History"/> class.</param>
        /// <param name="other">An instance of the <see cref="QuickTemplate.Logic.Entities.Revision.History"/> class.</param>
        static partial void AfterCreate(QuickTemplate.Logic.Models.Revision.History instance, QuickTemplate.Logic.Entities.Revision.History other);
    }
}
#endif
//MdEnd

