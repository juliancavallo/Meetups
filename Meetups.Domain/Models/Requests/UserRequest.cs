namespace Meetups.Domain.Models.Requests
{
    public class UserRequest
    {
        public UserRequest() { }
        public UserRequest(string name, string lastName, string userName, string password, int id = 0)
        {
            this.Name = name;
            this.LastName = lastName;
            this.UserName = userName;
            this.Password = password;
            this.Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
