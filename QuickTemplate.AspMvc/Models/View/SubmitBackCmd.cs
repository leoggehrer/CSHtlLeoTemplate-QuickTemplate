//@BaseCode
//MdStart
namespace QuickTemplate.AspMvc.Models.View
{
    /// <summary>
    /// Represents a command for submitting and returning back to a previous page.
    /// </summary>
    public partial class SubmitBackCmd : SubmitCmd
    {
        /// <summary>
        /// Initializes a new instance of the SubmitBackCmd class.
        /// </summary>
        /// <remarks>
        /// This constructor calls the Constructing and Constructed methods to perform initialization tasks.
        /// </remarks>
        public SubmitBackCmd()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the object construction process.
        /// </summary>
        /// <remarks>
        /// This method is called just before the object is constructed, giving an opportunity to perform any custom initialization logic.
        /// </remarks>
        /// <seealso cref="Constructing"/>
        partial void Constructing();
        /// <summary>
        /// Partial method called when the object is constructed.
        /// </summary>
        /// <remarks>
        /// This method is intended to be implemented by a partial class or class extension.
        /// It is called when the object is constructed.
        /// </remarks>
        partial void Constructed();
        
        /// <summary>
        /// Gets or sets a value indicating whether the submit action is being performed to go back.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the action to go back is set; otherwise, <c>false</c>.
        /// </value>
        public bool FromSubmitToBack { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the text shown for the back button.
        /// </summary>
        /// <value>
        /// The text displayed for the back button. The default value is "Back to List".
        /// </value>
        public string BackText { get; set; } = "Back to List";
        /// <summary>
        /// Gets or sets the action name for navigating back.
        /// The default value is "BackToIndex".
        /// </summary>
        /// <value>The action name for navigating back.</value>
        public string BackAction { get; set; } = "BackToIndex";
        /// <summary>
        /// Gets or sets the route values for returning to the previous page.
        /// </summary>
        public object? BackRouteValues { get; set; }
        /// <summary>
        /// Gets or sets the name of the back controller.
        /// </summary>
        /// <value>
        /// The name of the back controller.
        /// </value>
        public string? BackController { get; set; }
        /// <summary>
        /// Gets or sets the CSS class for the back button.
        /// </summary>
        public string BackCss { get; set; } = "btn btn-outline-dark";
        /// <summary>
        /// Gets or sets the back style of an element.
        /// </summary>
        /// <value>
        /// The back style, specified as a string in the format "min-width: 8em;".
        /// </value>
        public string BackStyle { get; set; } = "min-width: 8em;";
    }
}
//MdEnd

