//@BaseCode
//MdStart
#nullable disable
using Microsoft.AspNetCore.Mvc;
#if ACCOUNT_ON
using Microsoft.AspNetCore.Mvc.Filters;
#endif

namespace QuickTemplate.AspMvc.Controllers
{
    /// <summary>
    /// A generic one for the standard CRUD operations.
    /// </summary>
    /// <typeparam name="TAccessModel">The type of access model</typeparam>
    /// <typeparam name="TViewModel">The type of view model</typeparam>
    public abstract partial class GenericController<TAccessModel, TViewModel> : MvcController
    where TAccessModel : BaseContracts.IIdentifyable, new()
    where TViewModel : class, new()
    {
        protected enum ActionMode : int
        {
            Index = 1,
            Details = 2,
            ViewCreate = 4,
            Create = 8,
            ViewEdit = 16,
            Edit = 32,
            ViewDelete = 64,
            Delete = 128,
            
            CreateAction = ViewCreate + Create,
            EditAction = ViewEdit + Edit,
            DeleteAction = ViewDelete + Delete,
            
            CreateOrEditAction = CreateAction + EditAction,
        }
        /// <summary>
        /// Gets or sets the data access object for accessing the repository.
        /// </summary>
        /// <remarks>
        /// The data access object provides methods for accessing the repository to perform CRUD operations.
        /// </remarks>
        /// <value>
        /// The data access object implementing <see cref="BaseContracts.IBaseAccess{TAccessModel}"/> interface.
        /// </value>
        protected BaseContracts.IBaseAccess<TAccessModel> DataAccess { get; init; }
        
        ///<summary>
        /// Creates a new instance of the <see cref="GenericController{TAccessModel}"/> class.
        ///</summary>
        ///<param name="dataAccess">An implementation of the <see cref="BaseContracts.IBaseAccess{TAccessModel}"/> interface.</param>
        protected GenericController(BaseContracts.IBaseAccess<TAccessModel> dataAccess)
        {
            this.DataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }
        
#if ACCOUNT_ON
        /// <summary>
        /// Overrides the base method, initializes the DataAccess.SessionToken property with the value from SessionWrapper.SessionToken.
        /// </summary>
        /// <param name="context">The context for the action being executed.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            
            DataAccess.SessionToken = SessionWrapper.SessionToken;
        }
#endif
        /// <summary>
        /// Redirects to the Index action of the current controller with a specified action mode and access model.
        /// </summary>
        /// <param name="actionMode">The action mode to be passed to the Index action.</param>
        /// <param name="accessModel">The access model to be passed to the Index action.</param>
        /// <returns>A <see cref="RedirectToActionResult"/> that redirects to the Index action of the current controller.</returns>
        protected virtual RedirectToActionResult RedirectAfterAction(ActionMode actionMode, TAccessModel accessModel)
        {
            // ReSharper disable once Mvc.ActionNotResolved
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Converts the given view model to an access model.
        /// </summary>
        /// <param name="viewModel">The view model to convert.</param>
        /// <returns>The converted access model.</returns>
        /// <typeparam name="TAccessModel">The type of the access model.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        protected virtual TAccessModel ToAccessModel(TViewModel viewModel)
        {
            var result = new TAccessModel();
            
            result.CopyFrom(viewModel);
            return result;
        }
        /// <summary>
        /// Converts the given access model to a view model of type TViewModel.
        /// </summary>
        /// <param name="accessModel">The access model to convert.</param>
        /// <param name="actionMode">The action mode to be applied.</param>
        /// <returns>The converted view model.</returns>
        protected virtual TViewModel ToViewModel(TAccessModel accessModel, ActionMode actionMode)
        {
            var result = new TViewModel();
            
            result.CopyFrom(accessModel);
            return BeforeView(result, actionMode);
        }
        
        /// <summary>
        /// Implements the logic to execute after querying the specified collection of access models.
        /// </summary>
        /// <param name="accessModels">The collection of access models returned after querying.</param>
        /// <returns>The collection of access models after executing any necessary post-query operations.</returns>
        protected virtual IEnumerable<TAccessModel> AfterQuery(IEnumerable<TAccessModel> accessModels) => accessModels;
        /// <summary>
        /// Overrides the default behavior before displaying a view with the given view model and action mode.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model to be displayed.</param>
        /// <param name="actionMode">The action mode of the view.</param>
        /// <returns>The updated view model.</returns>
        protected virtual TViewModel BeforeView(TViewModel viewModel, ActionMode actionMode) => viewModel;
        /// <summary>
        /// A method that executes before the view is displayed.
        /// </summary>
        /// <param name="viewModels">The collection of view models.</param>
        /// <param name="actionMode">The action mode to determine the behavior.</param>
        /// <returns>The modified collection of view models.</returns>
        protected virtual IEnumerable<TViewModel> BeforeView(IEnumerable<TViewModel> viewModels, ActionMode actionMode) => viewModels;
        
        // GET: Items
        /// <summary>
        /// This method returns the main page of the application.
        /// </summary>
        /// <returns>Returns a task representing the asynchronous operation. The task result is an IActionResult.</returns>
        public virtual async Task<IActionResult> Index()
        {
            var pageSize = DataAccess.MaxPageSize;
            var modelCount = await DataAccess.CountAsync();
            var accessModels = await DataAccess.GetPageListAsync(0, pageSize);
            var viewModels = AfterQuery(accessModels).Select(e => ToViewModel(e, ActionMode.Index)).ToArray();
            
            ViewBag.PageSize = pageSize;
            ViewBag.ModelCount = modelCount;
            return View(BeforeView(viewModels, ActionMode.Index));
        }
        // GET: Items
        /// <summary>
        /// Redirects the user back to the index page.
        /// </summary>
        /// <returns>The result of the action.</returns>
        // ReSharper disable once Mvc.ActionNotResolved
        public virtual IActionResult BackToIndex()
        {
            // ReSharper disable once Mvc.ActionNotResolved
            return RedirectToAction(nameof(Index));
        }
        // GET: Item/Details/5
        /// <summary>
        /// Retrieves the details of a specific record by its ID.
        /// </summary>
        /// <param name="id">The ID of the record to retrieve details for.</param>
        /// <returns>The IActionResult representing the result of the operation.</returns>
        public virtual async Task<IActionResult> Details(IdType? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var accessModel = await DataAccess.GetByIdAsync(id.Value);
            
            if (accessModel == null)
            {
                return NotFound();
            }
            return View(ToViewModel(accessModel, ActionMode.Details));
        }
        
        // GET: Item/Create
        /// <summary>
        /// Creates a new instance of TAccessModel and returns a ViewResult containing a ViewModel for the Create action.
        /// </summary>
        /// <returns>A ViewResult containing the ViewModel for the Create action.</returns>
        public virtual IActionResult Create()
        {
            var accessModel = new TAccessModel();
            
            return View(ToViewModel(accessModel, ActionMode.ViewCreate));
        }
        
        // POST: Item/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creates a new instance of the specified ViewModel and saves it to the data store.
        /// </summary>
        /// <param name="viewModel">The ViewModel object used to create the new instance.</param>
        /// <returns>An asynchronous task that represents the operation and contains the IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(TViewModel viewModel)
        {
            TAccessModel accessModel = ToAccessModel(viewModel);
            
            if (ModelState.IsValid)
            {
                try
                {
                    accessModel = await DataAccess.InsertAsync(accessModel);
                    await DataAccess.SaveChangesAsync();
                    return RedirectAfterAction(ActionMode.Create, accessModel);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    
                    if (ex.InnerException != null)
                    {
                        ViewBag.Error = ex.InnerException.Message;
                    }
                }
            }
            else
            {
                ViewBag.Error = string.Join("; ", ModelState.Values
                       .SelectMany(x => x.Errors)
                       .Select(x => x.ErrorMessage));
            }
            return View(ToViewModel(accessModel, ActionMode.Index));
        }
        
        // GET: Item/Edit/5
        /// <summary>
        /// Edits a specific resource by its ID.
        /// </summary>
        /// <param name="id">The ID of the resource to edit.</param>
        /// <returns>An asynchronous task that returns an IActionResult representing the result of the edit operation.</returns>
        /// <remarks>
        /// If the provided ID is null, the method returns a NotFound result.
        /// If the resource with the provided ID is not found, the method returns a NotFound result.
        /// Otherwise, the method retrieves the resource using the ID and converts it to a view model with the requested action mode (ViewEdit).
        /// The method returns a View result with the converted view model.
        /// </remarks>
        public virtual async Task<IActionResult> Edit(IdType? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var accessModel = await DataAccess.GetByIdAsync(id.Value);
            
            if (accessModel == null)
            {
                return NotFound();
            }
            return View(ToViewModel(accessModel, ActionMode.ViewEdit));
        }
        
        // POST: Item/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edits a record with the specified ID and view model.
        /// </summary>
        /// <param name="id">The ID of the record to edit.</param>
        /// <param name="viewModel">The view model containing the updated data.</param>
        /// <returns>An asynchronous task that returns an IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(IdType id, TViewModel viewModel)
        {
            var accessModel = await DataAccess.GetByIdAsync(id);
            
            if (accessModel == null)
            {
                return NotFound();
            }
            
            accessModel.CopyFrom(viewModel);
            
            if (ModelState.IsValid)
            {
                try
                {
                    accessModel = await DataAccess.UpdateAsync(accessModel);
                    await DataAccess.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    
                    if (ex.InnerException != null)
                    {
                        ViewBag.Error = ex.InnerException.Message;
                    }
                }
            }
            else
            {
                ViewBag.Error = string.Join("; ", ModelState.Values
                       .SelectMany(x => x.Errors)
                       .Select(x => x.ErrorMessage));
            }
            return string.IsNullOrEmpty(ViewBag.Error) ? RedirectAfterAction(ActionMode.Edit, accessModel) : View(ToViewModel(accessModel, ActionMode.Edit));
        }
        
        // GET: Item/Delete/5
        /// <summary>
        /// Deletes a specific item with the provided ID.
        /// </summary>
        /// <param name="id">The ID of the item to be deleted.</param>
        /// <returns>The action result representing the view to be rendered.</returns>
        public virtual async Task<IActionResult> Delete(IdType? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var accessModel = await DataAccess.GetByIdAsync(id.Value);
            
            if (accessModel == null)
            {
                return NotFound();
            }
            return View(ToViewModel(accessModel, ActionMode.ViewDelete));
        }
        
        // POST: Item/Delete/5
        ///<summary>
        /// Deletes a record with the specified ID.
        ///</summary>
        ///<param name="id">The ID of the record to be deleted.</param>
        ///<returns>The action result that represents the response of the delete operation.</returns>
        ///<remarks>
        /// This method is decorated with the [HttpPost] and [ValidateAntiForgeryToken] attributes.
        /// The [HttpPost] attribute indicates that this method is only invoked for HTTP POST requests.
        /// The [ValidateAntiForgeryToken] attribute ensures that the request includes a valid anti-forgery token.
        ///</remarks>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DeleteConfirmed(IdType id)
        {
            var accessModel = await DataAccess.GetByIdAsync(id);
            
            if (accessModel != null)
            {
                try
                {
                    await DataAccess.DeleteAsync(id);
                    await DataAccess.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    
                    if (ex.InnerException != null)
                    {
                        ViewBag.Error = ex.InnerException.Message;
                    }
                }
            }
            return ViewBag.Error != null ? View(ToViewModel(accessModel, ActionMode.Delete)) : RedirectAfterAction(ActionMode.Delete, accessModel);
        }
        
        /// <summary>
        /// Releases the resources used by the DataAccess object and any other resources used by the DisposableEntity.
        /// </summary>
        /// <param name="disposing">Indicates whether the method is called from Dispose or from the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            DataAccess?.Dispose();
            base.Dispose(disposing);
        }
    }
}
//MdEnd

