using Meetups.Domain.Filters;
using Meetups.Domain.Models.Entities;
using Meetups.Domain.Models.Requests;
using Meetups.Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meetups.Domain.Interfaces.Services
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
