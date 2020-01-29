using System;

namespace AssociadoFantastico.Domain.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message) { }
    }
}
