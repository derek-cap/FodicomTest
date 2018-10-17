using DomainModel.Infrastructure;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomServerTest
{
    class ServiceLocator
    {
        public static IStudyRepository StudyRepository => new MongoStudyRepository();

        public static IDicomFileRepository DicomFileRepository => new DicomFileRepository();

        public static IImporter Importer => new Importer(StudyRepository, DicomFileRepository);

        public static IDicomImageFinderService DicomImageFinderService => null;
    }
}
