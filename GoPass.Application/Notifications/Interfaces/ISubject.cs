﻿namespace GoPass.Application.Notifications.Interfaces
{
    public interface ISubject<T>
    {
        void Attach(IObserver<T> observer);
        void Detach(IObserver<T> observer);
        Task Notify(T data);
    }
}
