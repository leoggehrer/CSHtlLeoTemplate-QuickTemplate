//@BaseCode
//MdStart
namespace CommonBase.Modules.Event
{
    /// <summary>
    /// Manages event handlers and notifications.
    /// </summary>
    public partial class NotificationManager
    {
        private readonly Dictionary<string, EventHandler> _observers = new();

        /// <summary>
        /// Adds an event handler to the dictionary of observers.
        /// </summary>
        /// <param name="key">The key used to identify the event handler.</param>
        /// <param name="handler">The event handler to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when either key or handler is null.</exception>
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
        /// Removes an event handler from the observers dictionary with the specified key.
        /// @param key The key used to identify the event handler collection in the observers dictionary.
        /// @param handler The event handler to be removed.
        /// @throws ArgumentNullException If the key or handler is null or empty.
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
        /// <summary>
        /// Notifies the specified key with the sender and event arguments.
        /// </summary>
        /// <param name="key">The key to notify.</param>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        /// <exception cref="ArgumentNullException">Thrown if the key is null or empty.</exception>
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


