using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoDicomTest.DataSource
{
    interface IStudySearchModel
    {
        IObservable<StudyViewHelper> Studies { get; }
        Task SearchStudyAsync();
    }
}
