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
using System.Runtime.Remoting.Activation;
using System.Threading;
using System.IO.Packaging;

namespace Menu
{
    public class Engine
    {
        public bool GoUp = false, GoDown = false, GoLeft = false, GoRight = false;
        public bool ShootUp, ShootDown, ShootLeft, ShootRight;
        public double MaxSpeedX = 0, MaxSpeedY = 0;
        public bool IsTradeOpen = false;
        
        public bool PlaceBomb = false;
        public int PlaceBombDelay = 1;
        
        public bool Interaction = false;
        public int HP = 100, Score = 0;
        public Player player;

        int FrozenCounter = 0;

        public int ReloadTicks = 0, ShootTicks = 0;

        public double InterfaceCoordX = 0, InterfaceCoordY = 0;

        public List<Enemy> EnemyList = new List<Enemy>();
        public List<Enemy> EnemyDel = new List<Enemy>();
        
        List<Image> ImageToDel = new List<Image>();
        List<Rectangle> RectToDel = new List<Rectangle>();
        
        List<Bullets> BulletsList = new List<Bullets>();
        public List<Bullets> EnemyBulletsList = new List<Bullets>();

        public List<Rectangle> RoomObjectsList = new List<Rectangle>();

        BitmapImage ChestClosed = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Chest/Chest_Closed.png", UriKind.Absolute));
        BitmapImage ChestOpened = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Chest/Chest_Opened.png", UriKind.Absolute));

        Rectangle PlayerCheck = new Rectangle
        {
            Stroke = Brushes.White,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,

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

        Label HPBar, Stats, Reloading, AmmoBar;

        Grid grid;
        Caves cave;

        Window GameScreen;

        public NPC Trader;

        DispatcherTimer ReloadTimer;
        DispatcherTimer AbilityTimer;
        DispatcherTimer BombDelayTimer;
        
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
            ReloadTimer = new DispatcherTimer();
            AbilityTimer = new DispatcherTimer();
            BombDelayTimer = new DispatcherTimer();

            BombDelayTimer.Tick += BombDelaySeconds;
            BombDelayTimer.Interval = TimeSpan.FromSeconds(1);
            
            AbilityTimer.Tick += AbilitiesCoolDown;
            AbilityTimer.Interval = TimeSpan.FromSeconds(1);
            
            ReloadTimer.Tick += Counter;
            ReloadTimer.Interval = TimeSpan.FromMilliseconds(100);
            //ReloadTimer.Start();
            
            damageCheck.Tick += Damage_Check;
            damageCheck.Interval = TimeSpan.FromMilliseconds(500);
            damageCheck.Start();
            
            BulletMovement.Tick += Bullet_Movement;
            BulletMovement.Tick += Bullet;
            BulletMovement.Interval = TimeSpan.FromMilliseconds(20);
            BulletMovement.Start();

            PlayerMovement.Tick += InterfaceUpdater;
            PlayerMovement.Tick += Player_Movement;
            PlayerMovement.Interval = TimeSpan.FromMilliseconds(1);
            PlayerMovement.Start();

            PlayerCheck.Height = player.PlayerHeight * 3 / 4;
            PlayerCheck.Width = player.PlayerModel.Width;
           // grid.Children.Add(PlayerCheck);

            InterfaceCretor();
        }

        BossRoom bossRoom;
        LastBoss Boss;
        public Engine(Player player, Grid grid, BossRoom bossRoom, LastBoss Boss)
        {
            this.Boss = Boss;
            this.bossRoom = bossRoom;
            this.GameScreen = bossRoom.GameScreen;
            this.player = player;
            this.grid = grid;

            GameScreen.KeyDown += KeyIsDown;
            GameScreen.KeyUp += KeyIsUp;

            DispatcherTimer damageCheck = new DispatcherTimer();
            DispatcherTimer PlayerMovement = new DispatcherTimer();
            DispatcherTimer BulletMovement = new DispatcherTimer();
            ReloadTimer = new DispatcherTimer();
            AbilityTimer = new DispatcherTimer();
            BombDelayTimer = new DispatcherTimer();

            BombDelayTimer.Tick += BombDelaySeconds;
            BombDelayTimer.Interval = TimeSpan.FromSeconds(1);

            AbilityTimer.Tick += AbilitiesCoolDown;
            AbilityTimer.Interval = TimeSpan.FromSeconds(1);

            ReloadTimer.Tick += Counter;
            ReloadTimer.Interval = TimeSpan.FromMilliseconds(100);
            //ReloadTimer.Start();

            damageCheck.Tick += Damage_Check;
            damageCheck.Interval = TimeSpan.FromMilliseconds(500);
            damageCheck.Start();

            BulletMovement.Tick += Bullet_Movement;
            BulletMovement.Tick += Bullet;
            BulletMovement.Interval = TimeSpan.FromMilliseconds(20);
            BulletMovement.Start();

            PlayerMovement.Tick += InterfaceUpdater;
            PlayerMovement.Tick += Player_Movement;
            PlayerMovement.Interval = TimeSpan.FromMilliseconds(1);
            PlayerMovement.Start();

            InterfaceCretor();
        }

