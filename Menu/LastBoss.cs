using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Reflection.Emit;

namespace Menu
{
    public class LastBoss
    {
        public int HP = 100, MaxHP = 100;
        public int Damage = 25;
        public double ChargeSpeed = 15, Speed = 2, friction = 0.85, SpeedX, SpeedY;
        double SpeedCoef;

        double Knockback = 50;
        public double ProjectileSpeed = 10, ProjectileRange = 500;
        public int ProjectileDamage = 25;

        bool EnemyGoLeft, EnemyGoRight, EnemyGoDown, EnemyGoUp;


        public Image BossModel = new Image()
        {
            Height = 120,
            Width = 120,
            Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookLeft.png", UriKind.Absolute)), //Изменить на босса
            Stretch = Stretch.Fill,
            Tag = "Enemy",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        BitmapImage ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Boss/BossLookLeft.png", UriKind.Absolute));
        BitmapImage ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Boss/BossLookLeft.png", UriKind.Absolute));
        BitmapImage ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Boss/BossGoLeft.png", UriKind.Absolute));
        BitmapImage ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Boss/BossLookRight.png", UriKind.Absolute));
        BitmapImage ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Boss/BossLookRight.png", UriKind.Absolute));
        BitmapImage ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Boss/BossGoRight.png", UriKind.Absolute));



        Bullets bullet1, bullet2, bullet3, bullet4;

        Player player;
        Grid grid;

        
        public Rect BossHitBox;
        public Rect BossDamageBox;

        Random ran = new Random();

        bool IsShoot, IsCharge, IsBobm, IsSpawned, IsSpawned2;
        int ShootCd = 2, ChargeCd = 5, ShootDir = 0; // Если ShootDir == 0, то стреляет вверх, вниз, влево, вправо, если нет, то по диагонали 
        int ChargeRoll;

        DispatcherTimer AbilityShootingTimer, MovementTimer, AbilityChargeTimer, AnimationTimer;

        int timerAbilityShooting = 0;
        int timerAbilityCharge = 0;
        int timer = 0;

        BossRoom Bossroom;

        public LastBoss(Player player, Grid grid, BossRoom Bossroom) 
        {
            this.player = player;
            this.grid = grid;
            this.Bossroom = Bossroom;
            BossModel.Height = player.PlayerModel.Height * 5;
            BossModel.Width = player.PlayerModel.Width * 5;
            Knockback *= player.Scaling;

            ProjectileRange *= player.Scaling;
            ProjectileSpeed *= player.Scaling;

            MovementTimer = new DispatcherTimer();
            MovementTimer.Interval = TimeSpan.FromMilliseconds(20);
            MovementTimer.Tick += Movement;
            MovementTimer.Start();

            AbilityChargeTimer = new DispatcherTimer();
            AbilityChargeTimer.Interval = TimeSpan.FromSeconds(1);
            AbilityChargeTimer.Tick += AbilityCharging;
            AbilityChargeTimer.Start();

            AbilityShootingTimer = new DispatcherTimer();
            AbilityShootingTimer.Interval = TimeSpan.FromSeconds(1);
            AbilityShootingTimer.Tick += AbilityShooting;
            AbilityShootingTimer.Start();

            AnimationTimer = new DispatcherTimer();
            AnimationTimer.Interval = TimeSpan.FromMilliseconds(300);
            AnimationTimer.Tick += BossAnimation;
            AnimationTimer.Start();

        }

        private void BossAnimation(object sender, EventArgs e)
        {
            timer += 1;

            if (BossModel != null && player.IsEnemyFrozen == false)
            {
                if (IsCharge == false)
                {
                    if (EnemyGoLeft == true)
                    {
                        switch (timer % 3)
                        {
                            case 2:
                                BossModel.Source = ModelLookLeft;
                                break;
                            case 1:
                                BossModel.Source = ModelGoLeft;
                                break;
                            case 0:
                                BossModel.Source = ModelLookLeft2;
                                break;
                        }
            
                    }
            
                    if (EnemyGoRight == true)
                    {
                        switch (timer % 3)
                        {
                            case 2:
                                BossModel.Source = ModelLookRight;
                                break;
                            case 1:
                                BossModel.Source = ModelGoRight;
                                break;
                            case 0:
                                BossModel.Source = ModelLookRight2;
                                break;
                        }
                    }
                }
            }
        }

        private void AbilityShooting(object sender, EventArgs e)
        {
            timerAbilityShooting += 1;
          
            if (timerAbilityShooting >= ShootCd && IsShoot == false && IsCharge == false)
            {
                IsShoot = true;
                timerAbilityShooting = 0;
            }
        }

        double PlayerX, PlayerY, BossX, BossY;

        int Costil = 0;

        private void AbilityCharging(object sender, EventArgs e)
        {
            timerAbilityCharge += 1;

            if (timerAbilityCharge >= ChargeCd && ran.Next(1, 6) == 1 && IsCharge == false)
            {
                Speed += 15;
                PlayerX = player.PlayerModel.Margin.Left;
                PlayerY = player.PlayerModel.Margin.Top;
                IsCharge = true;
                timerAbilityCharge = 0;
            }
            //if (IsCharge)
            //{
            //    Costil += 1;
            //}
        }

        private void Movement(object sender, EventArgs e)
        {

            BossHitBox = new Rect(BossModel.Margin.Left - BossModel.Width * 0.1, BossModel.Margin.Top + BossModel.Height / 2, BossModel.Width * 0.8, BossModel.Height / 2);
            BossDamageBox = new Rect(BossModel.Margin.Left, BossModel.Margin.Top, BossModel.Width, BossModel.Height);

            if (IsCharge == false)
            {
                SpeedCoef = Speed / Math.Sqrt(Math.Pow(player.PlayerModel.Margin.Left - BossModel.Margin.Left, 2) + Math.Pow(player.PlayerModel.Margin.Top - BossModel.Margin.Top, 2));

                BossModel.Margin = new Thickness(BossModel.Margin.Left + (player.PlayerModel.Margin.Left - BossModel.Margin.Left) * SpeedCoef, BossModel.Margin.Top + (player.PlayerModel.Margin.Top - BossModel.Margin.Top) * SpeedCoef, 0, 0);

                if (player.PlayerHitBox.IntersectsWith(BossDamageBox))
                {
                    if (player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 < BossModel.Margin.Left + BossModel.ActualWidth / 2)
                    {
                        player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left - Knockback, player.PlayerModel.Margin.Top, 0, 0);
                    }

                    if (player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 > BossModel.Margin.Left + BossModel.ActualWidth / 2)
                    {
                        player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left + Knockback, player.PlayerModel.Margin.Top, 0, 0);
                    }

                    if (player.PlayerModel.Margin.Top < BossModel.Margin.Top - BossModel.ActualHeight / 2)
                    {
                        player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top - Knockback, 0, 0);
                    }

                    if (player.PlayerModel.Margin.Top > BossModel.Margin.Top - BossModel.ActualHeight / 2)
                    {
                        player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top + Knockback, 0, 0);
                    }
                    player.PlayerHP -= Damage;
                }

