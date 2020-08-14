using System;
using System.Linq;
using Ziglearning.ProductDatabase;

namespace Ziglearning.Repository
{
    public interface IClassRepository
    {
        ClassModel[] Classes { get; }
        ClassModel Clas(int classId);
        
        ClassModel[] ForUser(int userId);

        bool AddUserToClass(User user, int classId);
    }

    public class ClassModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class ClassRepository : IClassRepository
    {
        public ClassModel[] Classes
        {
            get
            {
                return DatabaseAccessor.Instance.Classes
                                               .Select(t => new ClassModel { Id = t.ClassId, Name = t.ClassName, Description = t.ClassDescription, Price = t.ClassPrice })
                                               .ToArray();
            }
        }

        public ClassModel Clas(int classId)
        {
            var clas = DatabaseAccessor.Instance.Classes
                                                   .Where(t => t.ClassId == classId)
                                                   .Select(t => new ClassModel { Id = t.ClassId, Name = t.ClassName, Description = t.ClassDescription, Price = t.ClassPrice })
                                                   .First();
            return clas;
        }


        public ClassModel[] ForUser(int userId)
        {
            return DatabaseAccessor.Instance.Users.First(t => t.UserId == userId)
                                  .Classes.Select(t =>
                                        new ClassModel
                                        {
                                            Id = t.ClassId,
                                            Name = t.ClassName,
                                            Description = t.ClassDescription,
                                            Price = t.ClassPrice
                                        })
            .ToArray();
        }

        public bool AddUserToClass(User user, int classId)
        {
            try
            { var userClass = DatabaseAccessor.Instance.Classes.Find(classId);
                userClass.Users.Add(user);
                DatabaseAccessor.Instance.SaveChanges();
                
                return true;
            }

            catch (Exception) 
            {
                return false;
            }


        }





    }
}
