//@BaseCode
//MdStart
#if ACCOUNT_ON && LOGGING_ON
namespace QuickTemplate.Logic.Models.Logging
{
    using System;
    ///<summary>
    /// Represents an action log model.
    ///</summary>
    public partial class ActionLog : ModelObject
    {
        /// <summary>
        /// Initializes a new instance of the ActionLog class.
        /// </summary>
        static ActionLog()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the class construction is initiated.
        /// </summary>
        /// <remarks>
        /// This method is defined as partial, allowing the user to implement it in another part of the code.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is instantiated.
        /// </summary>
        /// <remarks>
        /// This method can be implemented in a partial class to provide custom initialization logic.
        /// </remarks>
        /// <seealso cref="YourClassName"/>
        /// <exception cref="Exception">
        /// This method may throw an exception if an error occurs during the class construction process.
        /// </exception>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the ActionLog class.
        /// </summary>
        /// <remarks>
        /// This constructor calls the Constructing and Constructed methods.
        /// </remarks>
        public ActionLog()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called before the object is constructed.
        /// </summary>
        /// <remarks>
        /// Implement this method in partial classes to perform any additional logic before the object is constructed.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is constructed.
        /// </summary>
        /// <remarks>
        /// This method is a partial method and is intended to be implemented by the user.
        /// When implementing this method, the user can initialize any fields or perform any setup required
        /// during object construction.
        /// </remarks>
        partial void Constructed();
        /// <summary>
        /// Gets or sets the source of the action log.
        /// </summary>
        /// <value>
        /// The source of the action log.
        /// </value>
        new internal QuickTemplate.Logic.Entities.Logging.ActionLog Source
        {
            get => (QuickTemplate.Logic.Entities.Logging.ActionLog)(_source ??= new QuickTemplate.Logic.Entities.Logging.ActionLog());
            set => _source = value;
        }
        /// <summary>
        /// Gets or sets the IdentityId of the object.
        /// </summary>
        public IdType IdentityId
        {
            get => Source.IdentityId;
            set => Source.IdentityId = value;
        }
        /// <summary>
        /// Gets or sets the time value of the source object.
        /// </summary>
        /// <value>
        /// The time value of the source object.
        /// </value>
        public System.DateTime Time
        {
            get => Source.Time;
            set => Source.Time = value;
        }
        /// <summary>
        /// Gets or sets the subject of the source.
        /// </summary>
        /// <value>The subject of the source.</value>
        public System.String Subject
        {
            get => Source.Subject;
            set => Source.Subject = value;
        }
        /// <summary>
        /// Gets or sets the action for the property.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public System.String Action
        {
            get => Source.Action;
            set => Source.Action = value;
        }
        /// <summary>
        /// Gets or sets the information related to the source.
        /// </summary>
        /// <value>
        /// The information string.
        /// </value>
        public System.String Info
        {
            get => Source.Info;
            set => Source.Info = value;
        }
        /// <summary>
        /// Copies the properties of the specified ActionLog object to this object, unless the BeforeCopyProperties event handles the copy operation.
        /// </summary>
        /// <param name="other">The ActionLog object from which to copy the properties.</param>
        // If the BeforeCopyProperties event doesn't handle the copy operation
        internal void CopyProperties(QuickTemplate.Logic.Entities.Logging.ActionLog other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                IdentityId = other.IdentityId;
                Time = other.Time;
                Subject = other.Subject;
                Action = other.Action;
                Info = other.Info;
                Id = other.Id;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// Invoked before copying properties from the source <see cref="QuickTemplate.Logic.Entities.Logging.ActionLog"/> to the current instance.
        /// </summary>
        /// <param name="other">The source <see cref="QuickTemplate.Logic.Entities.Logging.ActionLog"/> from which the properties are being copied.</param>
        /// <param name="handled">A reference to a bool indicating whether the copying has been handled. Set to true if handled, false otherwise.</param>
        /// <remarks>
        /// This method allows customization or interception of property copying process before it occurs.
        /// </remarks>
        partial void BeforeCopyProperties(QuickTemplate.Logic.Entities.Logging.ActionLog other, ref bool handled);
        /// <summary>
        /// This method is called after copying properties from another ActionLog object.
        /// </summary>
        /// <param name="other">The ActionLog object from which properties are copied.</param>
        partial void AfterCopyProperties(QuickTemplate.Logic.Entities.Logging.ActionLog other);
        /// <summary>
        /// Copy the properties of the specified ActionLog object to this ActionLog object.
        /// </summary>
        /// <param name="other">The ActionLog object to copy the properties from.</param>
        /// <remarks>
        /// This method copies the values of the IdentityId, Time, Subject, Action, Info, and Id properties
        /// from the specified ActionLog object to this ActionLog object, unless the "BeforeCopyProperties"
        /// event is handled by setting the "handled" parameter to true.
        /// </remarks>
        public void CopyProperties(QuickTemplate.Logic.Models.Logging.ActionLog other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                IdentityId = other.IdentityId;
                Time = other.Time;
                Subject = other.Subject;
                Action = other.Action;
                Info = other.Info;
                Id = other.Id;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// Prepares and handles actions to be performed before copying properties.
        /// </summary>
        /// <param name="other">The ActionLog object to copy properties from.</param>
        /// <param name="handled">A boolean value that indicates if the action has been handled.</param>
        /// <remarks>
        /// This method is called before copying properties from the given ActionLog object.
        /// It allows implementing custom actions or logic to be performed.
        /// The 'handled' parameter can be modified to indicate if the action has been handled or not.
        /// </remarks>
        partial void BeforeCopyProperties(QuickTemplate.Logic.Models.Logging.ActionLog other, ref bool handled);
        /// <summary>
        /// This method is called after the properties of the provided ActionLog object are copied to the current instance.
        /// </summary>
        /// <param name="other">The ActionLog object whose properties are copied to the current instance.</param>
        /// <remarks>
        /// This method can be overridden in a partial class to perform additional operations or check specific conditions
        /// after the properties have been copied.
        /// </remarks>
        partial void AfterCopyProperties(QuickTemplate.Logic.Models.Logging.ActionLog other);
        /// <summary>
        /// Determines whether the current ActionLog object is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare with the current ActionLog.</param>
        /// <returns>
        /// True if the specified object is an ActionLog and has the same Id as the current ActionLog object; otherwise, false.
        /// </returns>
        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is Models.Logging.ActionLog other)
            {
                result = Id == other.Id;
            }
            return result;
        }
        /// <summary>
        /// Computes and returns a hash code based on the values of the properties in this instance.
        /// </summary>
        /// <returns>A hash code value calculated using the combined hash code of IdentityId, Time, Subject, Action, Info, and Id.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(IdentityId, Time, Subject, Action, Info, Id);
        }
        /// <summary>
        /// Creates a new instance of the ActionLog class.
        /// </summary>
        /// <returns>A new ActionLog instance.</returns>
        public static QuickTemplate.Logic.Models.Logging.ActionLog Create()
        {
            BeforeCreate();
            var result = new QuickTemplate.Logic.Models.Logging.ActionLog();
            AfterCreate(result);
            return result;
        }
        /// <summary>
        /// Creates a new instance of the <see cref="QuickTemplate.Logic.Models.Logging.ActionLog"/> class
        /// by copying the properties from the specified object.
        /// </summary>
        /// <param name="other">The object whose properties will be copied to the new instance.</param>
        /// <returns>A new instance of the <see cref="QuickTemplate.Logic.Models.Logging.ActionLog"/> class.</returns>
        public static QuickTemplate.Logic.Models.Logging.ActionLog Create(object other)
        {
            BeforeCreate(other);
            CommonBase.Extensions.ObjectExtensions.CheckArgument(other, nameof(other));
            var result = new QuickTemplate.Logic.Models.Logging.ActionLog();
            CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// Creates a new instance of ActionLog with properties copied from an existing ActionLog.
        /// </summary>
        /// <param name="other">The existing ActionLog instance to be copied.</param>
        /// <returns>A new ActionLog instance with properties copied from the existing ActionLog.</returns>
        /// <remarks>
        /// This method internally calls BeforeCreate and AfterCreate methods for any additional operations before and after copying properties from the existing ActionLog.
        /// </remarks>
        public static QuickTemplate.Logic.Models.Logging.ActionLog Create(QuickTemplate.Logic.Models.Logging.ActionLog other)
        {
            BeforeCreate(other);
            var result = new QuickTemplate.Logic.Models.Logging.ActionLog();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        ///<summary>
        /// Creates a new instance of an ActionLog object using the properties of an existing ActionLog object.
        ///</summary>
        ///<param name="other">The existing ActionLog object to copy properties from.</param>
        ///<returns>A new instance of ActionLog.</returns>
        internal static QuickTemplate.Logic.Models.Logging.ActionLog Create(QuickTemplate.Logic.Entities.Logging.ActionLog other)
        {
            BeforeCreate(other);
            var result = new QuickTemplate.Logic.Models.Logging.ActionLog();
            result.Source = other;
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// This method is called before the creation of an object.
        /// </summary>
        /// <remarks>
        /// This method is a hook that allows you to perform necessary operations or validations before an object is created.
        /// </remarks>
        static partial void BeforeCreate();
        /// <summary>
        /// Called after an instance of the ActionLog model is created.
        /// </summary>
        /// <param name="instance">The newly created instance of ActionLog.</param>
        /// <remarks>
        /// This method is intended to be partially implemented within a partial class.
        /// It provides an extension point to perform additional logic or actions after an ActionLog instance is created.
        /// </remarks>
        static partial void AfterCreate(QuickTemplate.Logic.Models.Logging.ActionLog instance);
        /// <summary>
        ///   This method is called before creating an object.
        /// </summary>
        /// <param name="other">
        ///   The object to be created.
        /// </param>
        /// <remarks>
        ///   The BeforeCreate method allows for custom logic to be executed before creating an object.
        /// </remarks>
        static partial void BeforeCreate(object other);
        /// <summary>
        /// Performs additional operations after creating an ActionLog instance.
        /// </summary>
        /// <param name="instance">The instance of the ActionLog that was created.</param>
        /// <param name="other">An object representing additional data.</param>
        /// <remarks>
        /// This method is part of a partial class and is used to perform specific actions after creating an ActionLog instance.
        /// </remarks>
        static partial void AfterCreate(QuickTemplate.Logic.Models.Logging.ActionLog instance, object other);
        /// <summary>
        /// Executes before creating an ActionLog object.
        /// </summary>
        /// <param name="other">The ActionLog object to be created.</param>
        static partial void BeforeCreate(QuickTemplate.Logic.Models.Logging.ActionLog other);
        /// <summary>
        /// Executed after the creation of an ActionLog instance.
        /// </summary>
        /// <param name="instance">The created ActionLog instance.</param>
        /// <param name="other">Another ActionLog instance.</param>
        /// <remarks>
        /// This method is called internally and can be overridden to perform additional logic
        /// after the creation of an ActionLog instance. The "instance" parameter represents
        /// the newly created ActionLog, and the "other" parameter represents another
        /// ActionLog instance.
        /// </remarks>
        static partial void AfterCreate(QuickTemplate.Logic.Models.Logging.ActionLog instance, QuickTemplate.Logic.Models.Logging.ActionLog other);
        /// <summary>
        /// This method is called before creating an instance of the ActionLog object.
        /// </summary>
        /// <param name="other">The ActionLog object that is about to be created.</param>
        static partial void BeforeCreate(QuickTemplate.Logic.Entities.Logging.ActionLog other);
        /// <summary>
        /// Executes after the creation of an ActionLog instance.
        /// </summary>
        /// <param name="instance">The newly created ActionLog instance.</param>
        /// <param name="other">The ActionLog instance which was used as a template.</param>
        /// <remarks>
        /// This method is a partial method and is executed after the Create method in the QuickTemplate.Logic.Entities.Logging.ActionLog class.
        /// It allows custom logic to be executed after the creation of an ActionLog instance.
        /// </remarks>
        static partial void AfterCreate(QuickTemplate.Logic.Models.Logging.ActionLog instance, QuickTemplate.Logic.Entities.Logging.ActionLog other);
    }
}
#endif
//MdEnd

