namespace AssociadoFantastico.Domain.Entities
{
    public class Empresa: Entity
    {
        public Empresa() { } // EF

        public Empresa(string nome): base()
        {
            Nome = nome;
        }

        public string Nome { get; private set; }
    }
}
