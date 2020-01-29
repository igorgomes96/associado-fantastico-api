using AssociadoFantastico.Domain.Exceptions;

namespace AssociadoFantastico.Application.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException() : base("Usuário sem permissão de acesso!") { }
        public UnauthorizedException(string message) : base(message) { }
    }
}
