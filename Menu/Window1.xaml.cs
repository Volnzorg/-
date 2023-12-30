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
        List<Enemy> EnemyList = new List<Enemy>();
        List<Enemy> EnemyDel = new List<Enemy>();

        List<Image> ImageToDel = new List<Image>();
        List<Rectangle> RectToDel = new List<Rectangle>();
        List<string> RunMove = new List<string>()
        {
            "Down","Up","Right","Left","LeftUp","RightUp", "RightDown", "LeftDown", "Stay", "Stay", "Stay", "Stay"
        };


        TextBox check = new TextBox
        {
            Height = 70,
            Width = 40,
            Margin = new Thickness(0, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Text = ""
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

        Rectangle box = new Rectangle // Граница Л-Г
        {
            Name = "BorderL",
            Height = 0,
            Width = 1,
            Fill = Brushes.DarkRed,

            Margin = new Thickness(0, 0, 0, 0),
            Tag = "Border",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        Rectangle box2 = new Rectangle // Граница Н-В
        {
            Height = 1,
            Width = 0,
            Fill = Brushes.DarkRed,

            Margin = new Thickness(0, 0, 0, 0),
            Tag = "Border",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        Rectangle box3 = new Rectangle // Граница В-В
        {
            Name = "BorederUp",
            Height = 1,
            Width = 0,
            Fill = Brushes.DarkRed,

            Margin = new Thickness(0, 0, 0, 0),
            Tag = "Border",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        Rectangle box4 = new Rectangle // Граница П-Г
        {
            Height = 0,
            Width = 1,
            Fill = Brushes.DarkRed,

            Margin = new Thickness(0, 0, 0, 0),
            Tag = "Border",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        EnemySpawner Spawner;

        public Window1()
        {
            InitializeComponent();
            DispatcherTimer damageCheck = new DispatcherTimer();
            DispatcherTimer PlayerMovement = new DispatcherTimer();
            DispatcherTimer BulletMovement = new DispatcherTimer();
            DispatcherTimer BorderCreator = new DispatcherTimer();
            BorderCreator.Tick += Screen_Borders;
            BorderCreator.Interval = TimeSpan.FromMilliseconds(200);
            BorderCreator.Start();
            damageCheck.Tick += Damage_Check;
            damageCheck.Interval = TimeSpan.FromMilliseconds(500);
            damageCheck.Start();
            BulletMovement.Tick += Bullet_Movement;
            BulletMovement.Tick += EnemiesKilled;
            BulletMovement.Interval = TimeSpan.FromMilliseconds(10);
            BulletMovement.Start();
            PlayerMovement.Tick += Player_Movement;
            PlayerMovement.Interval = TimeSpan.FromMilliseconds(20);
            PlayerMovement.Start();
            Windows.Content = Grid1;
            Grid1.Children.Add(box); Grid1.Children.Add(box2); Grid1.Children.Add(box3); Grid1.Children.Add(box4);
            Grid1.Children.Add(Player);
            Spawner = new EnemySpawner(Grid1, Player, Windows);
            EnemyCount = CountOfEnemy.Next(2, 3);
            Spawner.Enemy_Spawn("zomnie");
            Element_Spawn();
            Grid1.Children.Add(check);
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
            check.Text = Spawner.enemyCount.ToString();
        }

        private void Screen_Borders(object sender, EventArgs e)
        {

            foreach (var x in Grid1.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "Border")
                {
                    if ((x.Name == "BorderL" && x.ActualHeight != Windows.ActualHeight) || (x.Name == "BorderUp" && x.ActualWidth != Windows.ActualWidth))
                    {
                        box4.Height = Windows.ActualHeight; box4.Margin = new Thickness(Windows.ActualWidth - 20, 0, 0, 0);
                        box3.Width = Windows.ActualWidth - 30;
                        box2.Width = Windows.ActualWidth - 30; box2.Margin = new Thickness(0, Windows.ActualHeight - 30, 0, 0);
                        box.Height = Windows.ActualHeight - 30;
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

        public void Enemies(Enemy enem)
        {
            EnemyList.Add(enem);
        }

        public void EnemiesKilled(object sender, EventArgs e)
        {
            foreach (Enemy x in EnemyList)
            {
                if (x.Killed == true)
                {
                    x.Delete();
                    EnemyDel.Add(x);
                    Spawner.enemyCount -= 1;  
                }
            }
            foreach (Enemy x in EnemyDel)
            {
                EnemyList.Remove(x);
            }
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
                    Rect BulletHB = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    foreach(Enemy y in EnemyList)
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
                if (x is Rectangle && (string)x.Tag == "Border")
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
            double cordx = 0;
            double cordy = 0;


            //if (EnemyCount == 1)
              //  foreach (var x in Grid1.Children.OfType<Rectangle>())
               // {
                //    if ((string)x.Tag == "Enemy" && x is Rectangle)
                  //  {
                    //    cordx = x.Margin.Left;
                      //  cordy = x.Margin.Top;
                   // }
               // }
            //if (EnemyCount == 0)
            //{
            //
              //  Image CoinS = new Image
                //{                  
                 //   Name = "coin",
                   // Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/coin.png", UriKind.Absolute)),
                    //Height = 30,
                   // Width = 30,
                    //Stretch = Stretch.Fill,
                    //Tag = "Collectables",
                    //Margin = new Thickness(cordx +70, cordy + 70, 0, 0),
                    //VerticalAlignment = VerticalAlignment.Top,
                    //HorizontalAlignment = HorizontalAlignment.Left,
                //};
                //Grid1.Children.Add(CoinS);
                //EnemyCount = CountOfEnemy.Next(1,3);
                //Spawner.Enemy_Spawn("zombie");
            //}
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
