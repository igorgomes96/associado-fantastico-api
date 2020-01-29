using AssociadoFantastico.Domain.Exceptions;

namespace AssociadoFantastico.Application.Exceptions
{
    public class DuplicatedException : CustomException
    {
        public DuplicatedException() : base("Registro duplicado!") { }

        public DuplicatedException(string message) : base(message) { }
    }
}
