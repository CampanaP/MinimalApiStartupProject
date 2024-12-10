namespace MinimalApiStartupProject.Modules.Email.Settings
{
    internal class EmailSetting
    {
        public string? Host { get; set; }

        public string? Password { get; set; }

        public string? SenderAddress { get; set; }

        public string? SenderDisplayName { get; set; }

        public string? Username { get; set; }
    }
}