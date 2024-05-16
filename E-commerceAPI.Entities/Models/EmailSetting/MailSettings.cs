using System;
using System.Linq;

namespace E_commerceAPI.Services.Repositories.Services.EmailSetting
{
    public class MailSettings
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

    }
}
