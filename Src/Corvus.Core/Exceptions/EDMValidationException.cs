using System;
using System.Collections.Generic;
using System.Text;

namespace Corvus.Core.Exceptions
{
    public class EDMValidationException : Exception
    {
        public EDMValidationException(string msg) : base(msg)
        {

        }
    }
}
