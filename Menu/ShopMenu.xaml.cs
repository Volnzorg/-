using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Menu
{
    /// <summary>
    /// Логика взаимодействия для ShopMenu.xaml
    /// </summary>
    public partial class ShopMenu : Window
    {
        bool IsShotGunBuyed = false, IsRifleBuyed = false, IsPlazmaGunBuyed = false;
        int bomb = 5; // Сделать ограниченоо число бомб
        Player player;
        NPC trader;
        DispatcherTimer UpdaterTimer;

        public ShopMenu(Player player, NPC trader)
        {
            this.trader = trader;
            UpdaterTimer = new DispatcherTimer();
            UpdaterTimer.Interval = TimeSpan.FromSeconds(1);
            UpdaterTimer.Tick += Updater;
            InitializeComponent();
            this.player = player;
            //WindowShop.Width *= player.Scaling;
            //WindowShop.Height *= player.Scaling;
            foreach (Image x in grid.Children.OfType<Image>())
            {
                x.Width *= player.Scaling;
                x.Height *= player.Scaling;
            }

            Update();
        }

        private void Updater(object sender, EventArgs e)
        {
            if (IsShotGunBuyed)
            {
                ShotgunButton.Content = "Куплено";
            }
            else if (player.weapon.name == "ShotGun")
            {
                ShotgunButton.Content = "Используется";
            }
            else
            {
                ShotgunButton.Content = "15 Coins";
            }

            if (IsRifleBuyed)
            {
                RifleButton.Content = "Куплено";
            }
            else if (player.weapon.name == "Rifle")
            {
                RifleButton.Content = "Используется";
            }
            else
            {
                RifleButton.Content = "30 Coins";
            }

            if (IsPlazmaGunBuyed)
            {
                PlazmaGunButton.Content = "Куплено";
            }
            else if (player.weapon.name == "PlazmaGun")
            {
                PlazmaGunButton.Content = "Используется";
            }
            else
            {
                PlazmaGunButton.Content = "40 Coins";
            }

            if (bomb == 0)
            {
                Bomb_Button.Content = "Закончились";
            }
            else
            {
                Bomb_Button.Content = "3 Coins";

            }
            UpdaterTimer.Stop();
        }

        private void Update()
        {
            if (IsShotGunBuyed)
            {
                ShotgunButton.Content = "Куплено";
            }
            else if (player.weapon.name == "ShotGun" && player.weapon != null)
            {
                ShotgunButton.Content = "Используется";
            }
            else
            {
                ShotgunButton.Content = "15 Coins";
            }

            if (IsRifleBuyed)
            {
                RifleButton.Content = "Куплено";
            }
            else if (player.weapon.name == "Rifle" && player.weapon != null)
            {
                RifleButton.Content = "Используется";
            }
            else
            {
                RifleButton.Content = "30 Coins";
            }

            if (IsPlazmaGunBuyed)
            {
                PlazmaGunButton.Content = "Куплено";
            }
            else if (player.weapon.name == "PlazmaGun")
            {
                PlazmaGunButton.Content = "Используется";
            }
            else
            {
                PlazmaGunButton.Content = "40 Coins";
            }
            
            if (bomb == 0)
            {
                PlazmaGunButton.Content = "Закончились";
            }
        }

        private void ShotgunButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.weapon.name != "ShotGun")
            {
                if (player.Coins > 15)
                {
                    player.Coins -= 15;
                    player.weapon = new Weapons("ShotGun", player);
                    ShotgunButton.Content = "Куплено";
                    IsShotGunBuyed = true; IsRifleBuyed = false; IsPlazmaGunBuyed = false;
                }
                else
                {
                    ShotgunButton.Content = "Не хватает";
                }
            }

            UpdaterTimer.Start();
        }

        private void RifleButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.weapon.name != "Rifle")
            {
                if (player.Coins > 30)
                {
                    player.Coins -= 30;
                    player.weapon = new Weapons("Rifle", player);
                    RifleButton.Content = "Куплено";
                    IsShotGunBuyed = false; IsRifleBuyed = true; IsPlazmaGunBuyed = false;
                }
                else
                {
                    RifleButton.Content = "Не хватает";
                }
            }
            UpdaterTimer.Start();
        }

        private void PlazmaGunButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.weapon.name != "PlazmaGun")
            {
                if (player.Coins > 40)
                {
                    player.weapon = new Weapons("PlazmaGun", player);
                    PlazmaGunButton.Content = "Куплено";
                    IsShotGunBuyed = false; IsRifleBuyed = false; IsPlazmaGunBuyed = true;
                }
                else
                {
                    PlazmaGunButton.Content = "Не хватает";
                }
            }
            UpdaterTimer.Start();
        }

        private void Bomb_Button_Click(object sender, RoutedEventArgs e)
        {
            if (bomb > 0)
            {
                if (player.Coins > 3)
                {
                    player.Coins -= 3;
                    player.Bombs += 1;
                    bomb -= 1; 
                    if(bomb == 0)
                    {
                        Bomb_Button.Content = "Закончились";
                    }
                    Bomb_Button.Content = "Куплено";
                }
                else
                {
                    Bomb_Button.Content = "Не хватает";
                }
            }
            else
            {
                Bomb_Button.Content = "Закончились";
            }
            UpdaterTimer.Start();
        }

        private void NextButtton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            trader.IsShopOpen = false;
            
            this.Close();
        }
    }
}
