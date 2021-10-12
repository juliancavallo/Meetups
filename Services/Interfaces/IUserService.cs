using Domain.Filters;
using Domain.Models;
using Domain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IUserService
    {
        List<User> Get(UserSearchFilter filter);

        int Create(UserRequest request);

        void Delete(int id);

        void Update(UserRequest request);
    }
}
