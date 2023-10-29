//@BaseCode
//MdStart

namespace QuickTemplate.AspMvc.Models.View
{
    /// <summary>
    /// Represents a command for submitting a form.
    /// </summary>
    public partial class SubmitCmd
    {
        /// <summary>
        /// Initializes a new instance of the SubmitCmd class.
        /// </summary>
        public SubmitCmd()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction phase of an object.
        /// It is meant to be overridden by derived classes to perform any additional initialization.
        /// </summary>
        partial void Constructing();
        /// <summary>
        /// Adds functionality to be executed when the object is fully constructed.
        /// This method is not intended to be called directly, but rather to be implemented by partial classes.
        /// </summary>
        /// <remarks>
        /// This method should be overridden in a partial class to include any construction logic that needs to be executed after all the object's fields and properties are initialized.
        /// It provides the opportunity to perform additional setup or initialization steps before the object is used.
        /// </remarks>
        partial void Constructed();
        
        /// <summary>
        /// Gets or sets a value indicating whether the text should be right aligned.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the text should be right aligned; otherwise, <c>false</c>.
        /// </value>
        public bool RightAlign { get; set; } = false;
        /// <summary>
        /// Gets or sets the text to display on the submit button.
        /// Default value is "Save".
        /// </summary>
        public string SubmitText { get; set; } = "Save";
        /// <summary>
        /// Gets or sets the CSS class to be applied to the submit button.
        /// </summary>
        /// <value>
        /// The CSS class to be applied to the submit button.
        /// </value>
        public string SubmitCss { get; set; } = "btn btn-primary";
        /// <summary>
        /// Gets or sets the submit style.
        /// </summary>
        /// <value>
        /// The style for the submit.
        /// </value>
        public string SubmitStyle { get; set; } = "min-width: 8em;";
        /// <summary>
        /// Gets or sets the SubmitAction for the application.
        /// </summary>
        /// <value>
        /// The SubmitAction for the application.
        /// </value>
        public string SubmitAction { get; set; } = string.Empty;
    }
}
//MdEnd

