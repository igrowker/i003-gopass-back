using GoPass.Application.Notifications.Interfaces;

namespace GoPass.Application.Notifications.Classes
{
    public class GenericSubject<T> : IGenericSubject<T>
    {
        private readonly List<IGenericObserver<T>> _observers = new List<IGenericObserver<T>>();

        public void Attach(IGenericObserver<T> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IGenericObserver<T> observer)
        {
            if(_observers.Contains(observer)) 
            {
                _observers.Remove(observer);
            }
        }

        public void Notify(T data)
        {
            foreach (var observer in _observers)
            {
                observer.Update(data);
            }
        }
    }
}
