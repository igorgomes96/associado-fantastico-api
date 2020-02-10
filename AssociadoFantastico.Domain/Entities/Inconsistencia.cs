using System;

namespace AssociadoFantastico.Domain.Entities
{
    public class Inconsistencia: Entity
    {
        public Inconsistencia(string coluna, int linha, string mensagem)
        {
            Coluna = coluna;
            Linha = linha;
            Mensagem = mensagem;
        }

        public string Coluna { get; private set; }
        public int Linha { get; private set; }
        public string Mensagem { get; private set; }
        public Guid ImportacaoId { get; private set; }

        public virtual Importacao Importacao { get; private set; }
    }
}
