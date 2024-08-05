using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decibels.DataAccess.Data
{
    public interface IDbInitializer 
    {
        // responsible for creating Admin and user Roles
        void Initialize();
    }
}
