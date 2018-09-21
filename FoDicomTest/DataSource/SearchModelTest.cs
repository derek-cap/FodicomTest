using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest.DataSource
{
    class SearchModelTest
    {
        public async Task Test()
        {
            
            LocalStudySearchModel seachModel = new LocalStudySearchModel();

            List<StudyViewHelper> studies = new List<StudyViewHelper>();
            seachModel.Studies.Subscribe(h =>
            {
                foreach (var item in typeof(StudyViewHelper).GetProperties())
                {
                    Console.WriteLine($"{item.Name} = {item.GetValue(h)}");
                }
                Console.WriteLine("------------------------");
                studies.Add(h);
            });
            await seachModel.SearchStudyAsync();

            // Series
            Console.WriteLine("-----------------Series:  ---------");
            string studyUID = studies.First().StudyInstanceUID;
            var series = seachModel.SearchSeriesOfStudy(studyUID);
            foreach (var item in series)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            await seachModel.SearchImageOfSeries(studyUID, series.FirstOrDefault());
        }
    }
}
