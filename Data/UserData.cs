using BCrypt.Net;
using SampleSecureWeb.Models;
using BC = BCrypt.Net.BCrypt;

namespace SampleSecureWeb.Data
{
    public class UserData : IUser
    {
        private readonly ApplicationDbContext _db;

        public UserData(ApplicationDbContext db)
        {
            _db = db;
        }

        // Implementasi Login
        public User Login(User user)
        {
            var _user = _db.Users.FirstOrDefault(u => u.Username == user.Username);
            if (_user == null)
            {
                throw new Exception("User Not Found");
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, _user.Password))
            {
                throw new Exception("Password is Incorrect");
            }

            return _user;
        }

        // Implementasi Registrasi
        public User Registration(User user)
        {
            try
            {
                user.Password = BC.HashPassword(user.Password);
                _db.Users.Add(user);
                _db.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Implementasi ChangePassword
        public void ChangePassword(string username, string newPassword)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Password = BC.HashPassword(newPassword);
            _db.SaveChanges();
        }
    }
}