                if (player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 - (BossModel.Margin.Left + BossModel.Width / 2) > 0)
                {
                    //SpeedX += Speed;
                    EnemyGoLeft = false;
                    EnemyGoRight = true;
                }
                else if (player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 - (BossModel.Margin.Left + BossModel.Width / 2) < 0)
                {
                    //SpeedX -= Speed;
                    EnemyGoLeft = true;
                    EnemyGoRight = false;
                }

               //else
               //{
               //    SpeedX = 0;
               //    //EnemyGoRight = false;
               //    //EnemyGoLeft = true;
               //}
               //
               //if (player.PlayerModel.Margin.Top + player.PlayerModel.Height - (BossModel.Margin.Top + BossModel.Height) > 0)
               //{
               //    SpeedY += Speed;
               //}
               //else if (player.PlayerModel.Margin.Top + player.PlayerModel.Height - (BossModel.Margin.Top + BossModel.Height) < 0)
               //{
               //    SpeedY -= Speed;
               //}
               //
               //else
               //{
               //    SpeedY = 0;
               //}

                if (HP <= MaxHP * 3 / 4 && IsSpawned == false)
                {
                    Enemy enem1 = new Enemy(player, grid, "BossStandart");
                    Enemy enem2 = new Enemy(player, grid, "BossStandart");
                    enem1.EnemyModel.Margin = new Thickness(BossModel.Margin.Left - enem1.EnemyModel.Width * 2, BossModel.Margin.Top + BossModel.Height - enem1.EnemyModel.Height, 0 ,0);
                    enem2.EnemyModel.Margin = new Thickness(BossModel.Margin.Left + BossModel.Width + enem2.EnemyModel.Width * 2, BossModel.Margin.Top + BossModel.Height - enem2.EnemyModel.Height, 0, 0);
                    grid.Children.Add(enem1.EnemyModel); grid.Children.Add(enem2.EnemyModel);
                    Bossroom.engine.EnemyList.Add(enem1); Bossroom.engine.EnemyList.Add(enem2);
                    IsSpawned = true;
                }

