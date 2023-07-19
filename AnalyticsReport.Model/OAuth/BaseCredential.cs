using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsReport.Model
{
    public class BaseCredential
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public BaseCredential()
        {
            Username = Password = string.Empty;
        }

        public BaseCredential(string? username, string? password) : this()
        {
            Username = username;
            Password = password;
        }
    }
}
