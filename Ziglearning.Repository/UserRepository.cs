using System;
using System.CodeDom;
using System.Linq;
using System.Web.Mvc;
using Ziglearning.ProductDatabase;

namespace Ziglearning.Repository
{
    public interface IUserRepository
    {
        UserModel LogIn(string email, string password);
        UserModel Register(string email, string password);

        UserModel[] Users { get; }
        UserModel User(int userId);

        User GetUserFromDb(int userId);

        UserModel AlreadyRegistered(string email, string password);
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRepository : IUserRepository
    {
        public UserModel LogIn(string email, string password)
        {
            var user = DatabaseAccessor.Instance.Users
                .FirstOrDefault(t => t.UserEmail.ToLower() == email.ToLower()
                                      && t.UserPassword == password);

            if (user == null)
            {
                return null;
            }

            return new UserModel { Id = user.UserId, Name = user.UserEmail };



        }

        public UserModel Register(string email, string password)
        {
            var user = DatabaseAccessor.Instance.Users
                    .Add(new Ziglearning.ProductDatabase.User
                    {
                        UserEmail = email,
                        UserPassword = password
                    });

            DatabaseAccessor.Instance.SaveChanges();

            return new UserModel { Id = user.UserId, Name = user.UserEmail };
        }


        public UserModel AlreadyRegistered(string email, string password)

        {
     
            
                var user = DatabaseAccessor.Instance.Users
                                               .Where(t => t.UserEmail == email)
                                               .FirstOrDefault();

                if (user == null)
                {
                    return null;
                }



                return new UserModel { Id = user.UserId, Name = user.UserEmail };
           

            }


        public UserModel[] Users
        {
            get
            {
                return DatabaseAccessor.Instance.Users
                                               .Select(t => new UserModel { Id = t.UserId, Name = t.UserEmail })
                                               .ToArray();
            }
        }

        public UserModel User(int userId)

        {
            
            
            var user = DatabaseAccessor.Instance.Users
                                                   .Where(t => t.UserId == userId)
                                                   .Select(t => new UserModel { Id = t.UserId, Name = t.UserEmail })
                                                   .First();
            return user;
        }

        public User GetUserFromDb(int userId)
        {
            var user = DatabaseAccessor.Instance.Users
                                                    .Where(t => t.UserId == userId)
                                                    .FirstOrDefault();

            return user;
        }

    }
}
