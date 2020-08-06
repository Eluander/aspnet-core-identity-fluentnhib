using System;
using System.Collections.Generic;
using System.Text;

namespace Eluander.Domain.Identity.Commands
{
    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
    }
}
