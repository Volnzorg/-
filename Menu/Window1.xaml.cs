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
        }

        private void Damage_Check(object sender, EventArgs e)
        {
            Rect PlayerHitBox = new Rect(Player.Margin.Left, Player.Margin.Top, Player.ActualWidth, Player.ActualHeight);
            foreach (var x in Grid1.Children.OfType<Rectangle>())
            {
                Rect EnemyHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                if (x is Rectangle && (string)x.Tag == "Enemy")
                {
                    if (EnemyHitBox.IntersectsWith(PlayerHitBox))
                    {
                        HP -= 20;
                    }
                }
            }
        }

        private void Element_Spawn()
        {
            Random ranx = new Random();
            Random rany = new Random();
            Coin.Margin = new Thickness(ranx.Next(200, 500), rany.Next(200, 500), 0,0);
            SpeedUpCoin.Margin = new Thickness(ranx.Next(200, 500), rany.Next(200, 500), 0, 0);
            Grid1.Children.Add(Coin);
            Grid1.Children.Add(SpeedUpCoin);
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
