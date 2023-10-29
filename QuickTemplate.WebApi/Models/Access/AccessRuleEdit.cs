﻿//@CodeCopy
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.WebApi.Models.Access
{
    using System;
    ///
    /// Generated by the generator
    ///
    public partial class AccessRuleEdit
    {
        ///
        /// Generated by the generator
        ///
        static AccessRuleEdit()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the constructor of the class is called. It allows for additional operations to be performed during class construction.
        /// </summary>
        /// <remarks>
        /// Implement this method in a partial class to add custom logic before the class is constructed.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// Represents a method that is executed when the class is constructed.
        /// </summary>
        /// <remarks>
        /// This method is declared as partial, meaning it can be implemented in different files.
        /// </remarks>
        /// <returns>
        /// This method does not return a value.
        /// </returns>
        static partial void ClassConstructed();
        ///
        /// Generated by the generator
        ///
        public AccessRuleEdit()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the class.
        /// </summary>
        /// <remarks>
        /// The partial keyword is used to indicate that this method is only a part of the class and is implemented in another file.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is fully constructed.
        /// </summary>
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
        public static WebApi.Models.Access.AccessRuleEdit Create()
        {
            BeforeCreate();
            var result = new QuickTemplate.WebApi.Models.Access.AccessRuleEdit();
            AfterCreate(result);
            return result;
        }
        ///
        /// Generated by the generator
        ///
        public static WebApi.Models.Access.AccessRuleEdit Create(object other)
        {
            BeforeCreate(other);
            var result = new WebApi.Models.Access.AccessRuleEdit();
            CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// This method is a hook that is called before the execution of the 'Create' method.
        /// </summary>
        static partial void BeforeCreate();
        /// <summary>
        /// Invoked after creating an instance of the AccessRuleEdit model.
        /// </summary>
        /// <param name="instance">The created AccessRuleEdit instance.</param>
        /// <remarks>
        /// This method is a partial method and can be implemented in another part of the code.
        /// It allows you to perform additional actions or logic after an AccessRuleEdit instance has been created.
        /// </remarks>
        static partial void AfterCreate(WebApi.Models.Access.AccessRuleEdit instance);
        /// <summary>
        /// This method is called before creating a new object.
        /// </summary>
        /// <param name="other">The object that is being created.</param>
        /// <remarks>
        /// This method is a partial method, which means it can be implemented
        /// in another part of the code if needed. It is called before the actual
        /// creation of the object takes place, allowing for any additional logic
        /// or checks to be performed.
        /// </remarks>
        static partial void BeforeCreate(object other);
        /// <summary>
        /// Represents an event that is executed after creating an instance of <see cref="AccessRuleEdit"/> in the WebApi.Models.Access namespace.
        /// </summary>
        /// <param name="instance">The newly created instance of <see cref="AccessRuleEdit"/>.</param>
        /// <param name="other">An object representing another related entity.</param>
        /// <remarks>
        /// This event provides a way to perform additional operations after creating an instance of <see cref="AccessRuleEdit"/>.
        /// The <paramref name="instance"/> parameter represents the newly created instance of <see cref="AccessRuleEdit"/>,
        /// while the <paramref name="other"/> parameter can be used to pass any other related entity information required for the event.
        /// </remarks>
        static partial void AfterCreate(WebApi.Models.Access.AccessRuleEdit instance, object other);
        ///
        /// Generated by the generator
        ///
        public void CopyProperties(WebApi.Models.Access.AccessRuleEdit other)
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
                CreatedOn = other.CreatedOn;
                ModifiedOn = other.ModifiedOn;
                IdentityId_CreatedBy = other.IdentityId_CreatedBy;
                IdentityId_ModifiedBy = other.IdentityId_ModifiedBy;
            }
            AfterCopyProperties(other);
        }
        /// <summary>
        /// This method is called before copying properties from another AccessRuleEdit object.
        /// </summary>
        /// <param name="other">The AccessRuleEdit object to copy properties from.</param>
        /// <param name="handled">A reference parameter that indicates whether the copying of properties has been handled by this method.</param>
        /// <remarks>
        /// This is a partial method that can be implemented in a partial class.
        /// </remarks>
        partial void BeforeCopyProperties(WebApi.Models.Access.AccessRuleEdit other, ref bool handled);
        /// <summary>
        /// Performs additional tasks after copying properties from another instance of AccessRuleEdit.
        /// </summary>
        /// <param name="other">The AccessRuleEdit instance from which properties are copied.</param>
        partial void AfterCopyProperties(WebApi.Models.Access.AccessRuleEdit other);
    }
}
#endif
//MdEnd
