using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace Menu
{
    public class Engine
    {
        public bool GoUp, GoDown, GoLeft, GoRight;
        string Direct;
        public int HP = 100, BulletSpeed = 10, Score = 0;
        public Player player; 

        public List<Enemy> EnemyList = new List<Enemy>();
        public List<Enemy> EnemyDel = new List<Enemy>();
        List<Image> ImageToDel = new List<Image>();
        List<Rectangle> RectToDel = new List<Rectangle>();

        Image Coin = new Image
        {
            Name = "coin",
            Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/coin.png", UriKind.Absolute)),

            Height = 30,
            Width = 30,
            Stretch = Stretch.Fill,
            Tag = "Collectables",
            Margin = new Thickness(0, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        Image SpeedUpCoin = new Image
        {
            Name = "SpeedUp",
            Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Trava.png", UriKind.Absolute)),
            Height = 50,
            Width = 50,
            Stretch = Stretch.Fill,
            Tag = "Collectables",
            Margin = new Thickness(0, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };


        Grid grid;
        Caves cave;

        Window GameScreen;
        public Engine(Player player, Grid grid, Caves cave)
        {
            this.cave = cave;
            this.GameScreen = cave.GameScreen;
            this.player = player;
            this.grid = grid;

            GameScreen.KeyDown += KeyIsDown;
            GameScreen.KeyUp += KeyIsUp;
            DispatcherTimer damageCheck = new DispatcherTimer();
            DispatcherTimer PlayerMovement = new DispatcherTimer();
            DispatcherTimer BulletMovement = new DispatcherTimer();
            damageCheck.Tick += Damage_Check;
            damageCheck.Interval = TimeSpan.FromSeconds(1);
            damageCheck.Start();
            BulletMovement.Tick += Bullet_Movement;
            BulletMovement.Interval = TimeSpan.FromMilliseconds(10);
            BulletMovement.Start();
            PlayerMovement.Tick += Player_Movement;
            PlayerMovement.Interval = TimeSpan.FromMilliseconds(20);
            PlayerMovement.Start();
        }

        public Engine(Window window, Grid grid)
        {
            this.grid = grid;
            this.GameScreen = window;
            player = new Player();
            GameScreen.KeyDown += KeyIsDown;
            GameScreen.KeyUp += KeyIsUp;
            DispatcherTimer damageCheck = new DispatcherTimer();
            DispatcherTimer PlayerMovement = new DispatcherTimer();
            DispatcherTimer BulletMovement = new DispatcherTimer();
            damageCheck.Tick += Damage_Check;
            damageCheck.Interval = TimeSpan.FromSeconds(1);
            damageCheck.Start();
            BulletMovement.Tick += Bullet_Movement;

            BulletMovement.Interval = TimeSpan.FromMilliseconds(10);
            BulletMovement.Start();
            PlayerMovement.Tick += Player_Movement;
            PlayerMovement.Interval = TimeSpan.FromMilliseconds(20);
            PlayerMovement.Start();
        }

        public Engine(Window1 window, Grid grid, Player player)
        {
            this.player = player;
            this.grid = grid;
            this.GameScreen = window.Windows;
            GameScreen.KeyDown += KeyIsDown;
            GameScreen.KeyUp += KeyIsUp;
            DispatcherTimer damageCheck = new DispatcherTimer();
            DispatcherTimer PlayerMovement = new DispatcherTimer();
            DispatcherTimer BulletMovement = new DispatcherTimer();
            damageCheck.Tick += Damage_Check;
            damageCheck.Interval = TimeSpan.FromSeconds(1);
            damageCheck.Start();
            BulletMovement.Tick += Bullet_Movement;

            BulletMovement.Interval = TimeSpan.FromMilliseconds(10);
            BulletMovement.Start();
            PlayerMovement.Tick += Player_Movement;
            PlayerMovement.Interval = TimeSpan.FromMilliseconds(20);
            PlayerMovement.Start();
        }

        public void Grid_Update(Grid Gridik)
        {
            grid = Gridik;
        }

        private void Damage_Check(object sender, EventArgs e)
        {
            foreach (Enemy x in EnemyList)
            {
                player.DamageCheck(x.EnemyHitBox);
            }
            if (player.PlayerHP <= 0)
            {
                grid.Children.Remove(player.PlayerModel);
                player.PlayerModel = null;
            }
        }


        public void Element_Spawn(Grid Gridik)
        {
            grid = Gridik;
            Random ranx = new Random();
            Random rany = new Random();
            Coin.Margin = new Thickness(ranx.Next(200, 500), rany.Next(200, 500), 0, 0);
            SpeedUpCoin.Margin = new Thickness(ranx.Next(200, 500), rany.Next(200, 500), 0, 0);
            grid.Children.Add(Coin);
            grid.Children.Add(SpeedUpCoin);
        }

        private void Bullet(string Dir)
        {
            if (Dir == "Right")
            {
                Rectangle bulletR = new Rectangle
                {
                    Name = "Bullets",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 5,
                    Width = 5,
                    Fill = Brushes.Yellow,
                    Stroke = Brushes.Black,
                    Margin = new Thickness(player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth, player.PlayerModel.Margin.Top + player.PlayerModel.ActualHeight / 2, 0, 0),
                    Tag = "BulletRight",
                };
                grid.Children.Add(bulletR);
            }
            if (Dir == "Left")
            {
                Rectangle bulletL = new Rectangle
                {
                    Name = "Bullets",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 5,
                    Width = 5,
                    Fill = Brushes.Yellow,
                    Stroke = Brushes.Black,
                    Margin = new Thickness(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top + player.PlayerModel.ActualHeight / 2, 0, 0),
                    Tag = "BulletLeft",
                };
                grid.Children.Add(bulletL);
            }
            if (Dir == "Up")
            {
                Rectangle bulletUp = new Rectangle
                {
                    Name = "Bullets",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 5,
                    Width = 5,
                    Fill = Brushes.Yellow,
                    Stroke = Brushes.Black,
                    Margin = new Thickness(player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth / 2, player.PlayerModel.Margin.Top, 0, 0),
                    Tag = "BulletUp",
                };
                grid.Children.Add(bulletUp);
            }
            if (Dir == "Down")
            {
                Rectangle bulletDown = new Rectangle
                {
                    Name = "Bullets",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 5,
                    Width = 5,
                    Fill = Brushes.Yellow,
                    Stroke = Brushes.Black,
                    Margin = new Thickness(player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth / 2, player.PlayerModel.Margin.Top + player.PlayerModel.ActualHeight, 0, 0),
                    Tag = "BulletDown",
                };
                grid.Children.Add(bulletDown);
            }
            Direct = "";
        }

        private void Bullet_Movement(object sender, EventArgs e)
        {

            foreach (var x in grid.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Name == "Bullets")
                {
                    if (x is Rectangle && (string)x.Tag == "BulletUp")
                    {
                        x.Margin = new Thickness(x.Margin.Left, x.Margin.Top - BulletSpeed, 0, 0);

                    }
                    if (x is Rectangle && (string)x.Tag == "BulletDown")
                    {
                        x.Margin = new Thickness(x.Margin.Left, x.Margin.Top + BulletSpeed, 0, 0);

                    }
                    if (x is Rectangle && (string)x.Tag == "BulletLeft")
                    {
                        x.Margin = new Thickness(x.Margin.Left - BulletSpeed, x.Margin.Top, 0, 0);

                    }
                    if (x is Rectangle && (string)x.Tag == "BulletRight")
                    {
                        x.Margin = new Thickness(x.Margin.Left + BulletSpeed, x.Margin.Top, 0, 0);

                    }
                    Rect BulletHB = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    foreach (Enemy y in EnemyList)
                    {
                        y.Damege(BulletHB, 1, x);
                    }
                }

            }
        }
        private void Player_Movement(object sender, EventArgs e)
        {

            if (GoUp)
            {
                player.PlayerSpeedY -= player.Speed;
            }
            if (GoDown)
            {
                player.PlayerSpeedY += player.Speed;
            }
            if (GoRight)
            {
                player.PlayerSpeedX += player.Speed;
            }
            if (GoLeft)
            {
                player.PlayerSpeedX -= player.Speed;
            }

            player.PlayerSpeedX *= player.friction;
            player.PlayerSpeedY *= player.friction;
            player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left + player.PlayerSpeedX, player.PlayerModel.Margin.Top + player.PlayerSpeedY, 0, 0);
            //grid.Margin = new Thickness(grid.Margin.Left - player.PlayerSpeedX, grid.Margin.Top - player.PlayerSpeedY, 0, 0); Сделать только в главной комнате
            Collision("x");
            Collision("y");
        }
    

        private void Collision(string Dir)
        {

            foreach (var y in ImageToDel)
            {
                grid.Children.Remove(y);
            }
            foreach (var y in RectToDel)
            {
                grid.Children.Remove(y);
            }

            foreach (var x in grid.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "Border")
                {
                    Rect BoxHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (BoxHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        if (Dir == "x")
                        {
                            player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left - player.PlayerSpeedX, player.PlayerModel.Margin.Top, 0, 0);
                            player.PlayerSpeedX = 0;
                        }
                        else
                        {
                            player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top - player.PlayerSpeedY, 0, 0);
                            player.PlayerSpeedY = 0;
                        }

                    }
                }

            }
            foreach (var x in grid.Children.OfType<Image>())
            {
                if (x is Image && (string)x.Tag == "Collectables")
                {
                    Rect CollectHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (player.PlayerHitBox.IntersectsWith(CollectHitBox))
                    {
                        if ((string)x.Name == "coin")
                        {
                            Score += 1;
                            ImageToDel.Add(x);
                        }
                        if ((string)x.Name == "SpeedUp")
                        {
                            player.Speed += 2;
                            player.PlayerCurrentSpeed += 2;
                            ImageToDel.Add(x);
                        }
                        if (x.Name == "Key")
                        {
                            player.Level += 1;
                            ImageToDel.Add(x);
                            Window1 window1 = new Window1(player);
                            cave.Close();
                            window1.ShowDialog();
                        }
                    }
                }
            }
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                GoUp = true;
            }
            if (e.Key == Key.S)
            {
                GoDown = true;
            }
            if (e.Key == Key.D)
            {
                GoRight = true;
            }
            if (e.Key == Key.A)
            {
                GoLeft = true;
            }

            if (e.Key == Key.Up)
            {
                Direct = "Up";
                Bullet(Direct);
            }
            if (e.Key == Key.Down)
            {
                Direct = "Down";
                Bullet(Direct);
            }
            if (e.Key == Key.Left)
            {
                Direct = "Left";
                Bullet(Direct);
            }
            if (e.Key == Key.Right)
            {
                Direct = "Right";
                Bullet(Direct);
            }

        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                GoUp = false;
            }
            if (e.Key == Key.S)
            {
                GoDown = false;
            }
            if (e.Key == Key.D)
            {
                GoRight = false;
            }
            if (e.Key == Key.A)
            {
                GoLeft = false;
            }
        }
    }
}
