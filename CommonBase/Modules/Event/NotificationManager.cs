//@CodeCopy
//MdStart
namespace CommonBase.Modules.Event
{
    public partial class NotificationManager
    {
        private readonly Dictionary<string, EventHandler> _observers = new();

        public void AddEventHandler(string key, EventHandler handler)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (_observers.ContainsKey(key))
            {
                if (_observers[key].GetInvocationList().Contains(handler) == false)
                {
                    _observers[key] += handler;
                }
            }
            else
            {
                _observers.Add(key, handler);
            }
        }
        public void RemoveEventHandler(string key, EventHandler handler)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (_observers.ContainsKey(key))
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                _observers[key] -= handler;
#pragma warning restore CS8601 // Possible null reference assignment.
            }
        }
        public void Notify(string key, object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (_observers.ContainsKey(key))
            {
                _observers[key]?.Invoke(sender, e);
            }
        }
    }
}
//MdEnd