        public Engine(Window window, Grid grid)
        {
            this.grid = grid;
            this.GameScreen = window;
            player = new Player();
            Trader = new NPC(player);
            
            GameScreen.KeyDown += KeyIsDown;
            GameScreen.KeyUp += KeyIsUp;
            
            DispatcherTimer damageCheck = new DispatcherTimer();
            DispatcherTimer PlayerMovement = new DispatcherTimer();
            DispatcherTimer BulletMovement = new DispatcherTimer();
            ReloadTimer = new DispatcherTimer();
            BombDelayTimer = new DispatcherTimer();

            BombDelayTimer.Tick += BombDelaySeconds;
            BombDelayTimer.Interval = TimeSpan.FromSeconds(1);

            ReloadTimer.Tick += Counter;
            ReloadTimer.Interval = TimeSpan.FromMilliseconds(100);
            //ReloadTimer.Start();

            damageCheck.Tick += Damage_Check;
            damageCheck.Interval = TimeSpan.FromSeconds(1);
            damageCheck.Start();

            BulletMovement.Tick += Bullet;
            BulletMovement.Tick += Bullet_Movement;
            BulletMovement.Interval = TimeSpan.FromMilliseconds(20);
            BulletMovement.Start();

            PlayerMovement.Tick += InterfaceUpdater;
            PlayerMovement.Tick += Player_Movement;
            PlayerMovement.Interval = TimeSpan.FromMilliseconds(1);
            PlayerMovement.Start();

            InterfaceCretor();
        }

        public Engine(Window1 window, Grid grid, Player player, NPC Trader)
        {
            this.player = player;
            this.grid = grid;
            this.GameScreen = window.Windows;
            this.Trader = Trader;
            
            GameScreen.KeyDown += KeyIsDown;
            GameScreen.KeyUp += KeyIsUp;
            
            DispatcherTimer damageCheck = new DispatcherTimer();
            DispatcherTimer PlayerMovement = new DispatcherTimer();
            DispatcherTimer BulletMovement = new DispatcherTimer();
            ReloadTimer = new DispatcherTimer();
            BombDelayTimer = new DispatcherTimer();

            BombDelayTimer.Tick += BombDelaySeconds;
            BombDelayTimer.Interval = TimeSpan.FromSeconds(1);

            ReloadTimer.Tick += Counter;
            ReloadTimer.Interval = TimeSpan.FromMilliseconds(100);
            //ReloadTimer.Start();

            damageCheck.Tick += Damage_Check;
            damageCheck.Interval = TimeSpan.FromSeconds(1);
            damageCheck.Start();

            BulletMovement.Tick += Bullet;
            BulletMovement.Tick += Bullet_Movement;
            BulletMovement.Interval = TimeSpan.FromMilliseconds(20);
            BulletMovement.Start();

            PlayerMovement.Tick += InterfaceUpdater;
            PlayerMovement.Tick += Player_Movement;
            PlayerMovement.Interval = TimeSpan.FromMilliseconds(1);
            PlayerMovement.Start();

            InterfaceCretor();


        }


        public void Grid_Update(Grid Gridik)
        {
            grid = Gridik;
        }

