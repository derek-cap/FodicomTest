namespace MenuCard.DataModel
{
    public class MenuItem : BindableBase
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                SetProperty(ref _text, value);
                SetDirty();
            }
        }

        private void SetDirty()
        {
            MenuCard?.SetDirty();
        }

        private double _price;
        public double Price
        {
            get { return _price; }
            set
            {
                SetProperty(ref _price, value);
                SetDirty();
            }
        }

        public MenuCard MenuCard { get; set; }
    }
}