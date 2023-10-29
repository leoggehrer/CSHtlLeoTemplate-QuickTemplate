﻿//@CodeCopy
//MdStart

namespace CommonBase.Contracts
{
    /// <summary>
    /// An interface representing versionable entities.
    /// </summary>
    public partial interface IVersionable : IIdentifyable
    {
#if ROWVERSION_ON
    /// <summary>
    /// Gets the row version associated with the entity.
    /// </summary>
    /// <remarks>
    /// The row version is a byte array that represents the version of the entity's data.
    /// It can be used for optimistic concurrency control in data storage and retrieval.
    /// </remarks>
    byte[]? RowVersion { get; }
#endif
    }
}
//MdEnd


