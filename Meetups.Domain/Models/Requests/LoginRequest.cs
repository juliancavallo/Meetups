using System;
using System.Collections.Generic;
using System.Text;

namespace Meetups.Domain.Models.Requests
{
    public class LoginRequest
    {
        public string User { get; set; }

        public string Password { get; set; }
    }
}
