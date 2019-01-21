using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class NotFoundStudy : DicomStudy
    {
        public static NotFoundStudy Instance = new NotFoundStudy();

        public NotFoundStudy()
            : base()
        { }
    }
}
