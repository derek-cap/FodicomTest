using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomViewer.Models
{
    public class InteractorFactory
    {
        private List<Interactor> _interactors = new List<Interactor>();

        public InteractorFactory()
        {
            _interactors.Add(new Interactor() { ToolTip = "Dot", IsChecked = false, ImagePath = "/Resources/Pngs/dot.png" });
            _interactors.Add(new Interactor() { ToolTip = "Line", IsChecked = false, ImagePath = "/Resources/Pngs/Line.png" });
            _interactors.Add(new Interactor() { ToolTip = "Rectangle", IsChecked = false, ImagePath = "/Resources/Pngs/Rect.png" });
            _interactors.Add(new Interactor() { ToolTip = "Circle", IsChecked = false, ImagePath = "/Resources/Pngs/Circle.png" });
        }

        public IEnumerable<Interactor> GetInteractors()
        {
            return _interactors;
        }
    }
}
