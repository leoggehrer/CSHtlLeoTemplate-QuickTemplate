//@BaseCode
//MdStart
using System.Collections;
using CommonBase.Extensions;

namespace CommonBase.Modules.Collection
{
    public partial class DelegateList<TInnerModel, TOutModel> : IList<TOutModel>
        where TInnerModel: class, new()
        where TOutModel: class, new()
    {
        #region Fields
        private List<TInnerModel> _innerlist;
        #endregion Fields

        public DelegateList(List<TInnerModel> list)
        {
            _innerlist = list;
            _toOutModel = ToOutModel;
            _toInnerModel = ToInnerModel;
        }

        public DelegateList(List<TInnerModel> innerlist, Func<TInnerModel, TOutModel> toOutModel)
            : this(innerlist)
        {
            _toOutModel = toOutModel;
        }
        public DelegateList(List<TInnerModel> innerlist, Func<TInnerModel, TOutModel> toOutModel, Func<TOutModel, TInnerModel> toInnerModel) 
            : this(innerlist)
        {
            _toOutModel = toOutModel;
            _toInnerModel = toInnerModel;
        }

        #region Implement IList<>
        public TOutModel this[int index] 
        {
            get => _toOutModel(_innerlist[index]);
            set => _innerlist[index] = _toInnerModel(value);
        }

        public int Count => _innerlist.Count;
        public bool IsReadOnly => false;

        public void Add(TOutModel item)
        {
            _innerlist.Add(_toInnerModel(item));
        }

        public void Clear()
        {
            _innerlist.Clear();
        }

        public bool Contains(TOutModel item)
        {
            return _innerlist.Contains(_toInnerModel(item));
        }

        public void CopyTo(TOutModel[] array, int arrayIndex)
        {
            var innerArray = new TInnerModel[array.Length];

            _innerlist.CopyTo(innerArray, arrayIndex);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = _toOutModel(innerArray[i]);
            }
        }

        public IEnumerator<TOutModel> GetEnumerator()
        {
            return new DelegateEnumerator<TOutModel, TInnerModel>(_innerlist.GetEnumerator(), _toOutModel);
        }

        public int IndexOf(TOutModel item)
        {
            var innerModel = _toInnerModel(item);

            return _innerlist.IndexOf(innerModel);
        }

        public void Insert(int index, TOutModel item)
        {
            _innerlist.Insert(index, _toInnerModel(item));
        }

        public bool Remove(TOutModel item)
        {
            return _innerlist.Remove(_toInnerModel(item));
        }

        public void RemoveAt(int index)
        {
            _innerlist.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion Implement IList<>

        #region Implement enumerator
        private class DelegateEnumerator<TOut, TInner> : IEnumerator<TOut>
            where TOut : class, new()
        {
            private IEnumerator<TInner> _enumerator;
            public TOut Current
            {
                get
                {
                    var result = default(TOut);

                    if (_enumerator.Current != null)
                    {
                        result = _toOut(_enumerator.Current);
                    }
                    return result ?? new TOut();
                }
            }
            object IEnumerator.Current => Current;

            public DelegateEnumerator(IEnumerator<TInner> enumerator, Func<TInner, TOut> toOut)
            {
                _enumerator = enumerator;
                _toOut = toOut;
            }
            public void Dispose()
            {
                _enumerator.Dispose();
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }

            private Func<TInner, TOut> _toOut;
        }
        #endregion Implement enumerator

        #region Helper
        private Func<TInnerModel, TOutModel> _toOutModel;
        private Func<TOutModel, TInnerModel> _toInnerModel;

        protected virtual TOutModel ToOutModel(TInnerModel model)
        {
            var result = new TOutModel();

            result.CopyFrom(model);
            return result;
        }
        protected virtual TInnerModel ToInnerModel(TOutModel model)
        {
            var result = new TInnerModel();

            result.CopyFrom(model);
            return result;
        }
        #endregion Helper
    }
}
//MdEnd
