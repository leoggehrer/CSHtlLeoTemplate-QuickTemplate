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
        }

        #region Implement IList<>
        public TOutModel this[int index] 
        {
            get => ToOutModel(_innerlist[index]);
            set => _innerlist[index].CopyFrom(value);
        }

        public int Count => _innerlist.Count;
        public bool IsReadOnly => false;

        public void Add(TOutModel item)
        {
            _innerlist.Add(ToInnerModel(item));
        }

        public void Clear()
        {
            _innerlist.Clear();
        }

        public bool Contains(TOutModel item)
        {
            return _innerlist.Contains(ToInnerModel(item));
        }

        public void CopyTo(TOutModel[] array, int arrayIndex)
        {
            var innerArray = new TInnerModel[array.Length];

            _innerlist.CopyTo(innerArray, arrayIndex);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = ToOutModel(innerArray[i]);
            }
        }

        public IEnumerator<TOutModel> GetEnumerator()
        {
            return new DelegateEnumerator<TOutModel, TInnerModel>(_innerlist.GetEnumerator());
        }

        public int IndexOf(TOutModel item)
        {
            var innerModel = ToInnerModel(item);

            return _innerlist.IndexOf(innerModel);
        }

        public void Insert(int index, TOutModel item)
        {
            var innerModel = ToInnerModel(item);

            _innerlist.Insert(index, innerModel);
        }

        public bool Remove(TOutModel item)
        {
            var innerModel = ToInnerModel(item);

            return _innerlist.Remove(innerModel);
        }

        public void RemoveAt(int index)
        {
            _innerlist.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion Implement IList<>

        #region Implement enumerator
        private class DelegateEnumerator<TModel, TOtherModel> : IEnumerator<TModel>
            where TModel : class, new()
        {
            private IEnumerator<TOtherModel> _enumerator;
            public TModel Current
            {
                get
                {
                    var result = new TModel();

                    if (_enumerator.Current != null)
                    {
                        result.CopyFrom(_enumerator.Current);
                    }
                    return result;
                }
            }
            object IEnumerator.Current => Current;

            public DelegateEnumerator(IEnumerator<TOtherModel> enumerator)
            {
                _enumerator = enumerator;
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
        }
        #endregion Implement enumerator

        #region Helper
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