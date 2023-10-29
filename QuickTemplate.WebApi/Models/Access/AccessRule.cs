﻿//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.WebApi.Models.Access
{
    using System;
    ///
    /// Generated by the generator
    ///
    public partial class AccessRule : VersionModel
    {
        ///
        /// Generated by the generator
        ///
        static AccessRule()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called just before a class is being constructed.
        /// </summary>
        /// <remarks>
        /// Implement this method to perform any necessary actions or initialization
        /// before the class is fully constructed.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the constructor of the class is called.
        /// </summary>
        /// <remarks>
        /// This method can be used to perform additional initialization tasks after the class is constructed.
        /// </remarks>
        /// <seealso cref="ClassName"/>
        static partial void ClassConstructed();
        ///
        /// Generated by the generator
        ///
        public AccessRule()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called when constructing an object.
        /// </summary>
        /// <remarks>
        /// Use this method to initialize or set up any necessary resources or variables before the object is fully constructed.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// Represents a partial method that is called when an object is constructed.
        /// </summary>
        /// <remarks>
        /// This method is intended to be overridden by partial classes to provide additional initialization logic.
        /// </remarks>
        /// <returns>
        /// This method does not return a value.
        /// </returns>
        partial void Constructed();
        ///
        /// Generated by the generator
        ///
        public Logic.Modules.Access.RuleType Type
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.String EntityType
        {
            get;
            set;
        }
        = string.Empty;
        ///
        /// Generated by the generator
        ///
        public System.String? RelationshipEntityType
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.String? PropertyName
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.String? EntityValue
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public Logic.Modules.Access.AccessType AccessType
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.String? AccessValue
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.Boolean Creatable
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.Boolean Readable
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.Boolean Updatable
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.Boolean Deletable
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.Boolean Viewable
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.DateTime CreatedOn
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public System.DateTime? ModifiedOn
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public IdType? IdentityId_CreatedBy
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public IdType? IdentityId_ModifiedBy
        {
            get;
            set;
        }
        ///
        /// Generated by the generator
        ///
        public static WebApi.Models.Access.AccessRule Create()
        {
            BeforeCreate();
            var result = new QuickTemplate.WebApi.Models.Access.AccessRule();
            AfterCreate(result);
            return result;
        }
        ///
        /// Generated by the generator
        ///
        public static WebApi.Models.Access.AccessRule Create(object other)
        {
            BeforeCreate(other);
            var result = new QuickTemplate.WebApi.Models.Access.AccessRule();
            CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// This method is called before the creation process. It can be overridden to add custom logic.
        /// </summary>
        static partial void BeforeCreate();
        /// <summary>
        /// This method is called after creating an instance of the AccessRule through the WebApi.
        /// </summary>
        /// <param name="instance">The newly created AccessRule instance.</param>
        static partial void AfterCreate(WebApi.Models.Access.AccessRule instance);
        /// <summary>
        /// Executes before the creation of an object
        /// </summary>
        /// <param name="other">The other object</param>
        /// <remarks>
        /// This method is called before creating an object. It can be used to perform any necessary logic or validation before the object is created.
        /// </remarks>
        static partial void BeforeCreate(object other);
        /// <summary>
        /// This method is called after the creation of an AccessRule instance, allowing for additional actions to be taken.
        /// </summary>
        /// <param name="instance">The AccessRule instance that has been created.</param>
        /// <param name="other">An object carrying additional data related to the creation process.</param>
        /// <remarks>
        /// This method performs specific actions that need to be executed after the creation of an AccessRule object.
        /// It can be used to update related data, trigger events, or perform any other necessary operations.
        /// </remarks>
        static partial void AfterCreate(WebApi.Models.Access.AccessRule instance, object other);
        ///
        /// Generated by the generator
        ///
        public void CopyProperties(WebApi.Models.Access.AccessRule other)
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
#if ROWVERSION_ON
                RowVersion = other.RowVersion;
#endif
#if CREATED_ON
                CreatedOn = other.CreatedOn;
#endif
#if CREATEDBY_ON
                IdentityId_CreatedBy = other.IdentityId_CreatedBy;
#endif
#if MODIFIED_ON
                ModifiedOn = other.ModifiedOn;
#endif
#if MODIFIEDBY_ON
                IdentityId_ModifiedBy = other.IdentityId_ModifiedBy;
#endif
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// Executes the before copy properties logic for an Access Rule.
        /// </summary>
        /// <param name="other">The AccessRule object to copy properties from.</param>
        /// <param name="handled">A reference to a bool value indicating whether the before copy properties logic has been handled. Set to true if handled; otherwise, set to false.</param>
        /// <remarks>
        /// This method is called before copying the properties from another AccessRule object.
        /// It allows custom logic to be executed before the properties are copied.
        /// Any modifications made to the handled parameter will be reflected in the calling method.
        /// </remarks>
        partial void BeforeCopyProperties(WebApi.Models.Access.AccessRule other, ref bool handled);
        /// <summary>
        /// Performs some action after copying properties from the given AccessRule object.
        /// </summary>
        /// <param name="other">The AccessRule object from which properties are being copied.</param>
        partial void AfterCopyProperties(WebApi.Models.Access.AccessRule other);
    }
}
#endif
//MdEnd

