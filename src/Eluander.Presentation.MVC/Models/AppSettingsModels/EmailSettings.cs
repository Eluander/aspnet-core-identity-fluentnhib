namespace Eluander.Presentation.MVC.Models.AppSettingsModels
{
    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }
        public int PrimaryPort { get; set; }
        public string From { get; set; }
        public string FromUserName { get; set; }
        public string PasswordUsername { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
    }
}
