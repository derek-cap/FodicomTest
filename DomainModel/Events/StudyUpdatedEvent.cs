using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Events
{
    class StudyUpdatedEvent
    {
        public DicomStudy Study { get; set; }
    }
}
