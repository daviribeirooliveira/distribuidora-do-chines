// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public interface IEmailConfiguration
    {
        string SmtpServer { get; set; }
        int SmtpPort { get; set; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }
        bool UseSsl { get; set; }

        // string PopServer { get; }
        // int PopPort { get; }
        // string PopUsername { get; }
        // string PopPassword { get; }
    }

    public class EmailConfiguration : IEmailConfiguration
    {
        public string PopServer { get; set; }
        public int PopPort { get; set; }
        public string PopUsername { get; set; }
        public string PopPassword { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public bool UseSsl { get; set; }
    }
}