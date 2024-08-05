using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreSIMPLECRM.Services
{
    public class CorrectInputService
    {
        public bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@(gmail\.com|mail\.ru|yandex\.by)$";
            return Regex.IsMatch(email, pattern);
        }

        public bool ValidatePassword(string password)
        {
            return password.Length >= 8 && Regex.IsMatch(password, @"[a-z]") && Regex.IsMatch(password, @"[A-Z]");
        }
    }
}
