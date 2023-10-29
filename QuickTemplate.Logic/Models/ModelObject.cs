//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Models
{
    /// <summary>
    /// Represents an abstract base class for model objects.
    /// </summary>
    /// <typeparam name="T">The type of the entity object.</typeparam>
    public abstract partial class ModelObject : BaseModels.ModelObject
    {
        protected Entities.EntityObject? _source;
        
        /// <summary>
        /// Gets or sets the source entity object.
        /// </summary>
        /// <value>
        /// The source entity object being accessed.
        /// </value>
        internal virtual Entities.EntityObject Source
        {
            get => _source!;
            set => _source = value;
        }
        /// <summary>
        /// ID of the entity (primary key)
        /// </summary>
        public override IdType Id
        {
            get => Source?.Id ?? default;
            set
            {
                if (Source != null)
                {
                    Source.Id = value;
                }
            }
        }
    }
}
//MdEnd
