//@BaseCode
//MdStart
namespace QuickTemplate.AspMvc.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AspMvc.Models.View;
    using Logic.Contracts;

    public abstract partial class FilterGenericController<TAccessModel, TViewModel, TFilterModel, TAccessContract> : Controllers.GenericController<TAccessModel, TViewModel>
        where TAccessModel : IIdentifyable, new()
        where TViewModel : class, new()
        where TFilterModel : class, IFilterModel, new()
        where TAccessContract : IBaseAccess<TAccessModel>
    {
        static FilterGenericController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        protected static string FilterName => typeof(TFilterModel).Name;
        protected abstract string ControllerName { get; }
        protected string OrderByName => $"{ControllerName}.OrderBy";
        protected FilterGenericController(TAccessContract dataAccess)
            : base(dataAccess)
        {

        }
        partial void BeforeToViewModel(TAccessModel accessModel, ActionMode actionMode, ref TViewModel? viewModel, ref bool handled);
        partial void AfterToViewModel(TViewModel viewModel, ActionMode actionMode);
        public IActionResult Clear()
        {
            var filter = new TFilterModel();
            ViewBag.Filter = filter;
            SessionWrapper.Set<TFilterModel>(FilterName, filter);
            return RedirectToAction("Index");
        }

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
        public IActionResult Filter(TFilterModel filter)
        {
            SessionWrapper.Set<TFilterModel>(FilterName, filter);
            return RedirectToAction("Index");
        }
        public IActionResult OrderBy(string orderBy)
        {
            SessionWrapper.Set<string>(OrderByName, orderBy);
            return RedirectToAction("Index");
        }
    }
}
//MdEnd