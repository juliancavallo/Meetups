using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Requests
{
    public class LoginRequest
    {
        public string User { get; set; }

        public string Password { get; set; }
    }
}
