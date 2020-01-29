using System;

namespace AssociadoFantastico.WebApi.ViewModels
{
    public class AuthInfoViewModel
    {
        public DateTime Criacao { get; set; }
        public DateTime Expiracao { get; set; }
        public string CPF { get; set; }
        public string Matricula { get; set; }
        public string[] Roles { get; set; }
        public string AccessToken { get; set; }
    }
}
