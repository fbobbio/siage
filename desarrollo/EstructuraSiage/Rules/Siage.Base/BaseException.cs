using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Siage.Base
{
    public class BaseException : SystemException
    {
        public BaseException()
            : base()
        { }

        public BaseException(string mensaje)
            : base(mensaje)
        { }
    }
}
