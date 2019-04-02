using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortMvvm
{
    public static class Extantions
    {

        public static T To<T>(this object dataContext)
        {
            return (T)dataContext;
        }

    }
}
