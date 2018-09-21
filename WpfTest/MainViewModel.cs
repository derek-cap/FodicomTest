using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest
{
    class MainViewModel : BindableBase
    {
        public ObservableCollection<string> _pacsNodes = new ObservableCollection<string>();
        public IEnumerable<string> PacsNodes => _pacsNodes;

        private string _selectedPacsNode;
        public string SelectedPacsNode
        {
            get { return _selectedPacsNode; }
            set { SetProperty(ref _selectedPacsNode, value); }
        }

        public MainViewModel()
        {
            _pacsNodes.Add("A");
            _pacsNodes.Add("B");
            _pacsNodes.Add("C");
            _pacsNodes.Add("D");

            _selectedPacsNode = PacsNodes.FirstOrDefault();
        }


    }
}
