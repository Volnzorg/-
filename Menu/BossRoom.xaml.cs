using Menu;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Menu
{
    /// <summary>
    /// Логика взаимодействия для BossRoom.xaml
    /// </summary>
    public partial class BossRoom : Window
    {
        public Engine engine;
        Player player;
        LastBoss Boss;

        public BossRoom(Player player)
        {
            InitializeComponent();

            this.player = player;
            Boss = new LastBoss(player, grid, this);
            engine = new Engine(player, grid, this, Boss);

            DispatcherTimer EnemiesChecked = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            EnemiesChecked.Tick += EnemiesKilled;
            EnemiesChecked.Start();

            player.PlayerModelCreator();

            grid.Width *= player.Scaling;
            grid.Height *= player.Scaling;

            player.PlayerModel.Margin = new Thickness(grid.Width / 2, grid.Height / 2, 0, 0);

            grid.Children.Add(player.PlayerModel);

            grid.Children.Add(Boss.BossModel);
            BorderCreator();
        }

        public void EnemiesKilled(object sender, EventArgs e)
        {
            foreach (Enemy x in engine.EnemyList)
            {
                if (x.EnemyHP <= 0 && x.Killed == false)
                {
                    x.Killed = true;
                   //Image DropItem = cave.Items.EnemiesRandomDropping(x.EnemyModel.Margin.Left, x.EnemyModel.Margin.Top, x.EnemyType);
                   //if (DropItem != null)
                   //{
                   //    cave.grid.Children.Add(DropItem);
                   //}
                    engine.EnemyDel.Add(x);
                    x.Delete(x);

                }
            }
        }

        public void BorderCreator()
        {
            Rectangle LeftBorder = new Rectangle
            {
                Tag = "Border",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = player.PlayerModel.Width,
                Height = grid.Height,
                Margin = new Thickness(0, 0, 0, 0)

            };
            Rectangle RightBorder = new Rectangle
            {
                Tag = "Border",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = player.PlayerModel.Width / 3,
                Height = grid.Height,
                Margin = new Thickness(0 + grid.Width - player.PlayerModel.Width / 3, 0, 0, 0)
            };
            Rectangle UpBorder = new Rectangle
            {
                Tag = "Border",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = grid.Width,
                Height = Boss.BossModel.Height / 2,
                Margin = new Thickness(0, 0, 0, 0)
            };
            Rectangle DownBorder = new Rectangle
            {
                Tag = "Border",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = grid.Width,
                Height = player.PlayerModel.Height,
                Margin = new Thickness(0, grid.Height - Boss.BossModel.Height / 2, 0, 0)
            };
            grid.Children.Add(LeftBorder); grid.Children.Add(RightBorder); grid.Children.Add(UpBorder); grid.Children.Add(DownBorder);
        }


    }
}

