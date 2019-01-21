using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Events
{
    class StudyCreatedEventHandler
    {
        private readonly IStudyRepository _repo;

        public StudyCreatedEventHandler(IStudyRepository repo)
        {
            _repo = repo;
        }

        public void Handle(StudyCreatedEvent @event)
        {
            _repo.Add(@event.Study).Wait();
        }
    }
}
