//@BaseCode
//MdStart
namespace QuickTemplate.AspMvc.Controllers
{
    using AspMvc.Models.View;
    using Microsoft.AspNetCore.Mvc;
    
    /// <summary>
    /// Abstract partial class that serves as a base for filterable generic controllers.
    /// </summary>
    /// <typeparam name="TAccessModel">The access model type.</typeparam>
    /// <typeparam name="TViewModel">The view model type.</typeparam>
    /// <typeparam name="TFilterModel">The filter model type.</typeparam>
    /// <typeparam name="TAccessContract">The access contract type.</typeparam>
    /// <remarks>
    /// This class represents a partial implementation of a generic controller that handles filtering functionality.
    /// It inherits from the <see cref="Controllers.GenericController{TAccessModel, TViewModel}"/> class.
    /// </remarks>
    public abstract partial class FilterGenericController<TAccessModel, TViewModel, TFilterModel, TAccessContract> : Controllers.GenericController<TAccessModel, TViewModel>
    where TAccessModel : BaseContracts.IIdentifyable, new()
    where TViewModel : class, new()
    where TFilterModel : class, IFilterModel, new()
    where TAccessContract : BaseContracts.IBaseAccess<TAccessModel>
    {
        /// <summary>
        /// Static constructor for the FilterGenericController class.
        /// </summary>
        static FilterGenericController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the class is constructed.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// Represents a partial method that gets called when the class is constructed.
        /// </summary>
        /// <remarks>
        /// This method is a placeholder and needs to be implemented in a partial class.
        /// </remarks>
        /// <seealso cref="MyClass"/>
        static partial void ClassConstructed();
        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        protected abstract string ControllerName { get; }
        /// <summary>
        /// Gets the name of the filter model type.
        /// </summary>
        /// <value>
        /// The name of the filter model type.
        /// </value>
        private static string FilterName => typeof(TFilterModel).Name;
        /// <summary>
        /// Gets the order by name.
        /// </summary>
        /// <value>The order by name in the format '{ControllerName}.OrderBy'.</value>
        private string OrderByName => $"{ControllerName}.OrderBy";
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterGenericController{TAccessContract}"/> class.
        /// </summary>
        /// <param name="dataAccess">The data access contract.</param>
        protected FilterGenericController(TAccessContract dataAccess)
        : base(dataAccess)
        {
        }
        /// <summary>
        /// Performs some actions before converting a TAccessModel object to a TViewModel object.
        /// </summary>
        /// <param name="accessModel">The TAccessModel object to convert.</param>
        /// <param name="actionMode">The action mode indicating the type of action.</param>
        /// <param name="viewModel">The TViewModel object that will be populated.</param>
        /// <param name="handled">A reference to a flag indicating if the action has been handled.</param>
        partial void BeforeToViewModel(TAccessModel accessModel, ActionMode actionMode, ref TViewModel? viewModel, ref bool handled);
        /// <summary>
        /// Executes after the conversion of the model to the view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="actionMode">The action mode indicating the operation being performed.</param>
        /// <remarks>
        /// This method is called after the model has been successfully converted to the corresponding view model.
        /// It can be used to perform additional logic or modifications on the view model as needed.
        /// </remarks>
        /// <seealso cref="BeforeToViewModel(TModel, ActionMode)"/>
        /// <seealso cref="ToViewModel(TModel, ActionMode)"/>
        /// <seealso cref="ViewModelToModelConverter{TModel, TViewModel}"/>
        partial void AfterToViewModel(TViewModel viewModel, ActionMode actionMode);
        /// <summary>
        /// Clears the current filter and redirects to the Index action.
        /// </summary>
        /// <returns>An IActionResult representing the redirection to the Index action.</returns>
        public IActionResult Clear()
        {
            var filter = new TFilterModel();
            ViewBag.Filter = filter;
            SessionWrapper.Set<TFilterModel>(FilterName, filter);
            // ReSharper disable once Mvc.ActionNotResolved
            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Retrieves and displays a paged list of items in the default index view.
        /// </summary>
        /// <returns>The <see cref="IActionResult"/> representing the result of the operation.</returns>
        public override async Task<IActionResult> Index()
        {
            IActionResult? result;
            var modelCount = 0;
            var pageSize = DataAccess.MaxPageSize;
            var filter = SessionWrapper.Get<TFilterModel>(FilterName) ?? new TFilterModel();
            var orderBy = SessionWrapper.Get<string>(OrderByName) ?? string.Empty;
            if (filter.HasEntityValue)
            {
                var predicate = filter.CreateEntityPredicate();
                var accessModels = string.IsNullOrEmpty(orderBy) ? await DataAccess.QueryAsync(predicate, 0, pageSize) : await DataAccess.QueryAsync(predicate, orderBy, 0, pageSize);
                var viewModels = AfterQuery(accessModels).Select(e => ToViewModel(e, ActionMode.Index));
                modelCount = await DataAccess.CountAsync(predicate);
                result = View(BeforeView(viewModels, ActionMode.Index));
            }
            else
            {
                var accessModels = string.IsNullOrEmpty(orderBy) ? await DataAccess.GetPageListAsync(0, pageSize) : await DataAccess.GetPageListAsync(orderBy, 0, pageSize);
                var viewModels = AfterQuery(accessModels).Select(e => ToViewModel(e, ActionMode.Index));
                modelCount = await DataAccess.CountAsync();
                result = View(BeforeView(viewModels, ActionMode.Index));
            }
            ViewBag.Filter = filter;
            ViewBag.OrderBy = orderBy;
            ViewBag.PageSize = pageSize;
            ViewBag.ModelCount = modelCount;
            return result;
        }
        /// <summary>
        /// Filters the specified <typeparamref name="TFilterModel"/> and sets it in the session.
        /// </summary>
        /// <param name="filter">The filter model to be set in the session.</param>
        /// <returns>Returns a <see cref="IActionResult"/> representing the action to be taken.</returns>
        /// <remarks>
        /// Sets the specified <typeparamref name="TFilterModel"/> in the session using the <see cref="SessionWrapper"/> class.
        /// Then, redirects to the "Index" action using <see cref="RedirectToActionResult"/>.
        /// </remarks>
        public IActionResult Filter(TFilterModel filter)
        {
            SessionWrapper.Set<TFilterModel>(FilterName, filter);
            // ReSharper disable once Mvc.ActionNotResolved
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Sets the order by name and redirects to the Index action.
        /// </summary>
        /// <param name="orderBy">The name used for ordering.</param>
        /// <returns>The result of the Index action.</returns>
        public IActionResult OrderBy(string orderBy)
        {
            SessionWrapper.Set<string>(OrderByName, orderBy);
            // ReSharper disable once Mvc.ActionNotResolved
            return RedirectToAction("Index");
        }
    }
}
//MdEnd


