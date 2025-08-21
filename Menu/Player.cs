using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Menu
{
    public class Player
    {
        public double Scaling;

        public int PlayerMaxHP = 100, PlayerHP = 100; // Для бафов на хп и прверки того, что хп не регенится больше, чем возможно(100 по стандарту)
        public bool Immunity = false;

        public int Shield = 0, MaxShield = 0;

        public int Counterattack = 0; // Если тебя атакует, ты наносишь в ответ столько урона.

        public int ValueChange = 100; // Для провверки получния урона, для анимации получения урона
        public int PlayerHeight = 55, PlayerWidth = 67;
        public double BulletSpeedUp = 0, BulletSpeed;
        public int DamageUp = 0, Damage; // DamageUp - урон со статов, Damage - весь урон
        public int ShootRangeUp = 200, ShootRange; // ShootRangeUp - дальность со статов, ShootRange - общая дальность
        public int AmmoUp = 0, MaxAmmo = 0, CurrentAmmo; // AmmoUp - доп. патроны в обойме со статов, MaxAmmo - максимальное кол-во патронов в обойме, CurrentAmmo - текущее кол-во патронов.
        public int Bombs = 3;
        public double KnockBack = 40;
        public double ReloadTimeUp = 0, ReloadTime = 0; //
        public double ShootSpeedUp = 0, ShootSpeed = 0;
        public bool IsReloading = false;
        public bool IsShoot = false;
        public bool IsKeyUp = false;

        public int Coins = 0;

        public bool IsDamageTaken = false;

        public int PlayerWeaponSkin = 0;

        public bool IsEnemyFrozen = false;

        public int Level = 0; //Число пройденных пещер

        public double PlayerCurrentSpeed = 1.1; // Переменная запоминающая скорость персонажа
        public double Speed = 1.1, PlayerSpeedX, PlayerSpeedY, friction = 0.77;
        public double DamageReduction; // Броня


        public Weapons weapon, weapon2;         //оружие
        public Rect PlayerHitBox;
        public Rect PlayerKnockBackHitBox;
        //public Rectangle PlayerModel;

        static BitmapImage PlayerView = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/Standart/PlayerModelFront.png", UriKind.Absolute));
        public BitmapImage PlayerViewLeft = PlayerView, PlayerViewRight = PlayerView;
        BitmapImage PlayerTakeDamage = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/Standart/PlayerTakeDamage.png", UriKind.Absolute));

        public Image PlayerModel = new Image
            {
                Source = PlayerView,
                Height = 80,
                Width = 100,
                Stretch = Stretch.Fill,
                Tag = "player",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

        DispatcherTimer TakeDamageAnimationTimer = new DispatcherTimer();
        DispatcherTimer ImmunityTimer = new DispatcherTimer();

        public Player()
        {
            DispatcherTimer timer = new DispatcherTimer();

            TakeDamageAnimationTimer.Interval = TimeSpan.FromMilliseconds(400);
            TakeDamageAnimationTimer.Tick += TakeDamageAnimation;

            ImmunityTimer.Interval = TimeSpan.FromSeconds(1);
            ImmunityTimer.Tick += ImmunityTime;

            timer.Tick += Correction;
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Start();



            Scaling = App.Current.Windows[0].ActualHeight / 1080;
            KnockBack *= Scaling;
            Speed *= Scaling;
            weapon = new Weapons("Pow", this);
            weapon2 = weapon;
        }


        int timerImmunity = 0; 
        private void ImmunityTime(object sender, EventArgs e)
        {
            timerImmunity += 1;
            if (timerImmunity >= 2)
            {
                Immunity = false;
                ImmunityTimer.Stop();
            }
            
        }

        private void TakeDamageAnimation(object sender, EventArgs e)
        {
            PlayerModel.Source = PlayerViewLeft;
            IsDamageTaken = false;
            TakeDamageAnimationTimer.Stop();     
        }

        //public void DamageCheck(Rect enemy)
        //{
        //    
        //    if (enemy.IntersectsWith(PlayerHitBox))
        //    {
        //        PlayerHP -= 10;
        //    }
        //
        //}
        private void Correction(object sender, EventArgs e)
        {
            if (weapon != null)
            {
                MaxAmmo = AmmoUp + weapon.AmmoCount;
                Damage = weapon.Damage + DamageUp; // Переделать, ведь мы можем получать баф на дэмэдеж
                ShootRange = weapon.BulletRange + ShootRangeUp;
                ReloadTime = weapon.ReloadTime - ReloadTimeUp; // добваить условие, если не равно нулю --- Или переделать под умножение
                ShootSpeed = weapon.ShootSpeed - ShootSpeedUp;
                BulletSpeed = weapon.BulletSpeed + BulletSpeedUp;
            }

            if (Immunity)
            {
                ImmunityTimer.Start();
            }

            PlayerCurrentSpeed = Speed;

            if (weapon2 != weapon)
            {
                
                if(weapon.name == "Pistol")
                {
                    PlayerViewLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/WithWeapon/Pistol/PlayerLookLeft.png", UriKind.Absolute));
                    PlayerViewRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/WithWeapon/Pistol/PlayerLookRight.png", UriKind.Absolute));
                }
                if (weapon.name == "ShotGun")
                {
                    PlayerViewLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/WithWeapon/ShotGun/PlayerLookLeft.png", UriKind.Absolute));
                    PlayerViewRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/WithWeapon/ShotGun/PlayerLookRight.png", UriKind.Absolute));
                }
                if (weapon.name == "Rifle")
                {
                    PlayerViewLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/WithWeapon/Rifle/PlayerLookLeft.png", UriKind.Absolute));
                    PlayerViewRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/WithWeapon/Rifle/PlayerLookRight.png", UriKind.Absolute));
                }
                if (weapon.name == "PlazmaGun")
                {
                    PlayerViewLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/WithWeapon/BFG/PlayerLookLeft.png", UriKind.Absolute));
                    PlayerViewRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Player/WithWeapon/BFG/PlayerLookRight.png", UriKind.Absolute));
                }
                weapon2 = weapon;
            }

            if (PlayerHP > PlayerMaxHP)
            {
                PlayerHP = PlayerMaxHP;
            }

            if (Shield < 0)
            {
                Shield = 0;
            }

            if (CurrentAmmo > MaxAmmo)
            {
                CurrentAmmo = MaxAmmo;
            }
            if (PlayerHP < ValueChange)
            {
                PlayerModel.Source = PlayerTakeDamage;
                ValueChange = PlayerHP;
                IsDamageTaken = true;
                TakeDamageAnimationTimer.Start();
            }
            if (PlayerHP > ValueChange)
            {
                ValueChange = PlayerHP;
            }

            PlayerModel.Width = PlayerWidth * Scaling;
            PlayerModel.Height = PlayerHeight * Scaling;
            PlayerHitBox = new Rect(PlayerModel.Margin.Left, PlayerModel.Margin.Top, PlayerWidth, PlayerHeight);
            //PlayerKnockBackHitBox = new Rect(PlayerModel.Margin.Left + PlayerModel.Width * 0.15, PlayerModel.Margin.Top + PlayerModel.Height * 0.2, PlayerModel.Width * 0.7, PlayerModel.Height * 0.6);
        }

        public void PlayerModelCreator()
        {
            PlayerModel = new Image
            {
                Source = PlayerView,
                Height = 80,
                Width = 100,
                Stretch = Stretch.Fill,
                Tag = "player",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
        }

        public void PlayerSpeedCorrection()
        {
            Speed = PlayerCurrentSpeed;
        }
    }
}
