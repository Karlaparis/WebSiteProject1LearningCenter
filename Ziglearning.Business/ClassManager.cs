using System.Linq;
using Ziglearning.Repository;


namespace Ziglearning.Business
{
    public interface IClassManager
    {
        ClassModel[] Classes { get; }
        ClassModel Clas(int classId);
        ClassModel[] ForUser(int userId);
        bool AddUserToClass(int userId, int classId);

    }

    public class ClassModel
    {
      
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public ClassModel(int id, string name, string description, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }


    }

    public class ClassManager : IClassManager
    {
        private readonly IClassRepository classRepository;
        private static IUserManager userManager = new UserManager(new UserRepository());
        public ClassManager(IClassRepository classRepository)
        {
            this.classRepository = classRepository;
        }

        public ClassModel[] Classes
        {
            get
            {
                return classRepository.Classes
                                         .Select(t => new ClassModel(t.Id, t.Name, t.Description, t.Price))
                                         .ToArray();
            }
        }

        public ClassModel Clas(int classId)
        {
            var classModel = classRepository.Clas(classId);
            return new ClassModel(classModel.Id, classModel.Name, classModel.Description, classModel.Price);
        }


        public ClassModel[] ForUser(int userId)
        {
            return classRepository.ForUser(userId).Select(t =>
                            new ClassModel(t.Id, t.Name, t.Description, t.Price)
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Description = t.Description,
                                Price = t.Price
                            })
                            .ToArray();
        }


 

        public bool AddUserToClass(int userId, int classId)
        {
            var user = userManager.GetUserFromRepository(userId);
            return classRepository.AddUserToClass(user, classId);
        }
    }
}
