using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace MenuCard.DataModel
{
    public class MenuCard : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
                SetDirty();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
                SetDirty();
            }
        }

        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set
            {
                SetProperty(ref _image, value);
                SetDirty();
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        public void SetDirty()
        {
            IsDirty = true;
        }

        public void ClearDirty()
        {
            IsDirty = false;
        }

        public bool IsDirty { get; private set; }

        private readonly ICollection<MenuItem> _menuItems = new ObservableCollection<MenuItem>();
        public ICollection<MenuItem> MenuItems => _menuItems;

        public void RestoreReferences()
        {
            foreach (var item in MenuItems)
            {
                item.MenuCard = this;
            }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
