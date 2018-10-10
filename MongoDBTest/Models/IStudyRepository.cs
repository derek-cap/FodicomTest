using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTest.Models
{
    interface IStudyRepository
    {
        Task AddOrUpdateAsync(StudyRecord study);
    }
}
