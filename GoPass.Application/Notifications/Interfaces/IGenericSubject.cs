using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Notifications.Interfaces
{
    public interface IGenericSubject<T>
    {
        void Attach(IGenericObserver<T> observer);
        void Detach(IGenericObserver<T> observer);
        void Notify(T data);
    }
}
