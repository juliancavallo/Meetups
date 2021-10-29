using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Filters
{
    public class UserSearchFilter
    {
        public UserSearchFilter(string userName = null, string lastName = null, string firstName = null)
        {
            this.Name = userName;
        }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }
}
