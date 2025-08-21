using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace Menu
{

    public class NPC
    {
        public bool IsShopOpen = false;

        public Image NPC_Trader = new Image
        {
            Tag = "NPC",
            Name = "Trader",
            Width = 100,
            Height = 150,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(500, 500, 0, 0),
            Stretch = Stretch.Fill,
            Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/NPC/NPC.png", UriKind.Absolute)),
        };

        public Rect Trader_Intersaction;

        Label DialogueBox = new Label
        {

        };

        Player player;

        public ShopMenu menu;

        public NPC(Player player) 
        {
            this.player = player;
            NPC_Trader.Width *= player.Scaling;
            NPC_Trader.Height *= player.Scaling;
            NPC_Trader.Margin = new Thickness(NPC_Trader.Margin.Left * player.Scaling, NPC_Trader.Margin.Top * player.Scaling, 0, 0);
            Trader_Intersaction = new Rect(NPC_Trader.Margin.Left - player.PlayerModel.Width, NPC_Trader.Margin.Top - player.PlayerModel.Height, player.PlayerModel.Width * 3, player.PlayerModel.Height * 3);
        }

        public void Shop_Menu()
        {
            menu = new ShopMenu(player, this);
            menu.Width *= player.Scaling;
            menu.Height *= player.Scaling;
            menu.Margin = new Thickness(NPC_Trader.Margin.Left + NPC_Trader.Width, NPC_Trader.Margin.Top, 0, 0);
            menu.ShowDialog();
            IsShopOpen = true;
        }

        public void Close_Menu()
        {
            IsShopOpen = false;
            menu.Close();
        }

        public void FirstDialogue()
        {

        }

        public void Dialogue()
        {
            if (player.Level == 4)
            {

            }
        }
    }
}
