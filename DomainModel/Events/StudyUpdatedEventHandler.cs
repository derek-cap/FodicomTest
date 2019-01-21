using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Events
{
    class StudyUpdatedEventHandler
    {
        private readonly IStudyRepository _repo;

        public StudyUpdatedEventHandler(IStudyRepository repo)
        {
            _repo = repo;
        }

        public void Handle(StudyUpdatedEvent @event)
        {
            _repo.Update(@event.Study).Wait();
        }
    }
}
