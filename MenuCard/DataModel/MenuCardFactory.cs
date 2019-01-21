using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuCard.DataModel
{
    public class MenuCardFactory
    {
        private ICollection<MenuCard> _cards;
        public ICollection<MenuCard> Cards => _cards;

        public void InitMenuCards(IEnumerable<MenuCard> menuCards)
        {
            _cards = new ObservableCollection<MenuCard>(menuCards);
        }

        private static MenuCardFactory _instace = null;
        public static MenuCardFactory Instace
        {
            get
            {
                return _instace ?? (_instace = new MenuCardFactory());
            }
        }

        public static ObservableCollection<MenuCard> GetSampleMenuCards()
        {
            var cards = new ObservableCollection<MenuCard>();
            MenuCard card1 = new MenuCard { Title = "Breadfast" };
            card1.MenuItems.Add(new MenuItem
            {
                Text = "Bread",
                Price = 5.4,
                MenuCard = card1
            });
            card1.MenuItems.Add(new MenuItem
            {
                Text = "Milk",
                Price = 3.5,
                MenuCard = card1
            });
            card1.MenuItems.Add(new MenuItem
            {
                Text = "Egg",
                Price = 6.1,
                MenuCard = card1
            });
            cards.Add(card1);
            return cards;
        }
    }
}
