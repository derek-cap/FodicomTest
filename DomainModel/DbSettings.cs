using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class DbSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }

        public static DbSettings StudySettings
        {
            get
            {
                return new DbSettings()
                {
                    ConnectionString = "mongodb://derek:123456@127.0.0.1:27017/imagedb",
                    Database = "imagedb"
                };
            }
        }
    }
}
