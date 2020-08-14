using Ziglearning.Repository;
using System.Linq;
using Ziglearning.ProductDatabase;

namespace Ziglearning.Business
{
    public interface IUserManager
    {
        UserModel LogIn(string email, string password);
        UserModel Register(string email, string password);
        UserModel[] Users { get; }
        UserModel User(int userId);
        User GetUserFromRepository(int userId);
        UserModel AlreadyRegistered(string email, string password);

    }

    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserModel(int id, string name)
        {
            Id = id;
            Name = name;
        }


    }

    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;

        public UserManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public UserModel LogIn(string email, string password)
        {
            var user = userRepository.LogIn(email, password);

            if (user == null)
            {
                return null;
            }

            return new UserModel (user.Id, user.Name) { Id = user.Id, Name = user.Name };
        }

        public UserModel Register(string email, string password)
        {
            var user = userRepository.Register(email, password);

            if (user == null)
            {
                return null;
            }

           
            return new UserModel(user.Id, user.Name) { Id = user.Id, Name = user.Name };
        }


        public UserModel AlreadyRegistered(string email, string password)
        {
            var user = userRepository.AlreadyRegistered(email, password);

            if (user == null)
            {
                return null;
            }


            return new UserModel(user.Id, user.Name) { Id = user.Id, Name = user.Name };
        }



        public UserModel[] Users
        {
            get
            {
                return userRepository.Users
                                         .Select(t => new UserModel(t.Id, t.Name))
                                         .ToArray();
            }
        }

        public UserModel User(int userId)
        {
            var userModel = userRepository.User(userId);
            return new UserModel(userModel.Id, userModel.Name);
        }

        public User GetUserFromRepository(int userId)
        {
            var user = userRepository.GetUserFromDb(userId);
            return user;

        }

    }
}
