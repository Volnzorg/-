using System;
using System.Collections;
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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        
        TextBox check = new TextBox
        {
            Height = 70,
            Width = 40,
            Margin = new Thickness(0, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Text = ""
        };

        Rectangle Box1 = new Rectangle
        {
            Tag = "CaveDoor1",
            Width = 50,
            Height = 50,
            Fill = Brushes.Red,
            Stroke = Brushes.Black,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(700, 300, 0, 0)
        };

        Rectangle Box2 = new Rectangle
        {
            Tag = "CaveDoor2",
            Width = 50,
            Height = 50,
            Fill = Brushes.Red,
            Stroke = Brushes.Black,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(300, 300, 0, 0)
        };

        Rectangle Box3 = new Rectangle
        {
            Tag = "CaveDoor3",
            Width = 50,
            Height = 50,
            Fill = Brushes.Red,
            Stroke = Brushes.Black,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(700, 700, 0, 0)
        };

        Rectangle Box4 = new Rectangle
        {
            Tag = "CaveDoor4",
            Width = 50,
            Height = 50,
            Fill = Brushes.Red,
            Stroke = Brushes.Black,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(300, 700, 0, 0)
        };

        Rectangle Box5 = new Rectangle
        {
            Tag = "BossRoomTeleporter",
            Width = 50,
            Height = 50,
            Fill = Brushes.Teal,
            Stroke = Brushes.Black,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(500, 100, 0, 0)
        };


        public Grid Grid1 = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
        };

        EnemySpawner Spawner;
        Engine engine;
        Player player;

        public Window1()
        {
            InitializeComponent();
            Windows.Content = Grid1;
            engine = new Engine(Windows, Grid1);

            DispatcherTimer EnemiesChecked = new DispatcherTimer();
            EnemiesChecked.Interval = TimeSpan.FromMilliseconds(20);
            EnemiesChecked.Tick += EnemiesKilled;
            EnemiesChecked.Start();
            DispatcherTimer GridUpdate = new DispatcherTimer();
            GridUpdate.Tick += Grid_Updater;
            GridUpdate.Interval = TimeSpan.FromMilliseconds(20);
            GridUpdate.Start();

            player = engine.player;
            engine.Element_Spawn(Grid1);
            //Spawner = new EnemySpawner(player, Windows);
           // Spawner.Enemy_Spawn("zomnie", Grid1);
            Grid1.Children.Add(check);
            player.PlayerModelCreator();
            player.PlayerModel.Margin = new Thickness(Windows.Width / 2, Windows.Height / 2, 0, 0);
            Grid1.Children.Add(player.PlayerModel);
            Grid1.Children.Add(Box1); Grid1.Children.Add(Box2); Grid1.Children.Add(Box3); Grid1.Children.Add(Box4);
        }
        public Window1(Player player)
        {
            InitializeComponent();
            this.player = player;
            Windows.Content = Grid1;
            engine = new Engine(this, Grid1, player);

            DispatcherTimer EnemiesChecked = new DispatcherTimer();
            EnemiesChecked.Interval = TimeSpan.FromMilliseconds(20);
            EnemiesChecked.Tick += EnemiesKilled;
            EnemiesChecked.Start();
            DispatcherTimer GridUpdate = new DispatcherTimer();
            GridUpdate.Tick += Grid_Updater;
            GridUpdate.Interval = TimeSpan.FromMilliseconds(20);
            GridUpdate.Start();

            engine.Element_Spawn(Grid1);
            //Spawner = new EnemySpawner(player, Windows);
            // Spawner.Enemy_Spawn("zomnie", Grid1);
            Grid1.Children.Add(check);
            player.PlayerModelCreator();
            player.PlayerModel.Margin = new Thickness(Windows.Width / 2, Windows.Height / 2, 0, 0);
            Grid1.Children.Add(player.PlayerModel);
            Grid1.Children.Add(Box1); Grid1.Children.Add(Box2); Grid1.Children.Add(Box3); Grid1.Children.Add(Box4);

            if (player.Level == 4)
            {
                Grid1.Children.Add(Box5);
            }
        }

        private void Grid_Updater(object sender, EventArgs e)
        {
            engine.Grid_Update(Grid1);
            foreach (var x in Grid1.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "CaveDoor1" && player.Level == 0)
                {
                    Box1.Fill = Brushes.Coral;
                    Rect CaveDoorHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (CaveDoorHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        engine.GoUp = false; engine.GoDown = false; engine.GoRight = false; engine.GoLeft = false;
                        Caves cave = new Caves(player);
                        Windows.Close();
                        cave.ShowDialog();
                        //cave.Show();
                    }
                }
                if (x is Rectangle && (string)x.Tag == "CaveDoor2" && player.Level == 1)
                {
                    Box2.Fill = Brushes.Coral;
                    Rect CaveDoorHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (CaveDoorHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        Caves cave = new Caves(player);
                        Windows.Close();
                        cave.ShowDialog();
                        //cave.Show();
                    }
                }
                if (x is Rectangle && (string)x.Tag == "CaveDoor3" && player.Level == 2)
                {
                    Box3.Fill = Brushes.Coral;
                    Rect CaveDoorHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (CaveDoorHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        Caves cave = new Caves(player);
                        Windows.Close();
                        cave.ShowDialog();
                        //cave.Show();
                    }
                }
                if (x is Rectangle && (string)x.Tag == "CaveDoor4" && player.Level == 3)
                {
                    Box4.Fill = Brushes.Coral;
                    Rect CaveDoorHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (CaveDoorHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        Caves cave = new Caves(player);
                        Windows.Close();
                        cave.ShowDialog();
                        //cave.Show();
                    }
                }
            }
        }

        public void EnemiesKilled(object sender, EventArgs e)
        {
            foreach (Enemy x in engine.EnemyList)
            {
                if (x.Killed == true)
                {
                    x.Delete();
                    engine.EnemyDel.Add(x);
                    Spawner.enemyCount -= 1;
                }
            }
            foreach (Enemy x in engine.EnemyDel)
            {
                engine.EnemyList.Remove(x);
            }
            check.Text = player.PlayerHP.ToString();
        }

        public void Enemies(Enemy enem)
        {
            engine.EnemyList.Add(enem);
        }
    }
}
