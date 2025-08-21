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
        bool GoUp, GoDown, GoLeft, GoRight;
        string LastView, Stop, MoveS, Direct;
        public int ZombieSpeed = 3, HP = 100, spawnEnemy, spawnEnemyX, spawnEnemyY, BulletSpeed = 10, Score = 0, EnemyCount;
        double PlayerSpeedX, PlayerSpeedY, Speed = 2, friction = 0.77;
        bool ShootUp, ShootDown, ShootLeft, ShootRight, EnemyExist = true;
        Random CountOfEnemy = new Random();
        Random Move = new Random();

        List<Image> ImageToDel = new List<Image>();
        List<Rectangle> RectToDel = new List<Rectangle>();
        List<string> RunMove = new List<string>()
        {
            "Down","Up","Right","Left","LeftUp","RightUp", "RightDown", "LeftDown", "Stay", "Stay", "Stay", "Stay"
        };
        Rectangle box = new Rectangle // Граница Л-Г
        {
            Height = 1200,
            Width = 10,
            Fill = Brushes.DarkRed,

            Margin = new Thickness(0, -190, 0, 0),
            Tag = "Box",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        Rectangle box2 = new Rectangle // Граница Н-В
        {
            Height = 10,
            Width = 1600,
            Fill = Brushes.DarkRed,

            Margin = new Thickness(0, 795, 0, 0),
            Tag = "Box2",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        Rectangle box3 = new Rectangle // Граница В-В
        {
            Height = 10,
            Width = 1600,
            Fill = Brushes.DarkRed,

            Margin = new Thickness(0, 0, 0, 0),
            Tag = "Box3",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        Rectangle box4 = new Rectangle // Граница П-Г
        {
            Height = 1200,
            Width = 10,
            Fill = Brushes.DarkRed,

            Margin = new Thickness(1525, 0, 0, 0),
            Tag = "Box4",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        Rectangle Player = new Rectangle
        {
            Height = 70,
            Width = 40,
            Fill = Brushes.AntiqueWhite,
            Margin = new Thickness(300, 300, 0, 0),
            Tag = "player",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        Grid Grid1 = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
        };
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

<<<<<<< Updated upstream
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

        public Window1()
        {
            InitializeComponent();
            DispatcherTimer damageCheck = new DispatcherTimer();
            DispatcherTimer timer = new DispatcherTimer();
            DispatcherTimer EnemyMovement = new DispatcherTimer();
            DispatcherTimer PlayerMovement = new DispatcherTimer();
            DispatcherTimer BulletMovement = new DispatcherTimer();
            damageCheck.Tick += Damage_Check;
            damageCheck.Interval = TimeSpan.FromSeconds(1);
            damageCheck.Start();
            BulletMovement.Tick += Bullet_Movement;
            BulletMovement.Interval = TimeSpan.FromMilliseconds(20);
            BulletMovement.Start();
            PlayerMovement.Tick += Player_Movement;
            PlayerMovement.Interval = TimeSpan.FromMilliseconds(20);
            PlayerMovement.Start();
            EnemyMovement.Tick += Enemy_Move;
            EnemyMovement.Interval = TimeSpan.FromMilliseconds(500);
            EnemyMovement.Start();
            timer.Tick += CollusionCheck;
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Start();
            Grid1.Children.Add(box);  Grid1.Children.Add(box2);   Grid1.Children.Add(box3);  Grid1.Children.Add(box4);
            Windows.Content = Grid1;
            Grid1.Children.Add(Player);
            EnemyCount = CountOfEnemy.Next(1, 3);
            Enemy_Spawn(EnemyCount);
            Element_Spawn();
=======
        static double Scaling = App.Current.Windows[0].ActualHeight / 1080;

        Image WeaponCheck = new Image
        {
            Tag = "Collectables",
            Name = "Pistol",
            Width = 50,
            Height = 30,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(500, 500, 0, 0),
            Stretch = Stretch.Fill,
            Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Weapon/Pistol/Pistol.png", UriKind.Absolute)),
        };

        static BitmapImage OpenCaveLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Area/OpenCaveLeft.png", UriKind.Absolute));
        static BitmapImage OpenCaveRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Area/OpenCaveRight.png", UriKind.Absolute));
        static BitmapImage CloseCaveLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Area/CloseCaveLeft.png", UriKind.Absolute));
        static BitmapImage CloseCaveRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Area/CloseCaveRight.png", UriKind.Absolute));

        NPC Trader;

        

        //EnemySpawner Spawner;
        Engine engine;
        Player player;

        public Window1()
        {
            InitializeComponent();
            Grid1.Width *= Scaling;
            Grid1.Height *= Scaling;

            engine = new Engine(Windows, Grid1);


            DispatcherTimer EnemiesChecked = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            EnemiesChecked.Tick += EnemiesKilled;
            EnemiesChecked.Start();
           
            DispatcherTimer GridUpdate = new DispatcherTimer();
            GridUpdate.Tick += Grid_Updater;
            GridUpdate.Interval = TimeSpan.FromMilliseconds(20);
            GridUpdate.Start();

            player = engine.player;
            engine.Element_Spawn(Grid1);
            
            //Spawner = new EnemySpawner(player, Windows);
            //Spawner.Enemy_Spawn("zomnie", Grid1);
            //Grid1.Children.Add(check);
            
            player.PlayerModelCreator();          
            player.PlayerModel.Margin = new Thickness((Windows.Width / 2) - (player.PlayerModel.Width / 2), (Windows.Height / 2) - (player.PlayerModel.Height / 2), 0, 0);

            Grid1.Children.Add(player.PlayerModel);

            WeaponCheck.Margin = new Thickness(Cave1.Margin.Left + Cave1.Width / 2, Cave1.Margin.Top + Cave1.Height + player.PlayerWidth, 0, 0);
            
            Grid1.Children.Add(WeaponCheck);

            Trader = engine.Trader;
            Grid1.Children.Add(Trader.NPC_Trader);
   
            Cave3.Margin = new Thickness(Cave3.Margin.Left + 15 * Scaling, Cave3.Margin.Top + 13 * Scaling, 0, 0);
            Cave4.Margin = new Thickness(Cave4.Margin.Left + 25 * Scaling, Cave4.Margin.Top + 10 * Scaling, 0, 0);
        }

        public Window1(Player player)
        {
            InitializeComponent();
            this.player = player;
            Windows.Content = Grid1;
            Trader = new NPC(player);
            engine = new Engine(this, Grid1, player, Trader);

            DispatcherTimer EnemiesChecked = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            EnemiesChecked.Tick += EnemiesKilled;
            EnemiesChecked.Start();
            
            DispatcherTimer GridUpdate = new DispatcherTimer();
            GridUpdate.Tick += Grid_Updater;
            GridUpdate.Interval = TimeSpan.FromMilliseconds(20);
            GridUpdate.Start();

            engine.Element_Spawn(Grid1);

            //Spawner = new EnemySpawner(player, Windows);
            // Spawner.Enemy_Spawn("zomnie", Grid1);
            //Grid1.Children.Add(check);

            player.PlayerModelCreator();

            switch (player.Level)
            {
                case 1:
                    player.PlayerModel.Margin = new Thickness(Cave1.Margin.Left + Cave1.Width / 2, Cave1.Margin.Top + Cave1.Height, 0, 0);
                    break;
                case 2:
                    player.PlayerModel.Margin = new Thickness(Cave2.Margin.Left + Cave2.Width / 2, Cave2.Margin.Top + Cave2.Height, 0, 0);
                    break;
                case 3:
                    player.PlayerModel.Margin = new Thickness(Cave3.Margin.Left + Cave3.Width / 2, Cave3.Margin.Top + Cave3.Height, 0, 0);
                    break;
                case 4:
                    player.PlayerModel.Margin = new Thickness(Cave4.Margin.Left + Cave4.Width / 2, Cave4.Margin.Top + Cave4.Height, 0, 0);
                    break;

            }

            Grid1.Children.Add(player.PlayerModel);

            Grid1.Children.Add(Trader.NPC_Trader);
>>>>>>> Stashed changes
        }

        private void Damage_Check(object sender, EventArgs e)
        {
<<<<<<< Updated upstream
            Rect PlayerHitBox = new Rect(Player.Margin.Left, Player.Margin.Top, Player.ActualWidth, Player.ActualHeight);
            foreach (var x in Grid1.Children.OfType<Rectangle>())
            {
                Rect EnemyHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                if (x is Rectangle && (string)x.Tag == "Enemy")
                {
                    if (EnemyHitBox.IntersectsWith(PlayerHitBox))
                    {
                        HP -= 20;
=======
            engine.Grid_Update(Grid1);
            foreach (var x in Grid1.Children.OfType<Image>())
            {
                if (x is Image && (string)x.Tag == "CaveDoor1" && player.Level == 0)
                {
                    Cave1.Source = OpenCaveLeft;
                    Rect CaveDoorHitBox = new Rect(x.Margin.Left - player.PlayerModel.Width / 4, x.Margin.Top, x.ActualWidth - player.PlayerModel.Width / 2, x.ActualHeight - player.PlayerModel.Height / 2);
                    if (CaveDoorHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        engine.GoUp = false; engine.GoDown = false; engine.GoRight = false; engine.GoLeft = false;
                        Caves cave = new Caves(player);
                        Windows.Close();
                        Windows = null;
                        cave.ShowDialog(); 
                        //cave.Show();
                    }
                }
                if (x is Image && (string)x.Tag == "CaveDoor2" && player.Level == 1)
                {
                    Cave2.Source = OpenCaveLeft;
                    Rect CaveDoorHitBox = new Rect(x.Margin.Left - player.PlayerModel.Width / 4, x.Margin.Top, x.ActualWidth - player.PlayerModel.Width / 2, x.ActualHeight - player.PlayerModel.Height / 2);
                    if (CaveDoorHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        engine.GoUp = false; engine.GoDown = false; engine.GoRight = false; engine.GoLeft = false;
                        Caves cave = new Caves(player);
                        Windows.Close();
                        cave.ShowDialog();
                        //cave.Show();
                    }
                }
                if (x is Image && (string)x.Tag == "CaveDoor3" && player.Level == 2)
                {
                    Cave3.Source = OpenCaveRight;
                    Rect CaveDoorHitBox = new Rect(x.Margin.Left - player.PlayerModel.Width / 4, x.Margin.Top, x.ActualWidth - player.PlayerModel.Width / 2, x.ActualHeight - player.PlayerModel.Height / 2);
                    if (CaveDoorHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        engine.GoUp = false; engine.GoDown = false; engine.GoRight = false; engine.GoLeft = false;
                        Caves cave = new Caves(player);
                        Windows.Close();
                        cave.ShowDialog();
                        //cave.Show();
                    }
                }
                if (x is Image && (string)x.Tag == "CaveDoor4" && player.Level == 3)
                {
                    Cave4.Source = OpenCaveRight;
                    Rect CaveDoorHitBox = new Rect(x.Margin.Left - player.PlayerModel.Width / 4, x.Margin.Top, x.ActualWidth - player.PlayerModel.Width / 2, x.ActualHeight - player.PlayerModel.Height / 2);
                    if (CaveDoorHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        engine.GoUp = false; engine.GoDown = false; engine.GoRight = false; engine.GoLeft = false;
                        Caves cave = new Caves(player);
                        Windows.Close();
                        cave.ShowDialog();
                        //cave.Show();
                    }
                }

                if (x is Image && (string)x.Tag == "BossDoor" && player.Level == 4)
                {
                    BossRoom.Visibility = Visibility.Visible;
                    Rect BossDoorHitBox = new Rect(x.Margin.Left + x.Width / 4, x.Margin.Top, x.ActualWidth / 2, x.ActualHeight / 2);
                    if (BossDoorHitBox.IntersectsWith(player.PlayerHitBox))
                    {
                        engine.GoUp = false; engine.GoDown = false; engine.GoRight = false; engine.GoLeft = false;
                        BossRoom bossRoom = new BossRoom(player);
                        Windows.Close();
                        bossRoom.ShowDialog();
                        //cave.Show();
>>>>>>> Stashed changes
                    }
                }

                if (Trader.IsShopOpen == true && player.PlayerHitBox.IntersectsWith(Trader.Trader_Intersaction) == false)
                {
                    Trader.Close_Menu();
                    Trader.IsShopOpen = false;
                }
                if (Trader.menu == null && Trader.IsShopOpen == true)
                {
                    Trader.IsShopOpen = false;
                }
            }
        }

        private void Element_Spawn()
        {
<<<<<<< Updated upstream
            Random ranx = new Random();
            Random rany = new Random();
            Coin.Margin = new Thickness(ranx.Next(200, 500), rany.Next(200, 500), 0,0);
            SpeedUpCoin.Margin = new Thickness(ranx.Next(200, 500), rany.Next(200, 500), 0, 0);
            Grid1.Children.Add(Coin);
            Grid1.Children.Add(SpeedUpCoin);
=======
            foreach (Enemy x in engine.EnemyList)
            {
                if (x.Killed == true)
                {
                    x.Delete(x);
                    engine.EnemyDel.Add(x);
                    //Spawner.enemyCount -= 1;
                }
            }
            foreach (Enemy x in engine.EnemyDel)
            {
                engine.EnemyList.Remove(x);
            }
            //check.Text = player.PlayerHP.ToString();
            
            check.Text = player.CurrentAmmo.ToString();
            
            
>>>>>>> Stashed changes
        }

        public void Enemy_Move(object sender, EventArgs e)
        {
            MoveS = RunMove[Move.Next(0, 8)];
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
                    Margin = new Thickness(Player.Margin.Left + Player.ActualWidth, Player.Margin.Top + Player.ActualHeight / 2, 0, 0),
                    Tag = "BulletRight",
                };
                Grid1.Children.Add(bulletR);
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
                    Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + Player.ActualHeight / 2, 0, 0),
                    Tag = "BulletLeft",
                };
                Grid1.Children.Add(bulletL);
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
                    Margin = new Thickness(Player.Margin.Left + Player.ActualWidth / 2, Player.Margin.Top, 0, 0),
                    Tag = "BulletUp",
                };
                Grid1.Children.Add(bulletUp);
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
                    Margin = new Thickness(Player.Margin.Left + Player.ActualWidth / 2, Player.Margin.Top + Player.ActualHeight, 0, 0),
                    Tag = "BulletDown",
                };
                Grid1.Children.Add(bulletDown);
            }
            Direct = "";
        }

        public class Rectn
        {
            public static Rect create;
        }

        public void Enemy_Spawn(int spawn)
        {
            Random ranX = new Random();
            Random ranY = new Random();
            for (int i = 0; i < spawn; i++)
            {
                Rectangle zombie = new Rectangle
                {
                    Height = 30,
                    Width = 20,
                    Fill = Brushes.Aqua,
                    Stroke = Brushes.Yellow,
                    Margin = new Thickness(ranX.Next(300, 700), ranY.Next(100, 350), 0, 0),
                    Tag = "Enemy",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };

                Grid1.Children.Add(zombie);
                Rect EnemyAgro = new Rect(zombie.Margin.Left - 50, zombie.Margin.Top - 50, 100, 100);
                //Rectangle check = new Rectangle
                //{
                //    Width = 100,
                  //  Height = 100,
                    //Margin = new Thickness(zombie.Margin.Left - 50, zombie.Margin.Top - 50, 0, 0),
                    //Stroke = Brushes.Black,
                    //HorizontalAlignment = HorizontalAlignment.Left,
                    //VerticalAlignment = VerticalAlignment.Top,
                //};
                //Grid1.Children.Add(check);
                Rectn.create = EnemyAgro;
            }
            spawnEnemy = 0;
        }
        private void Bullet_Movement(object sender, EventArgs e)
        {
            
            foreach (var x in Grid1.Children.OfType<Rectangle>())
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
                    foreach (var y in Grid1.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "Enemy" && y is Rectangle)
                        {
                            Rect BulletHitBox = new Rect(x.Margin.Top, x.Margin.Left, x.ActualWidth, x.ActualHeight);
                            Rect EnemyHitBox = new Rect(y.Margin.Top, y.Margin.Left, y.ActualWidth, y.ActualHeight);
                            if (EnemyHitBox.IntersectsWith(BulletHitBox))
                            {
                                RectToDel.Add(y);
                                RectToDel.Add(x);
                                EnemyCount -= 1;
                            }
                        }
                    };
                }
                
            }
        }
        private void Player_Movement(object sender, EventArgs e)
        {

            if (GoUp)
            {
                PlayerSpeedY -= Speed;
            }
            if (GoDown)
            {
                PlayerSpeedY += Speed;
            }
            if (GoRight)
            {
                PlayerSpeedX += Speed;
            }
            if (GoLeft)
            {
                PlayerSpeedX -= Speed;
            }

            PlayerSpeedX *= friction;
            PlayerSpeedY *= friction;
            Player.Margin = new Thickness(Player.Margin.Left + PlayerSpeedX, Player.Margin.Top + PlayerSpeedY, 0, 0);
            Collision("x");
            Collision("y");
        }

        public void CollusionCheck(object sender, EventArgs e)
        {
            Rect PlayerHitBox = new Rect(Player.Margin.Left, Player.Margin.Top, Player.ActualWidth, Player.ActualHeight);
            foreach (var x in Grid1.Children.OfType<Rectangle>())
            {
                Rect EnemyAgr = new Rect(x.Margin.Left - 50, x.Margin.Top - 50, 200, 200);
                Rect EnemyHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                if (x is Rectangle && (string)x.Tag == "Enemy")
                {

                    if (EnemyAgr.IntersectsWith(PlayerHitBox))
                    {
                        if (x.Margin.Left < Player.Margin.Left)
                        {
                            x.Margin = new Thickness(x.Margin.Left + ZombieSpeed, x.Margin.Top, 0, 0);
                        }
                        if (x.Margin.Left > Player.Margin.Left)
                        {
                            x.Margin = new Thickness(x.Margin.Left - ZombieSpeed, x.Margin.Top, 0, 0);
                        }
                        if (x.Margin.Top < Player.Margin.Top)
                        {
                            x.Margin = new Thickness(x.Margin.Left, x.Margin.Top + ZombieSpeed, 0, 0);
                        }
                        if (x.Margin.Top > Player.Margin.Top)
                        {
                            x.Margin = new Thickness(x.Margin.Left, x.Margin.Top - ZombieSpeed, 0, 0);
                        }
                    }
                    else
                    {
                        if (EnemyHitBox.IntersectsWith(Rectn.create))
                        {
                            if (MoveS == "Up")
                            {
                                x.Margin = new Thickness(x.Margin.Left, x.Margin.Top - ZombieSpeed, 0, 0);
                            }
                            if (MoveS == "RightUp")
                            {
                                x.Margin = new Thickness(x.Margin.Left + ZombieSpeed, x.Margin.Top - ZombieSpeed, 0, 0);
                            }
                            if (MoveS == "LeftUp")
                            {
                                x.Margin = new Thickness(x.Margin.Left - ZombieSpeed, x.Margin.Top - ZombieSpeed, 0, 0);
                            }
                            if (MoveS == "LeftDown")
                            {
                                x.Margin = new Thickness(x.Margin.Left - ZombieSpeed, x.Margin.Top + ZombieSpeed, 0, 0);
                            }
                            if (MoveS == "RightDown")
                            {
                                x.Margin = new Thickness(x.Margin.Left + ZombieSpeed, x.Margin.Top + ZombieSpeed, 0, 0);
                            }
                            if (MoveS == "Down")
                            {
                                x.Margin = new Thickness(x.Margin.Left, x.Margin.Top + ZombieSpeed, 0, 0);
                            }
                            if (MoveS == "Left")
                            {
                                x.Margin = new Thickness(x.Margin.Left - ZombieSpeed, x.Margin.Top, 0, 0);
                            }
                            if (MoveS == "Right")
                            {
                                x.Margin = new Thickness(x.Margin.Left + ZombieSpeed, x.Margin.Top, 0, 0);
                            }
                        }
                        else
                        {
                            if (x.Margin.Top < Rectn.create.Top)
                            {
                                x.Margin = new Thickness(x.Margin.Left, x.Margin.Top + ZombieSpeed, 0, 0);
                            }
                            if (x.Margin.Top > Rectn.create.Top)
                            {
                                x.Margin = new Thickness(x.Margin.Left, x.Margin.Top - ZombieSpeed, 0, 0);
                            }
                            if (x.Margin.Left < Rectn.create.Left)
                            {
                                x.Margin = new Thickness(x.Margin.Left + ZombieSpeed, x.Margin.Top, 0, 0);
                            }
                            if (x.Margin.Left > Rectn.create.Left)
                            {
                                x.Margin = new Thickness(x.Margin.Left - ZombieSpeed, x.Margin.Top, 0, 0);
                            }
                        }

                    }
                }
            }
        }

        private void Collision(string Dir)
        {

            foreach (var y in ImageToDel)
            {
                Grid1.Children.Remove(y);
            }
            foreach (var y in RectToDel)
            {
                Grid1.Children.Remove(y);
            }
            Rect PlayerHitBox = new Rect(Player.Margin.Left, Player.Margin.Top, Player.ActualWidth, Player.ActualHeight);

            foreach (var x in Grid1.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "Box")
                {
                    Rect BoxHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (BoxHitBox.IntersectsWith(PlayerHitBox))
                    {
                        if (Dir == "x")
                        {
                            Player.Margin = new Thickness(Player.Margin.Left - PlayerSpeedX, Player.Margin.Top, 0, 0);
                            PlayerSpeedX = 0;
                        }
                        else
                        {
                            Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - PlayerSpeedY, 0, 0);
                            PlayerSpeedY = 0;
                        }


                    }
                }
                if (x is Rectangle && (string)x.Tag == "Box2")
                {
                    Rect BoxHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (BoxHitBox.IntersectsWith(PlayerHitBox))
                    {
                        if (Dir == "x")
                        {
                            Player.Margin = new Thickness(Player.Margin.Left - PlayerSpeedX, Player.Margin.Top, 0, 0);
                            PlayerSpeedX = 0;
                        }
                        else
                        {
                            Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - PlayerSpeedY, 0, 0);
                            PlayerSpeedY = 0;
                        }
                    }
                }
                if (x is Rectangle && (string)x.Tag == "Box3")
                {
                    Rect BoxHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (BoxHitBox.IntersectsWith(PlayerHitBox))
                    {
                        if (Dir == "x")
                        {
                            Player.Margin = new Thickness(Player.Margin.Left - PlayerSpeedX, Player.Margin.Top, 0, 0);
                            PlayerSpeedX = 0;
                        }
                        else
                        {
                            Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - PlayerSpeedY, 0, 0);
                            PlayerSpeedY = 0;
                        }
                    }

                }
                if (x is Rectangle && (string)x.Tag == "Box4")
                {
                    Rect BoxHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (BoxHitBox.IntersectsWith(PlayerHitBox))
                    {
                        if (Dir == "x")
                        {
                            Player.Margin = new Thickness(Player.Margin.Left - PlayerSpeedX, Player.Margin.Top, 0, 0);
                            PlayerSpeedX = 0;
                        }
                        else
                        {
                            Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - PlayerSpeedY, 0, 0);
                            PlayerSpeedY = 0;
                        }


                    }
                }
            }
            foreach (var x in Grid1.Children.OfType<Image>())
            {
                if (x is Image && (string)x.Tag == "Collectables")
                {
                    Rect CollectHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (PlayerHitBox.IntersectsWith(CollectHitBox))
                    {
                        if ((string)x.Name == "coin")
                        {
                            Score += 1;
                            ImageToDel.Add(x);
                        }
                        if ((string)x.Name == "SpeedUp")
                        {
                            Speed += 1;
                            ImageToDel.Add(x);
                        }
                    }
                }
            }
            int cordx = 0;
            int cordy = 0;


            if (EnemyCount == 1)
                foreach (var x in Grid1.Children.OfType<Rectangle>())
                {
                    if ((string)x.Tag == "Enemy" && x is Rectangle)
                    {
                        cordx = (int)x.Margin.Left;
                        cordy = (int)x.Margin.Top;
                    }
                }
            if (EnemyCount == 0)
            {

                Image CoinS = new Image
                {                  
                    Name = "coin",
                    Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/coin.png", UriKind.Absolute)),
                    Height = 30,
                    Width = 30,
                    Stretch = Stretch.Fill,
                    Tag = "Collectables",
                    Margin = new Thickness(cordx +70, cordy + 70, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                Grid1.Children.Add(CoinS);
                EnemyCount = CountOfEnemy.Next(1,3);
                Enemy_Spawn(EnemyCount);
            }
            if (HP <= 0)
            {
                RectToDel.Add(Player);
            }
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W && Stop != "Up")
            {
                LastView = "Up";
                GoUp = true;
            }
            if (e.Key == Key.S && Stop != "Down")
            {
                LastView = "Down";
                GoDown = true;
            }
            if (e.Key == Key.D && Stop != "Right")
            {
                LastView = "Right";
                GoRight = true;
            }
            if (e.Key == Key.A && Stop != "Left")
            {
                LastView = "Left";
                GoLeft = true;
            }

            if (e.Key == Key.Up)
            {
                ShootUp = true;
                Direct = "Up";
                Bullet(Direct);
            }
            if (e.Key == Key.Down)
            {
                ShootDown = true;
                Direct = "Down";
                Bullet(Direct);
            }
            if (e.Key == Key.Left)
            {
                ShootLeft = true;
                Direct = "Left";
                Bullet(Direct);
            }
            if (e.Key == Key.Right)
            {
                ShootRight = true;
                Direct = "Right";
                Bullet(Direct);
            }

        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W && Stop != "Up")
            {
                LastView = "";
                GoUp = false;
            }
            if (e.Key == Key.S && Stop != "Down")
            {
                LastView = "";
                GoDown = false;
            }
            if (e.Key == Key.D && Stop != "Right")
            {
                LastView = "";
                GoRight = false;
            }
            if (e.Key == Key.A && Stop != "Left")
            {
                LastView = "";
                GoLeft = false;
            }

            if (e.Key == Key.Up)
            {
                ShootUp = false;
            }
            if (e.Key == Key.Down)
            {
                ShootDown = false;
            }
            if (e.Key == Key.Left)
            {
                ShootLeft = false;
            }
            if (e.Key == Key.Right)
            {
                ShootRight = false;
            }
        }

    }
}
