using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Notifications.Interfaces
{
    public interface IGenericObserver<T>
    {
        void Update(T data);
    }
}
