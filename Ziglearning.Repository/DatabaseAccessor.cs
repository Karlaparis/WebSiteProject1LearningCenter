using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ziglearning.ProductDatabase;

namespace Ziglearning.Repository
{
    public class DatabaseAccessor
    {
        private static readonly LearningDbEntities entities;

        static DatabaseAccessor()
        {
            entities = new LearningDbEntities();
            entities.Database.Connection.Open();
        }

        public static LearningDbEntities Instance
        {
            get
            {
                return entities;
            }
        }
    }
}
