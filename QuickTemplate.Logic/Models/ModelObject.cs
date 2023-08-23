//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Models
{
    public abstract partial class ModelObject : BaseModels.ModelObject
    {
        protected Entities.EntityObject? _source;

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
