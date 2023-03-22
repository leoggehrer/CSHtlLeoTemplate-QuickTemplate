//@BaseCode
//MdStart
using System.Collections;
using CommonBase.Extensions;

namespace CommonBase.Modules.Collection
{
    public partial class DelegateList<TInnerModel, TOutModel> : IList<TOutModel>
        where TInnerModel : class, new()
        where TOutModel : class, new()
    {
        #region Fields
        private readonly List<TInnerModel> _innerList;
        private readonly List<TOutModel> _outerList;
        #endregion Fields

        partial void Constructing();
        partial void Constructed();

        public DelegateList(List<TInnerModel> innerList)
        {
            Constructing();
            _innerList = innerList;
            _toOutModel = ToDefaultOutModel;
            _toInnerModel = ToDefaultInnerModel;
            _outerList = innerList.Select(e => _toOutModel(e)).ToList();
            Constructed();
        }
        public DelegateList(List<TInnerModel> innerList, Func<TInnerModel, TOutModel> toOutModel)
        {
            Constructing();
            _innerList = innerList;
            _toOutModel = toOutModel;
            _toInnerModel = ToDefaultInnerModel;
            _outerList = innerList.Select(e => _toOutModel(e)).ToList();
            Constructed();
        }
        public DelegateList(List<TInnerModel> innerList, Func<TInnerModel, TOutModel> toOutModel, Func<TOutModel, TInnerModel> toInnerModel)
        {
            Constructing();
            _innerList = innerList;
            _toOutModel = toOutModel;
            _toInnerModel = toInnerModel;
            _outerList = innerList.Select(e => _toOutModel(e)).ToList();
            Constructed();
        }

        public Func<TInnerModel, TOutModel> ToOutModel => _toOutModel;
        public Func<TOutModel, TInnerModel> ToInnerModel => _toInnerModel;
        #region Implement IList<>
        public TOutModel this[int index]
        {
            get => _toOutModel(_innerList[index]);
            set => _outerList[index] = value;
        }

        public int Count => _innerList.Count;
        public bool IsReadOnly => false;

        public TOutModel Create() => _toOutModel(new TInnerModel());
        public void Add(TOutModel item)
        {
            _innerList.Add(_toInnerModel(item));
            _outerList.Add(item);
        }

        public void Clear()
        {
            _innerList.Clear();
            _outerList.Clear();
        }

        public bool Contains(TOutModel item)
        {
            return _outerList.Contains(item);
        }

        public void CopyTo(TOutModel[] array, int arrayIndex)
        {
            _outerList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TOutModel> GetEnumerator()
        {
            return _outerList.GetEnumerator();
        }

        public int IndexOf(TOutModel item)
        {
            return _outerList.IndexOf(item);
        }

        public void Insert(int index, TOutModel item)
        {
            _innerList.Insert(index, _toInnerModel(item));
            _outerList.Insert(index, item);
        }

        public bool Remove(TOutModel item)
        {
            _innerList.Remove(_toInnerModel(item));
            return _outerList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _innerList.RemoveAt(index);
            _outerList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion Implement IList<>

        #region Helper
        private Func<TInnerModel, TOutModel> _toOutModel;
        private Func<TOutModel, TInnerModel> _toInnerModel;

        protected virtual TOutModel ToDefaultOutModel(TInnerModel model)
        {
            var result = new TOutModel();

            result.CopyFrom(model);
            return result;
        }
        protected virtual TInnerModel ToDefaultInnerModel(TOutModel model)
        {
            var result = new TInnerModel();

            result.CopyFrom(model);
            return result;
        }
        #endregion Helper
    }
}
//MdEnd
