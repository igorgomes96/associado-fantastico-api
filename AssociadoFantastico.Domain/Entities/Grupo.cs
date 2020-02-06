using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AssociadoFantastico.Domain.Entities
{
    public class Grupo: Entity
    {
        public Grupo() { } // EF
        public Grupo(string nome): base()
        {
            Nome = nome;
        }

        public string Nome { get; set; }
        public Guid CicloId { get; private set; }

        public virtual Ciclo Ciclo { get; private set; }        
        private readonly List<Associado> _associados = new List<Associado>();
        public virtual IReadOnlyCollection<Associado> Associados => new ReadOnlyCollection<Associado>(_associados);

    }
}
