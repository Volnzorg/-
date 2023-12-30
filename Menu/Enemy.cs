using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Menu.Window1;

namespace Menu
{

    public class Enemy
    {
        int ZombieSpeed = 5, x, y;
        public bool Killed = false;
        public int EnemyHP = 3, Damage, TakenDamage;
        Rect EnemyAgr, EnemyPosition, EnemyHitBox;
        Rectangle Player;
        Grid grid;
        string MoveS;
        Rect EnemyAgro;
        Window1 window;
        List<Rectangle> RectToDel = new List<Rectangle>();

        TextBox enemyHPBar = new TextBox
        {
            Width = 20,
            Height = 30,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 0, 0, 0),
            Text = "",
            FontSize = 12,
        };

        TextBox check = new TextBox
        {
            Height = 70,
            Width = 40,
            Margin = new Thickness(300, 300, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Text = ""
        };

        List<string> RunMove = new List<string>()
        {
            "Down","Up","Right","Left","LeftUp","RightUp", "RightDown", "LeftDown", "Stay", "Stay", "Stay", "Stay"
        };

        public Rectangle EnemyModel = new Rectangle
        {
            Height = 30,
            Width = 20,
            Fill = Brushes.Aqua,
            Stroke = Brushes.Yellow,
            Tag = "Enemy",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        public Enemy(Rectangle Player, Grid grid, Rect EnemyAgro, int x, int y, Window1 window)
        {
            this.Player = Player;
            this.grid = grid;
            this.EnemyAgro = EnemyAgro;
            this.window = window;
            this.y = y; this.x = x;
            
            EnemyModel.Margin = new Thickness(x, y, 0, 0);
            DispatcherTimer DeadChecking = new DispatcherTimer();
            DeadChecking.Tick += DeadCheck;
            DeadChecking.Interval = TimeSpan.FromMilliseconds(30);
            DeadChecking.Start();
            DispatcherTimer Collision = new DispatcherTimer();
            Collision.Tick += CollusionCheck;
            Collision.Interval = TimeSpan.FromMilliseconds(100);
            Collision.Start();
            DispatcherTimer EnemyMovement = new DispatcherTimer();
            EnemyMovement.Tick += Enemy_Move;
            EnemyMovement.Interval = TimeSpan.FromMilliseconds(500);
            EnemyMovement.Start();
            //grid.Children.Add(check);
            grid.Children.Add(enemyHPBar);
        }

        public Enemy()
        {

        }



        public void Enemy_Move(object sender, EventArgs e)
        {
            Random Move = new Random();
            MoveS = RunMove[Move.Next(0, 8)];
        }

        public void Damege(Rect BulletHitBox, int PlayerDamage, Rectangle Bullet)
        {
            EnemyHitBox = new Rect(EnemyModel.Margin.Left, EnemyModel.Margin.Top, EnemyModel.ActualWidth, EnemyModel.ActualHeight);

            if (EnemyHitBox.IntersectsWith(BulletHitBox))
            {
                EnemyHP -= PlayerDamage;
                RectToDel.Add(Bullet);
                check.Text += "1";
            }
        }

        public void DeadCheck(object sender, EventArgs e)
        {
            foreach (var x in RectToDel)
            {
                grid.Children.Remove(x);
            }

            if (EnemyHP <= 0 && Killed == false)
            {
                Image Coin = new Image
                {
                    Name = "coin",
                    Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/coin.png", UriKind.Absolute)),

                    Height = 30,
                    Width = 30,
                    Stretch = Stretch.Fill,
                    Tag = "Collectables",
                    Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                grid.Children.Add(Coin);
                Killed = true;
            }
        }

        public void CollusionCheck(object sender, EventArgs e)
        {
            Rect PlayerHitBox = new Rect(Player.Margin.Left, Player.Margin.Top, Player.ActualWidth, Player.ActualHeight);
            Rect EnemyAgr = new Rect(EnemyModel.Margin.Left - 50, EnemyModel.Margin.Top - 50, 200, 200);
            EnemyHitBox = new Rect(EnemyModel.Margin.Left, EnemyModel.Margin.Top, EnemyModel.ActualWidth, EnemyModel.ActualHeight);
            enemyHPBar.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - enemyHPBar.ActualHeight, 0, 0);
            enemyHPBar.Text = EnemyHP.ToString();
            if (EnemyAgr.IntersectsWith(PlayerHitBox))
            {
                if (EnemyModel.Margin.Left < Player.Margin.Left)
                {
                    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                }
                if (EnemyModel.Margin.Left > Player.Margin.Left)
                {
                    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                }
                if (EnemyModel.Margin.Top < Player.Margin.Top)
                {
                    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                }
                if (EnemyModel.Margin.Top > Player.Margin.Top)
                {
                    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                }
            }
            else
            {
                if (EnemyHitBox.IntersectsWith(EnemyAgro))
                {
                    if (MoveS == "Up")
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                    }
                    if (MoveS == "RightUp")
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                    }
                    if (MoveS == "LeftUp")
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                    }
                    if (MoveS == "LeftDown")
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                    }
                    if (MoveS == "RightDown")
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                    }
                    if (MoveS == "Down")
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                    }
                    if (MoveS == "Left")
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                    }
                    if (MoveS == "Right")
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                    }
                }
                else
                {
                    if (EnemyModel.Margin.Top < EnemyAgro.Top)
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                    }
                    if (EnemyModel.Margin.Top > EnemyAgro.Top)
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                    }
                    if (EnemyModel.Margin.Left < EnemyAgro.Left)
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                    }
                    if (EnemyModel.Margin.Left > EnemyAgro.Left)
                    {
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                    }
                }

            }
        }
        public void Delete()
        {
            grid.Children.Remove(EnemyModel);
            grid.Children.Remove(enemyHPBar);
            EnemyAgr = new Rect(0, 0, 0, 0);
            EnemyAgro = new Rect(0, 0, 0, 0);
            
        }


    }



    }