                if (HP <= MaxHP / 2 && IsSpawned2 == false)
                {
                    IsSpawned = false;
                    IsSpawned2 = true;
                }

                if (IsShoot)
                {
                    if (ShootDir == 0)
                    {
                        bullet1 = new Bullets(BossModel.Margin.Left + BossModel.ActualWidth * 5 / 4, BossModel.Margin.Top + BossModel.ActualHeight / 4, "BossWeapon", "UpRight", this);
                        grid.Children.Add(bullet1.BulletModelIm);
                        Bossroom.engine.EnemyBulletsList.Add(bullet1);
                   
                        bullet2 = new Bullets(BossModel.Margin.Left - BossModel.ActualHeight / 2, BossModel.Margin.Top + BossModel.ActualHeight / 4, "BossWeapon", "UpLeft", this);
                        grid.Children.Add(bullet2.BulletModelIm);
                        Bossroom.engine.EnemyBulletsList.Add(bullet2);
                   
                        bullet3 = new Bullets(BossModel.Margin.Left - BossModel.ActualWidth / 2, BossModel.Margin.Top + BossModel.ActualHeight / 2 , "BossWeapon", "DownLeft", this);
                        grid.Children.Add(bullet3.BulletModelIm);
                        Bossroom.engine.EnemyBulletsList.Add(bullet3);
                   
                        bullet4 = new Bullets(BossModel.Margin.Left + BossModel.ActualWidth * 5 / 4, BossModel.Margin.Top + BossModel.Height / 2, "BossWeapon", "DownRight", this);
                        grid.Children.Add(bullet4.BulletModelIm);
                        Bossroom.engine.EnemyBulletsList.Add(bullet4);
                   
                        ShootDir = 1;
                    }
                   
                    else
                    {
                        bullet1 = new Bullets(BossModel.Margin.Left + BossModel.ActualWidth, BossModel.Margin.Top + BossModel.ActualHeight / 2, "BossWeapon", "Right", this);
                        grid.Children.Add(bullet1.BulletModelIm);
                        Bossroom.engine.EnemyBulletsList.Add(bullet1);
                   
                        bullet2 = new Bullets(BossModel.Margin.Left, BossModel.Margin.Top + BossModel.ActualHeight / 2, "BossWeapon", "Left", this);
                        grid.Children.Add(bullet2.BulletModelIm);
                        Bossroom.engine.EnemyBulletsList.Add(bullet2);
                   
                        bullet3 = new Bullets(BossModel.Margin.Left + BossModel.ActualWidth / 2, BossModel.Margin.Top, "BossWeapon", "Up", this);
                        grid.Children.Add(bullet3.BulletModelIm);
                        Bossroom.engine.EnemyBulletsList.Add(bullet3);
                   
                        bullet4 = new Bullets(BossModel.Margin.Left + BossModel.ActualWidth / 2, BossModel.Margin.Top + BossModel.Height, "BossWeapon", "Down", this);
                        grid.Children.Add(bullet4.BulletModelIm);
                        Bossroom.engine.EnemyBulletsList.Add(bullet4);
                   
                        ShootDir = 0;
                    }


                    IsShoot = false;
                }
            }

            if (IsCharge)
            {
                //BossModel.Margin = new Thickness(BossModel.Margin.Left + (PlayerX - BossX) / Speed, BossModel.Margin.Top + (PlayerY - BossY) / Speed, 0, 0);
                
                SpeedCoef = Speed / Math.Sqrt(Math.Pow(PlayerX - BossModel.Margin.Left, 2) + Math.Pow(PlayerY - BossModel.Margin.Top, 2));
                BossModel.Margin = new Thickness(BossModel.Margin.Left + (PlayerX - BossModel.Margin.Left) * SpeedCoef, BossModel.Margin.Top + (PlayerY - BossModel.Margin.Top) * SpeedCoef, 0, 0);
                Rect CharegeRect = new Rect(PlayerX - BossModel.Width / 4, PlayerY - BossModel.Height / 4, BossModel.Width, BossModel.Height);
                if(BossHitBox.IntersectsWith(CharegeRect))
                {
                    Speed -= 15;
                    IsCharge = false;
                    Costil = 0; 
                }
            }

        }
    }
}
