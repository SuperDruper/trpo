using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BsuirHealthProjectServer.Shared
{
    public class BHPException : Exception
    {
        public BHPException(string message)
            : base(message)
        {
        }

        public BHPException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}