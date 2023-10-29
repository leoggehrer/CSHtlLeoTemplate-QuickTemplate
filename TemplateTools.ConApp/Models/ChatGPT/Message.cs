//@BaseCode
//MdStart
namespace TemplateTooles.ConApp.Models.ChatGPT
{
    /// <summary>
    /// Represents a message object.
    /// </summary>
    public partial class Message
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the content of the property.
        /// </summary>
        /// <value>
        /// The content of the property.
        /// </value>
        public string Content { get; set; } = string.Empty;
    }
}
//MdEnd
