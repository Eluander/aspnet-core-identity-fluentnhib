using System;

namespace Eluander.Presentation.MVC.Models
{
    public class EmailSettings
    {
        public String PrimaryDomain { get; set; }
        public int PrimaryPort { get; set; }
        public String From { get; set; }
        public String FromUserName { get; set; }
        public String PasswordUsername { get; set; }
        public String ToEmail { get; set; }
        public String CcEmail { get; set; }
    }
}