        private void Damage_Check(object sender, EventArgs e)
        {
            foreach (Enemy x in EnemyList)
            {  
                if (player.PlayerHitBox.IntersectsWith(x.EnemyHitBox))
                {
                    if (player.Immunity == false)
                    {
                        if (x.EnemyType == "Range")
                        {

                            player.PlayerHP = player.PlayerHP + player.Shield - 10;
                            player.Shield -= 10;

                        }
                        else
                        {

                            player.PlayerHP = player.PlayerHP + player.Shield - x.Damage;
                            player.Shield -= x.Damage;
                        }

                        x.EnemyHP -= player.Counterattack;
                        if (player.Counterattack > 0)
                        {
                            x.Knockbacking();
                        }
                    }
                    
                    
                    //x.EnemyModel.Source =
                    //x.Attacking = true;
                }
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

        private void Bullet(object sender, EventArgs e)
        {
            if (player.weapon != null && player.IsReloading == false && player.IsShoot == false && player.weapon.name != "Pow")
            {
                if (ShootRight)
                {
                    if (player.weapon.name == "ShotGun" && player.CurrentAmmo > 1)
                    {
                        Bullets bulletUp = new Bullets(player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth, player.PlayerModel.Margin.Top + player.PlayerModel.ActualHeight / 5, player.weapon.name, "Right");
                        grid.Children.Add(bulletUp.BulletModel);
                        BulletsList.Add(bulletUp);
                        player.CurrentAmmo -= 1;
                    }
                    Bullets bullet = new Bullets(player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth, player.PlayerModel.Margin.Top + player.PlayerModel.ActualHeight / 2, player.weapon.name, "Right");
                    grid.Children.Add(bullet.BulletModel);
                    BulletsList.Add(bullet);
                    player.CurrentAmmo -= 1;
                    player.IsShoot = true;
                    ReloadTimer.Start();
                }

                if (ShootLeft)
                {
                    if (player.weapon.name == "ShotGun" && player.CurrentAmmo > 1)
                    {
                        Bullets bulletUp = new Bullets(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top + player.PlayerModel.ActualHeight / 5, player.weapon.name, "Left");
                        grid.Children.Add(bulletUp.BulletModel);
                        BulletsList.Add(bulletUp);
                        player.CurrentAmmo -= 1;
                    }
                    Bullets bullet = new Bullets(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top + player.PlayerModel.ActualHeight / 2, player.weapon.name, "Left");
                    grid.Children.Add(bullet.BulletModel);
                    BulletsList.Add(bullet);
                    player.CurrentAmmo -= 1;
                    player.IsShoot = true;
                    ReloadTimer.Start();
                }

                if (ShootUp)
                {
                    if (player.weapon.name == "ShotGun" && player.CurrentAmmo > 1)
                    {
                        Bullets bulletUp = new Bullets(player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth / 4, player.PlayerModel.Margin.Top, player.weapon.name, "Up");
                        grid.Children.Add(bulletUp.BulletModel);
                        BulletsList.Add(bulletUp);
                        player.CurrentAmmo -= 1;
                    }
                    Bullets bullet = new Bullets(player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth / 2, player.PlayerModel.Margin.Top, player.weapon.name, "Up");
                    (bullet.BulletModel.Width, bullet.BulletModel.Height) = (bullet.BulletModel.Height, bullet.BulletModel.Width);
                    grid.Children.Add(bullet.BulletModel);
                    BulletsList.Add(bullet);
                    player.CurrentAmmo -= 1;
                    player.IsShoot = true;
                    ReloadTimer.Start();
                }

                if (ShootDown)
                {
                    if (player.weapon.name == "ShotGun" && player.CurrentAmmo > 1)
                    {
                        Bullets bulletUp = new Bullets(player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth * 3 / 4, player.PlayerModel.Margin.Top + player.PlayerModel.ActualHeight, player.weapon.name, "Down");
                        grid.Children.Add(bulletUp.BulletModel);
                        BulletsList.Add(bulletUp);
                        player.CurrentAmmo -= 1;
                    }

                    Bullets bullet = new Bullets(player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth / 2, player.PlayerModel.Margin.Top + player.PlayerModel.Height, player.weapon.name, "Down");
                    (bullet.BulletModel.Width, bullet.BulletModel.Height) = (bullet.BulletModel.Height, bullet.BulletModel.Width);
                    grid.Children.Add(bullet.BulletModel);
                    BulletsList.Add(bullet);
                    player.CurrentAmmo -= 1;
                    player.IsShoot = true;
                    ReloadTimer.Start();
                }

                if (player.CurrentAmmo <= 0 && player.IsReloading == false)
                {
                    player.IsReloading = true;
                    ReloadTimer.Start();
                }
            }

            //foreach (Enemy x in EnemyList)
            //{
            //    if (x.EnemyType == "Range")
            //    {
            //        if (x.AttackRight)
            //        {
            //            Bullets bullet = new Bullets(x.EnemyModel.Margin.Left + x.EnemyModel.ActualWidth, x.EnemyModel.Margin.Top + x.EnemyModel.ActualHeight / 2, x.weapon.name, "Right", x);
            //            grid.Children.Add(bullet.BulletModel);
            //            EnemyBulletsList.Add(bullet);
            //            //x.Attacking = false;
            //        }
            //        if (x.AttackLeft)
            //        {
            //            Bullets bullet = new Bullets(x.EnemyModel.Margin.Left, x.EnemyModel.Margin.Top + x.EnemyModel.ActualHeight / 2, x.weapon.name, "Left", x);
            //            grid.Children.Add(bullet.BulletModel);
            //            EnemyBulletsList.Add(bullet);
            //            //x.Attacking = false;
            //        }
            //        if (x.AttackUp)
            //        {
            //            Bullets bullet = new Bullets(x.EnemyModel.Margin.Left + x.EnemyModel.ActualWidth / 2, x.EnemyModel.Margin.Top, x.weapon.name, "Up", x);
            //            (bullet.BulletModel.Width, bullet.BulletModel.Height) = (bullet.BulletModel.Height, bullet.BulletModel.Width);
            //            grid.Children.Add(bullet.BulletModel);
            //            EnemyBulletsList.Add(bullet);
            //            //x.Attacking = false;
            //        }
            //        if (x.AttackDown)
            //        {
            //            Bullets bullet = new Bullets(x.EnemyModel.Margin.Left + x.EnemyModel.ActualWidth / 2, x.EnemyModel.Margin.Top + x.EnemyModel.ActualHeight, x.weapon.name, "Down", x);
            //            (bullet.BulletModel.Width, bullet.BulletModel.Height) = (bullet.BulletModel.Height, bullet.BulletModel.Width);
            //            grid.Children.Add(bullet.BulletModel);
            //            EnemyBulletsList.Add(bullet);
            //            //x.Attacking = false;
            //        }
            //    };
            //}
        }

        public void Counter(object sender, EventArgs e)
        {
            if (player.IsShoot == true && player.IsReloading == false)
            {
                ShootTicks += 1;
                if (ShootTicks == player.ShootSpeed)
                {
                    player.IsShoot = false;
                    ShootTicks = 0;
                    ReloadTimer.Stop();
                }
            }

            if (player.IsReloading == true)
            {
                ReloadTicks += 1;
                if (ReloadTicks == player.ReloadTime * 10)
                {
                    player.CurrentAmmo = player.MaxAmmo;
                    player.IsReloading = false;
                    player.IsShoot = false;
                    ReloadTicks = 0;
                    ReloadTimer.Stop();
                }

            }
        } //Переделать, что таймер запускается только при выстреле или перезарядке.

        private void Bullet_Movement(object sender, EventArgs e)
        {

            foreach (Bullets x in BulletsList)
            {
                if (x.BulletModel != null)
                {
                    if (x.Direction == "Up")
                    {
                        x.BulletModel.Margin = new Thickness(x.BulletModel.Margin.Left, x.BulletModel.Margin.Top - player.BulletSpeed, 0, 0);

                        if (x.CoordY - player.ShootRange > x.BulletModel.Margin.Top)
                        {
                            grid.Children.Remove(x.BulletModel);
                            x.BulletModel = null;
                        }
                    }
                    if (x.Direction == "Down")
                    {
                        x.BulletModel.Margin = new Thickness(x.BulletModel.Margin.Left, x.BulletModel.Margin.Top + player.BulletSpeed, 0, 0);

                        if (x.CoordY + player.ShootRange < x.BulletModel.Margin.Top)
                        {
                            grid.Children.Remove(x.BulletModel);
                            x.BulletModel = null;
                        }
                    }
                    if (x.Direction == "Left")
                    {
                        x.BulletModel.Margin = new Thickness(x.BulletModel.Margin.Left - player.BulletSpeed, x.BulletModel.Margin.Top, 0, 0);

                        if (x.CoordX - player.ShootRange > x.BulletModel.Margin.Left)
                        {
                            grid.Children.Remove(x.BulletModel);
                            x.BulletModel = null;
                        }
                    }
                    if (x.Direction == "Right")
                    {
                        x.BulletModel.Margin = new Thickness(x.BulletModel.Margin.Left + player.BulletSpeed, x.BulletModel.Margin.Top, 0, 0);

                        if (x.CoordX + player.ShootRange < x.BulletModel.Margin.Left)
                        {
                            grid.Children.Remove(x.BulletModel);
                            x.BulletModel = null;
                        }
                    }

                    
                    if (x.BulletModel != null)
                    {
                        Rect BulletHB = new Rect(x.BulletModel.Margin.Left, x.BulletModel.Margin.Top, x.BulletModel.ActualWidth, x.BulletModel.ActualHeight);
                        foreach (Enemy y in EnemyList)
                        {
                            //y.Damege(BulletHB, player.Damage, x);
                            if (y.EnemyModel != null) // Мб переделать
                            {
                                bool isIntersect = false;
                                Rect EnemyHitBox = new Rect(y.EnemyModel.Margin.Left, y.EnemyModel.Margin.Top, y.EnemyModel.ActualWidth, y.EnemyModel.ActualHeight);

                                if (EnemyHitBox.IntersectsWith(BulletHB))
                                {
                                    isIntersect = true;
                                    if (player.weapon.name != "PlazmaGun")
                                    grid.Children.Remove(x.BulletModel);
                                    x.BulletModel = null;
                                    //check.Text += "1";
                                }
                                if (isIntersect && x.IsMakeDamage == false)
                                {
                                    y.EnemyHP -= player.Damage;
                                    x.IsMakeDamage = true;
                                }
                            }
                        }

                        if (Boss != null)
                        {
                            bool isIntersect = false;
                            if (Boss.BossHitBox.IntersectsWith(BulletHB))
                            {
                                isIntersect = true;
                                grid.Children.Remove(x.BulletModel);
                                x.BulletModel = null;
                                //check.Text += "1";
                            }
                            if (isIntersect && x.IsMakeDamage == false)
                            {
                                Boss.HP -= player.Damage;
                                x.IsMakeDamage = true;
                            }
                        }
                    }
                }
                
            }

            foreach (Bullets x in EnemyBulletsList)
            {
                if (x.BulletModelIm != null)
                {
                    if (x.Direction == "Up")
                    {
                        x.BulletModelIm.Margin = new Thickness(x.BulletModelIm.Margin.Left, x.BulletModelIm.Margin.Top - x.Boss.ProjectileSpeed, 0, 0);

                        if (x.CoordY - x.Boss.ProjectileRange > x.BulletModelIm.Margin.Top)
                        {
                            grid.Children.Remove(x.BulletModelIm);
                            x.BulletModelIm = null;
                        }
                    }

                    if (x.Direction == "Down")
                    {
                        x.BulletModelIm.Margin = new Thickness(x.BulletModelIm.Margin.Left, x.BulletModelIm.Margin.Top  + x.Boss.ProjectileSpeed, 0, 0);

                        if (x.CoordY + x.Boss.ProjectileRange < x.BulletModelIm.Margin.Top)
                        {
                            grid.Children.Remove(x.BulletModelIm);
                            x.BulletModelIm = null;
                        }
                    }
                    if (x.Direction == "Left")
                    {
                        x.BulletModelIm.Margin = new Thickness(x.BulletModelIm.Margin.Left - x.Boss.ProjectileSpeed, x.BulletModelIm.Margin.Top, 0, 0);

                        if (x.CoordX - x.Boss.ProjectileRange > x.BulletModelIm.Margin.Left)
                        {
                            grid.Children.Remove(x.BulletModelIm);
                            x.BulletModelIm = null;
                        }
                    }
                    if (x.Direction == "Right")
                    {
                        x.BulletModelIm.Margin = new Thickness(x.BulletModelIm.Margin.Left + x.Boss.ProjectileSpeed, x.BulletModelIm.Margin.Top, 0, 0);

                        if (x.CoordX + x.Boss.ProjectileRange < x.BulletModelIm.Margin.Left)
                        {
                            grid.Children.Remove(x.BulletModelIm);
                            x.BulletModelIm = null;
                        }
                    }

                    if (x.Direction == "UpRight")
                    {
                        x.BulletModelIm.Margin = new Thickness(x.BulletModelIm.Margin.Left + x.Boss.ProjectileSpeed / 2 , x.BulletModelIm.Margin.Top - x.Boss.ProjectileSpeed / 2, 0, 0);
                        if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.BulletModelIm.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.BulletModelIm.Margin.Left), 2)) >= x.Boss.ProjectileRange * 2)
                        {
                            grid.Children.Remove(x.BulletModelIm);
                            x.BulletModelIm = null;
                        }
                    }

                    if (x.Direction == "UpLeft")
                    {
                        x.BulletModelIm.Margin = new Thickness(x.BulletModelIm.Margin.Left  - x.Boss.ProjectileSpeed / 2, x.BulletModelIm.Margin.Top - x.Boss.ProjectileSpeed / 2, 0, 0);
                        if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.BulletModelIm.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.BulletModelIm.Margin.Left), 2)) >= x.Boss.ProjectileRange * 2)
                        {
                            grid.Children.Remove(x.BulletModelIm);
                            x.BulletModelIm = null;
                        }
                    }

                    if (x.Direction == "DownLeft")
                    {
                        x.BulletModelIm.Margin = new Thickness(x.BulletModelIm.Margin.Left - x.Boss.ProjectileSpeed / 2 , x.BulletModelIm.Margin.Top + x.Boss.ProjectileSpeed / 2, 0, 0);
                        if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.BulletModelIm.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.BulletModelIm.Margin.Left), 2)) >= x.Boss.ProjectileRange * 2)
                        {
                            grid.Children.Remove(x.BulletModelIm);
                            x.BulletModelIm = null;
                        }
                    }

                    if (x.Direction == "DownRight")
                    {
                        x.BulletModelIm.Margin = new Thickness(x.BulletModelIm.Margin.Left + x.Boss.ProjectileSpeed / 2, x.BulletModelIm.Margin.Top + x.Boss.ProjectileSpeed / 2, 0, 0);
                        if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.BulletModelIm.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.BulletModelIm.Margin.Left), 2)) >= x.Boss.ProjectileRange * 2)
                        {
                            grid.Children.Remove(x.BulletModelIm);
                            x.BulletModelIm = null;
                        }
                    }
                    

                    if (x.BulletModelIm != null)
                    {
                        Rect BulletHB = new Rect(x.BulletModelIm.Margin.Left, x.BulletModelIm.Margin.Top, x.BulletModelIm.ActualWidth, x.BulletModelIm.ActualHeight);

                        Rect PlayerHitBox = new Rect(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top, player.PlayerModel.Width, player.PlayerModel.Height);
                        bool isIntersect = false;

                        if (BulletHB.IntersectsWith(PlayerHitBox))
                        {
                            isIntersect = true;
                            grid.Children.Remove(x.BulletModelIm);
                            x.BulletModel = null;
                        }
                        if (isIntersect && x.IsMakeDamage == false)
                        {
                            player.PlayerHP -= x.Boss.ProjectileDamage;
                            x.IsMakeDamage = true;
                        }

                    }
                }
            }
        }

        private void Player_Movement(object sender, EventArgs e)
        {
            if (PlaceBomb && player.Bombs > 0)
            {
                player.Bombs--;
                PlaceBomb = false;
                BombDelayTimer.Start();
                Bombs bomb = new Bombs(this, "Player");
                bomb.BombModel.Margin = new Thickness(player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 - bomb.BombModel.Width / 2, player.PlayerModel.Margin.Top + player.PlayerModel.Height - bomb.BombModel.Height, 0, 0);
                grid.Children.Add(bomb.BombModel);
                PlaceBombDelay = 0;
            }

            if (Interaction)
            {
                if (GameScreen.Name == "Windows")
                {
                    if (player.PlayerHitBox.IntersectsWith(Trader.Trader_Intersaction) && Trader.IsShopOpen == false)
                    {
                        Trader.Shop_Menu();
                        Interaction = false;
                    }
                }
            }

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
                    player.PlayerModel.Source = player.PlayerViewRight;
                }
                if (GoLeft)
                {
                    player.PlayerSpeedX -= player.Speed;
                    player.PlayerModel.Source = player.PlayerViewLeft;
                }
            

            player.PlayerSpeedX *= player.friction;
            player.PlayerSpeedY *= player.friction;
            if (MaxSpeedX < Math.Abs(player.PlayerSpeedX))
            {
                MaxSpeedX = player.PlayerSpeedX;
            }
            if (MaxSpeedY < Math.Abs(player.PlayerSpeedY))
            {
                MaxSpeedY = player.PlayerSpeedY;
            }

            //PlayerCheck.Margin = new Thickness(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top + player.PlayerModel.Height / 4, 0, 0);
            player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left + player.PlayerSpeedX, player.PlayerModel.Margin.Top + player.PlayerSpeedY, 0, 0);
            if (GameScreen.Name == "Windows" )
            {
                if (player.PlayerModel.Margin.Left >= grid.Margin.Left + App.Current.Windows[0].Width / 2 - player.PlayerModel.Width / 2 && player.PlayerModel.Margin.Left <= grid.Width - App.Current.Windows[0].Width / 2 - player.PlayerModel.Width / 2)
                {
                    grid.Margin = new Thickness(0 - player.PlayerModel.Margin.Left + App.Current.Windows[0].Width / 2 - player.PlayerModel.Width / 2, grid.Margin.Top, 0, 0);
                }
                if (player.PlayerModel.Margin.Top >= grid.Margin.Top + App.Current.Windows[0].Height / 2 - player.PlayerModel.Height / 2 && player.PlayerModel.Margin.Top <=  grid.Height - App.Current.Windows[0].Height / 2 - player.PlayerModel.Height / 2)
                {
                    grid.Margin = new Thickness(grid.Margin.Left, 0 - player.PlayerModel.Margin.Top + App.Current.Windows[0].Height / 2 - player.PlayerModel.Height / 2, 0, 0);
                }
                
            }
            // Сделать только в главной комнате
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
                Rect PlayerHitBox = new Rect(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top + player.PlayerModel.Height, player.PlayerWidth, player.PlayerHeight);
                if (x is Rectangle && (string)x.Tag == "Border")
                {
                    Rect BoxHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.Width, x.Height);
                    if (BoxHitBox.IntersectsWith(PlayerHitBox))
                    {
                            
                        player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left - player.PlayerSpeedX, player.PlayerModel.Margin.Top - player.PlayerSpeedY, 0, 0);
                        //if (player.PlayerSpeedX > 0)
                        //{
                        //    player.PlayerSpeedX = player.PlayerModel.Margin.Left + player.PlayerModel.Width - x.Margin.Left;
                        //}
                        //else
                        //{
                        //    player.PlayerSpeedX = x.Margin.Left + x.Width - player.PlayerModel.Margin.Left;
                        //}
                        //if (player.PlayerSpeedY > 0)
                        //{
                        //    player.PlayerSpeedY = player.PlayerModel.Margin.Top + player.PlayerModel.Height - x.Margin.Top;
                        //}
                        //else
                        //{
                        //    player.PlayerSpeedX = x.Margin.Top + x.Height - player.PlayerModel.Margin.Top;
                        //}
                        player.PlayerSpeedX = 0; player.PlayerSpeedY = 0;
                        MaxSpeedX = 0; MaxSpeedY = 0;

                        //if (player.PlayerModel.Margin.Left + player.PlayerModel.Width > x.Margin.Left && player.PlayerModel.Margin.Left < x.Margin.Left + x.Width && player.PlayerModel.Margin.Top < x.Margin.Top && player.PlayerModel.Margin.Top + player.PlayerModel.Height > x.Margin.Top)
                        //{
                        //    if (player.PlayerModel.Margin.Top > x.Margin.Top)
                        //    {
                        //        player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, x.Margin.Top + x.Height, 0, 0);
                        //    }
                        //    if (player.PlayerModel.Margin.Top < x.Margin.Top)
                        //    {
                        //        player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, x.Margin.Top, 0, 0);
                        //    }
                        //    if (player.PlayerModel.Margin.Left < x.Margin.Left)
                        //    {
                        //        player.PlayerModel.Margin = new Thickness(x.Margin.Left, player.PlayerModel.Margin.Top,0,0);
                        //    }
                        //    if (player.PlayerModel.Margin.Left > x.Margin.Left)
                        //    {
                        //        player.PlayerModel.Margin = new Thickness(x.Margin.Left + x.Width, player.PlayerModel.Margin.Top, 0, 0);
                        //    }
                        //}
                    }
                }
            }

            foreach (var x in grid.Children.OfType<Image>())
            {
                Rect PlayerHitBox = new Rect(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top + player.PlayerModel.Height / 4, player.PlayerWidth, player.PlayerHeight * 3 / 4);
                if (x is Image && (string)x.Tag == "Collectables")
                {
                    Rect CollectHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.ActualWidth, x.ActualHeight);
                    if (PlayerHitBox.IntersectsWith(CollectHitBox))
                    {
                        if ((string)x.Name == "coin")
                        {
                            player.Coins += 1;
                            ImageToDel.Add(x);
                        }
                        if ((string)x.Name == "HealthUp")
                        {
                            player.PlayerMaxHP += 20;
                            player.PlayerHP += 20;
                            ImageToDel.Add(x);
                        }

                        if ((string)x.Name == "SpeedRun")
                        {
                            player.Speed += 0.25;
                            player.PlayerCurrentSpeed += 0.5;
                            ImageToDel.Add(x);
                        }

                        if ((string)x.Name == "RoseOfWind")
                        {
                            player.DamageUp += 1;
                            ImageToDel.Add(x);
                        }

                        if ((string)x.Name == "Healthy" && player.PlayerHP != player.PlayerMaxHP)
                        {
                            player.PlayerHP += 15;
                            ImageToDel.Add(x);
                        }

                        if ((string)x.Name == "Stop")
                        {
                            player.IsEnemyFrozen = true;
                            ImageToDel.Add(x);
                            AbilityTimer.Start();
                        }

                        if (x.Name == "Key")
                        {
                            player.IsKeyUp = true;
                            ImageToDel.Add(x);
                        }
                        if (x.Name == "Pistol")
                        {
                            player.weapon = new Weapons("Pistol", player);
                            ImageToDel.Add(x);
                        }
                        if (x.Name == "Bubble")
                        {
                            player.MaxShield += 10;
                            ImageToDel.Add(x);
                        }
                        if (x.Name == "Counterattack")
                        {
                            player.Counterattack += 1;
                            ImageToDel.Add(x);
                        }
                    }
                }
                if (x is Image && (string)x.Name == "Chest")
                {

                    Rect ObjHitBox = new Rect(x.Margin.Left, x.Margin.Top + x.Height / 2, x.Width, x.Height / 2);
                    if (PlayerHitBox.IntersectsWith(ObjHitBox))
                    {
                        if ((string)x.Tag == "Closed")
                        {
                            x.Tag = "Opened";
                            x.Source = ChestOpened;
                        }

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
                if (x is Image && (string)x.Tag == "CaveExit")
                {
                    Rect CaveDoorHiBox = new Rect(x.Margin.Left, x.Margin.Top, x.Width , x.Height );
                    if (Interaction && PlayerHitBox.IntersectsWith(CaveDoorHiBox) && player.IsKeyUp)
                    {
                        player.Level += 1;
                        Window1 window1 = new Window1(player);
                        cave.Close();
                        cave = null;
                        window1.ShowDialog();
                    }
                }
            }
        }

        private void AbilitiesCoolDown(object sender, EventArgs e)
        {
            if (player.IsEnemyFrozen)
            {
                FrozenCounter += 1;
                if (FrozenCounter == 5)
                {
                    player.IsEnemyFrozen = false;
                    FrozenCounter = 0;
                    AbilityTimer.Stop();
                }
            }
            else
            {
                FrozenCounter = 0;
            }
        }

        private void BombDelaySeconds(object sender, EventArgs e)
        {
            PlaceBombDelay += 1;
            BombDelayTimer.Stop();
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
                if (player.IsDamageTaken == false)
                {
                    player.PlayerModel.Source = player.PlayerViewRight;
                }   
            }
            if (e.Key == Key.A)
            {
                GoLeft = true;
                if (player.IsDamageTaken == false)
                {
                    player.PlayerModel.Source = player.PlayerViewLeft;
                }
            }

            if (e.Key == Key.E)
            {
                Interaction = true;
            }

            if (e.Key == Key.Q && PlaceBombDelay == 1)
            {
                PlaceBomb = true;
            }

            if (e.Key == Key.Up)
            {
                ShootUp = true; ShootDown = false; ShootLeft = false; ShootRight = false;
            }
            if (e.Key == Key.Down)
            {
                ShootDown = true; ShootUp = false;  ShootLeft = false; ShootRight = false;
            }
            if (e.Key == Key.Left)
            {
                if (player.IsDamageTaken == false)
                {
                    player.PlayerModel.Source = player.PlayerViewLeft;
                }
                ShootLeft = true; ShootUp = false; ShootDown = false; ShootRight = false;
            }
            if (e.Key == Key.Right)
            {
                if (player.IsDamageTaken == false)
                {
                    player.PlayerModel.Source = player.PlayerViewRight;
                }

                ShootRight = true; ShootUp = false; ShootDown = false; ShootLeft = false;
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

            if (e.Key == Key.E)
            {
                Interaction = false;
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

        private void InterfaceCretor()
        {
            HPBar = new Label
            {
                Height = 100,
                Width = 100,
                FlowDirection = FlowDirection.LeftToRight,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0),
                Background = Brushes.White,
            };
            AmmoBar = new Label
            {
                Height = 100,
                Width = 100,
                FlowDirection = FlowDirection.LeftToRight,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0),
                Background = Brushes.White,
            };
            Reloading = new Label
            {
                Height = 50,
                Width = 100,
                FlowDirection = FlowDirection.LeftToRight,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0),
                Background = Brushes.White,
            };

            Stats = new Label
            {
                Height = 100,
                Width = 100,
                FlowDirection = FlowDirection.LeftToRight,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0),
                Background = Brushes.White,
            };

            grid.Children.Add(HPBar); grid.Children.Add(AmmoBar); //grid.Children.Add(Reloading); 
            grid.Children.Add(Stats);

        }

        private void InterfaceUpdater(object sender, EventArgs e)
        {
            HPBar.Content = "HP:" + player.PlayerHP.ToString() + "/" + player.PlayerMaxHP + "\n" + "Shield:" + player.Shield.ToString() + "/" + player.MaxShield.ToString() + "\n" + "Ammo:" + player.CurrentAmmo.ToString() + "/" + player.MaxAmmo.ToString() + "\n" + "Bombs:" + player.Bombs.ToString();
            AmmoBar.Content = "Coins:" + player.Coins.ToString() + "\n" + "X:" + player.PlayerSpeedX.ToString() + "\n" + "y:" + player.PlayerSpeedY.ToString();
            Stats.Content = "Damage:" + player.Damage.ToString() + "\n" + "Speed:" + player.Speed.ToString() + "\n" + "ShootRange:" + player.ShootRange.ToString() + "\n" + "ShootSpeed" + Convert.ToString(10 / player.ShootSpeed) + "\n" + "ReloadTime:" + player.ReloadTime + "\n" + player.weapon.name + "\n" + "x" + player.PlayerModel.Margin.Left.ToString() + "\n" + "y" + player.PlayerModel.Margin.Top.ToString();
            if (cave != null)
            {
                HPBar.Margin = new Thickness(InterfaceCoordX - HPBar.Width - Stats.Width, InterfaceCoordY, 0, 0);
                AmmoBar.Margin = new Thickness(InterfaceCoordX - HPBar.Width - Stats.Width, InterfaceCoordY + HPBar.Height + 20, 0, 0);
                Stats.Margin = new Thickness(InterfaceCoordX - Stats.Width, InterfaceCoordY, 0, 0);
            }
            else
            {
                HPBar.Margin = new Thickness(grid.Margin.Left, grid.Margin.Top, 0, 0);
                AmmoBar.Margin = new Thickness(grid.Margin.Left, grid.Margin.Top + HPBar.Height + 20, 0, 0);
                Stats.Margin = new Thickness(grid.Margin.Left + HPBar.Width + 20, grid.Margin.Top, 0, 0);
            }
        }

        public void Explodes(Bombs bomb)
        {
            foreach (Enemy enem in EnemyList)
            {
                if (enem.EnemyModel != null)
                {
                    if (enem.EnemyHitBox.IntersectsWith(bomb.ExplodeRadius))
                    {
                        if (enem.EnemyModel.Margin.Left + enem.EnemyModel.Width / 2 - (bomb.BombModel.Margin.Left + bomb.BombModel.Width / 2) > 0)
                        {
                            //KnockbackX = bomb.Knockback;
                            enem.ZombieSpeedX += bomb.Knockback * 1.7;
                        }
                        else
                        {
                            enem.ZombieSpeedX -= bomb.Knockback * 1.7;
                        }

                        if (enem.EnemyModel.Margin.Top + enem.EnemyModel.Height - (bomb.BombModel.Margin.Top + bomb.BombModel.Height) > 0)
                        {
                            enem.ZombieSpeedY += bomb.Knockback * 1.7;
                        }
                        else
                        {
                            enem.ZombieSpeedY -= bomb.Knockback * 1.7;
                        }

                        if (enem.EnemyHP < bomb.Damage * 6)
                        {
                            enem.TakenDamage = 0;
                            enem.WillDead = true;
                        }
                        else
                        {
                            enem.TakenDamage = bomb.Damage;
                        }

                        enem.IsKnockbacking = true;
                        enem.KnockbackingTimer.Start();
                    }
                }
            }
            if (player.PlayerHitBox.IntersectsWith(bomb.ExplodeRadius))
            {
                if (player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 - (bomb.BombModel.Margin.Left + bomb.BombModel.Width / 2) > 0)
                {
                    //KnockbackX = bomb.Knockback;
                    player.PlayerSpeedX += bomb.Knockback;
                }
                else
                {
                    player.PlayerSpeedX -= bomb.Knockback;
                }
            
                if (player.PlayerModel.Margin.Top + player.PlayerModel.Height - (bomb.BombModel.Margin.Top + bomb.BombModel.Height) > 0)
                {
                    player.PlayerSpeedY += bomb.Knockback;
                }
                else
                {
                    player.PlayerSpeedY -= bomb.Knockback;
                }
                //player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left + KnockbackX, player.PlayerModel.Margin.Top + KnockbackY, 0, 0);
            }

            grid.Children.Remove(bomb.BombModel);
            bomb.BombModel = null;
            bomb = null;
        }
    }
}
