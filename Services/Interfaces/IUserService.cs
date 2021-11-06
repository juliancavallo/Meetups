using Domain.Filters;
using Domain.Models;
using Domain.Requests;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IUserService
    {
        UsersSearchResponse Get(UserSearchFilter filter);

        int Create(UserRequest request);

        void Delete(int id);

        void Update(UserRequest request);

        User ValidateLogin(LoginRequest loginDetails);
    }
}
