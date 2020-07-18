using System;

namespace Eluander.Shared.Core
{
    public class BaseEntity
    {
        public virtual long Id { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual bool IsInativo { get; set; }

        public BaseEntity()
        {
            IsInativo = false;
            DataCadastro = DateTime.Now;
        }

        public virtual bool IsNovo()
        {
            return Id == 0;
        }

    }
}
