using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Notifications.Interfaces
{
    public interface IObserver<T>
    {
        Task Update(T data);
    }
}
