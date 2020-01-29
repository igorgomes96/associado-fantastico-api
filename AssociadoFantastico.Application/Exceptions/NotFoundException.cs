using AssociadoFantastico.Domain.Exceptions;

namespace AssociadoFantastico.Application.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException() : base("Registro não encontrado!") { }
        public NotFoundException(string message) : base(message) { }
    }
}
