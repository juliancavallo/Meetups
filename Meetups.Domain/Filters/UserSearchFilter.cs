using System;
using System.Collections.Generic;
using System.Text;

namespace Meetups.Domain.Filters
{
    public class UserSearchFilter
    {
        public UserSearchFilter(string userName = null, string lastName = null, string firstName = null)
        {
            this.UserName = userName;
            this.LastName = lastName;
            this.FirstName = firstName;
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }
}