//oreach (Bullets x in EnemyBulletsList)
//
//   if (x.BulletModel != null)
//   {
//       if (x.Direction == "Up")
//       {
//           x.BulletModel.Margin = new Thickness(x.BulletModel.Margin.Left, (double)(x.BulletModel.Margin.Top - x.enemy?.BulletSpeed ?? 0 - x.Boss?.ProjectileSpeed ?? 0), 0, 0);
//
//           if ((double)(x.CoordY - x.enemy?.AttackRange ?? 0 - x.Boss?.ProjectileRange ?? 0) > x.BulletModel.Margin.Top)
//           {
//               grid.Children.Remove(x.BulletModel);
//               x.BulletModel = null;
//           }
//       }
//
//       if (x.Direction == "Down")
//       {
//           x.BulletModel.Margin = new Thickness(x.BulletModel.Margin.Left, (double)(x.BulletModel.Margin.Top + x.enemy?.BulletSpeed ?? 0 + x.Boss?.ProjectileSpeed ?? 0), 0, 0);
//
//           if ((double)(x.CoordY + x.enemy?.AttackRange ?? 0 + x.Boss?.ProjectileRange ?? 0) < x.BulletModel.Margin.Top)
//           {
//               grid.Children.Remove(x.BulletModel);
//               x.BulletModel = null;
//           }
//       }
//       if (x.Direction == "Left")
//       {
//           x.BulletModel.Margin = new Thickness((double)(x.BulletModel.Margin.Left - x.enemy?.BulletSpeed ?? 0 - x.Boss?.ProjectileSpeed ?? 0), x.BulletModel.Margin.Top, 0, 0);
//
//           if ((double)(x.CoordX - x.enemy?.AttackRange ?? 0 - x.Boss?.ProjectileRange ?? 0) > x.BulletModel.Margin.Left)
//           {
//               grid.Children.Remove(x.BulletModel);
//               x.BulletModel = null;
//           }
//       }
//       if (x.Direction == "Right")
//       {
//           x.BulletModel.Margin = new Thickness((double)(x.BulletModel.Margin.Left + x.enemy?.BulletSpeed ?? 0 + x.Boss?.ProjectileSpeed ?? 0), x.BulletModel.Margin.Top, 0, 0);
//
//           if ((double)(x.CoordX + x.enemy?.AttackRange ?? 0 + x.Boss?.ProjectileRange ?? 0) < x.BulletModel.Margin.Left)
//           {
//               grid.Children.Remove(x.BulletModel);
//               x.BulletModel = null;
//           }
//       }
//
//       if (x.Direction == "UpRight")
//       {
//           x.BulletModel.Margin = new Thickness((double)(x.BulletModel.Margin.Left + x.enemy?.BulletSpeed / 2 ?? 0 + x.Boss?.ProjectileSpeed / 2 ?? 0), (double)(x.BulletModel.Margin.Top - x.enemy?.BulletSpeed / 2 ?? 0 - x.Boss?.ProjectileSpeed / 2 ?? 0), 0, 0);
//
//           if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.BulletModel.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.BulletModel.Margin.Left), 2)) >= (double)(x.Boss?.ProjectileRange ?? 0 + x.enemy?.AttackRange ?? 0))
//           {
//               grid.Children.Remove(x.BulletModel);
//               x.BulletModel = null;
//           }
//       }
//
//       if (x.Direction == "UpLeft")
//       {
//           x.BulletModel.Margin = new Thickness((double)(x.BulletModel.Margin.Left - x.enemy?.BulletSpeed / 2 ?? 0 - x.Boss?.ProjectileSpeed / 2 ?? 0), (double)(x.BulletModel.Margin.Top - x.enemy?.BulletSpeed / 2 ?? 0 - x.Boss?.ProjectileSpeed / 2 ?? 0), 0, 0);
//
//           if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.BulletModel.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.BulletModel.Margin.Left), 2)) >= (double)(x.Boss?.ProjectileRange ?? 0 + x.enemy?.AttackRange ?? 0))
//           {
//               grid.Children.Remove(x.BulletModel);
//               x.BulletModel = null;
//           }
//       }
//
//       if (x.Direction == "DownLeft")
//       {
//           x.BulletModel.Margin = new Thickness((double)(x.BulletModel.Margin.Left - x.enemy?.BulletSpeed / 2 ?? 0 - x.Boss?.ProjectileSpeed / 2 ?? 0), (double)(x.BulletModel.Margin.Top + x.enemy?.BulletSpeed / 2 ?? 0 + x.Boss?.ProjectileSpeed / 2 ?? 0), 0, 0);
//
//           if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.BulletModel.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.BulletModel.Margin.Left), 2)) >= (double)(x.Boss?.ProjectileRange ?? 0 + x.enemy?.AttackRange ?? 0))
//           {
//               grid.Children.Remove(x.BulletModel);
//               x.BulletModel = null;
//           }
//       }
//
//       if (x.Direction == "DownRight")
//       {
//           x.BulletModel.Margin = new Thickness((double)(x.BulletModel.Margin.Left + x.enemy?.BulletSpeed / 2 ?? 0 + x.Boss?.ProjectileSpeed / 2 ?? 0), (double)(x.BulletModel.Margin.Top + x.enemy?.BulletSpeed / 2 ?? 0 + x.Boss?.ProjectileSpeed / 2 ?? 0), 0, 0);
//
//           if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.BulletModel.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.BulletModel.Margin.Left), 2)) >= (double)(x.Boss?.ProjectileRange ?? 0 + x.enemy?.AttackRange ?? 0))
//           {
//               grid.Children.Remove(x.BulletModel);
//               x.BulletModel = null;
//           }
//       }