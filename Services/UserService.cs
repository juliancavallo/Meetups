using Domain.Filters;
using Domain.Models;
using Domain.Requests;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<User> Get(UserSearchFilter filter)
        {
            var q = context.User.Where(x => true);

            if (!string.IsNullOrWhiteSpace(filter.Name))
                q = q.Where(x => x.FirstName.Contains(filter.Name));

            if (!string.IsNullOrWhiteSpace(filter.LastName))
                q = q.Where(x => x.LastName.Contains(filter.LastName));

            if (!string.IsNullOrWhiteSpace(filter.UserName))
                q = q.Where(x => x.UserName.Contains(filter.UserName));

            return q.ToList();
        }

        public int Create(UserRequest request)
        {
            try
            {
                var user = new User();
                user.FirstName = request.Name;
                user.LastName = request.LastName;
                user.UserName = request.UserName;
                user.Password = request.Password;

                context.Add(user);
                context.SaveChanges();

                return user.Id;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var user = context.User.FirstOrDefault(x => x.Id == id);

                if (user == null)
                    throw new Exception("Not found");

                context.User.Remove(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(UserRequest request)
        {
            try
            {
                var user = context.User.First(x => x.Id == request.Id);
                user.FirstName = request.Name;
                user.LastName = request.LastName;
                user.UserName = request.UserName;
                user.Password = request.Password;

                context.Update(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }   
        
        public User ValidateLogin(LoginRequest loginDetails)
        {
            return context.User.FirstOrDefault(x => x.UserName == loginDetails.User && x.Password == loginDetails.Password);
        }
    }
}
