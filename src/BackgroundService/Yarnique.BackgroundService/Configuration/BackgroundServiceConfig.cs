namespace Yarnique.BackgroundService.Configuration
{
    public class BackgroundServiceConfig
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public EmailConfiguration EmailConfiguration { get; set; }
    }

    public class ConnectionStrings
    {
        public string YarniqueConnectionString { get; set; }
    }

    public class EmailConfiguration
    {
        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string AppPassword { get; set; }
    }
}
