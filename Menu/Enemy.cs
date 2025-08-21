using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        public int AnimationType; // 0 - влево, 1 - вправо, 2 - Удар влево, 3 - удар вправо
        public double ZombieSpeed = 5, x, y;
        public double ZombieSpeedX = 0, ZombieSpeedY = 0;
        public double SpeedCoef;
        public int EnemyHP = 3, Damage = 10, TakenDamage; // Добавить урон для противников и урон ими получаемый;
        public double ProjectileRange = 400, ProjectileSpeed = 10, StopRange = 0;
        public int ProjectileDamage = 0;
        public Weapons weapon;

        public bool EnemyGoLeft, EnemyGoRight, Attacking;
        public bool EnemyGoUp, EnemyGoDown;
        public bool AttackRight, AttackLeft, AttackUp, AttackDown;

        public bool Killed = false;
        public bool IsDropped = false;
        public bool IsKnockbacking = false; // для отталкивания от взрыва
        public bool WillDead = false;

        public bool IsSpeedChange = false;
        public bool IsOut = false; //Проверка на выход за текстуры

        public Rect EnemyAgr, EnemyPosition, EnemyHitBox;
        Rect EnemyAgro;
        public double ModelWidth = 100, ModelHeight = 100;

        Player player;
        Grid grid;
        string MoveS;

        List<Bullets> bullets = new List<Bullets>();
        List<Rectangle> RectToDel = new List<Rectangle>();

        int timer = 0;
        int timerKnockbacking = 0;
        int timerSpecialAbility;

        Random ran = new Random();
        int AbilityCD;
        int AbilityTime;
        bool IsAbilityActive = false;

        Label enemyHPBar = new Label
        {
            Width = 20,
            Height = 25,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 0, 0, 0),
            Content = "",
            FontSize = 12,
            FlowDirection = FlowDirection.LeftToRight,
            Background = Brushes.White,
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

        public Image EnemyModel = new Image
        {
            Height = 65,
            Width = 60,
            Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookLeft.png", UriKind.Absolute)),
            Stretch = Stretch.Fill,
            Tag = "Enemy",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        Room room;

        public string EnemyType;

        DispatcherTimer Collision, AnimationTimer, ForAnimationTimer, SpecialAbilityTimer, RandomMovementTimer;
        public DispatcherTimer KnockbackingTimer;

        BitmapImage ModelGoLeft, ModelGoRight, ModelLookRight, ModelLookLeft, ModelLookLeft2, ModelLookRight2;

        Double PlayerX, PlayerY, ZombieX, ZombieY;

        public Enemy(Player player, Grid grid, Rect EnemyAgro, Room room, string Type)
        {
            EnemyType = Type;
            this.room = room;
            this.player = player;
            this.grid = grid;
            this.EnemyAgro = EnemyAgro;

            RandomMovementTimer = new DispatcherTimer();
            RandomMovementTimer.Tick += Enemy_Move;
            RandomMovementTimer.Interval = TimeSpan.FromSeconds(2);

            SpecialAbilityTimer = new DispatcherTimer();
            SpecialAbilityTimer.Tick += SpecialAbility;
            SpecialAbilityTimer.Interval = TimeSpan.FromSeconds(1);

            KnockbackingTimer = new DispatcherTimer();
            KnockbackingTimer.Tick += ExplodeKnockBacking;
            KnockbackingTimer.Interval = TimeSpan.FromMilliseconds(150);

            //ForAnimationTimer = new DispatcherTimer();
            //ForAnimationTimer.Tick += SecondsForAnimations;
            //ForAnimationTimer.Interval = TimeSpan.FromMilliseconds(300);
            //ForAnimationTimer.Start();

            AnimationTimer = new DispatcherTimer();
            AnimationTimer.Tick += ZombieAnimation;
            AnimationTimer.Interval = TimeSpan.FromMilliseconds(200);
            AnimationTimer.Start();

            Collision = new DispatcherTimer();
            Collision.Tick += CollusionCheck;
            Collision.Interval = TimeSpan.FromMilliseconds(100);
            Collision.Start();



            //EnemyMovement = new DispatcherTimer();
            //EnemyMovement.Tick += Enemy_Move;
            //EnemyMovement.Interval = TimeSpan.FromSeconds(2);
            //EnemyMovement.Start();

            //grid.Children.Add(check);
            grid.Children.Add(enemyHPBar);


            if (EnemyType == "Strong")
            {
                EnemyHP = player.Level * 3 + 8;
                ZombieSpeed = player.Level * 2 + 7;
                Damage = 15 + player.Level * 2;
                ModelHeight *= 1.2;
                ModelWidth *= 1.2;
                ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongGoLeft.png", UriKind.Absolute));
                ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongGoRight.png", UriKind.Absolute));
                ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongLookLeft.png", UriKind.Absolute));
                ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongLookRight.png", UriKind.Absolute));
                ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongLookLeft2.png", UriKind.Absolute));
                ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongLookRight2.png", UriKind.Absolute));
                EnemyModel.Source = ModelLookLeft;


            }
            if (EnemyType == "Standart")
            {
                Damage = player.Level * 3 + 10;
                EnemyHP = player.Level * 3 + 3;
                ZombieSpeed = player.Level * 2 + 10;
                ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieGoLeft.png", UriKind.Absolute));
                ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieGoRight.png", UriKind.Absolute));
                ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookLeft.png", UriKind.Absolute));
                ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookRight.png", UriKind.Absolute));
                ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookLeft2.png", UriKind.Absolute));
                ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookRight2.png", UriKind.Absolute));
                switch (player.Level)
                {
                    case 1:
                        ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl2/ZombieGoLeft.png", UriKind.Absolute));
                        ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl2/ZombieGoRight.png", UriKind.Absolute));
                        ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl2/ZombieLookLeft.png", UriKind.Absolute));
                        ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl2/ZombieLookRight.png", UriKind.Absolute));
                        ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl2/ZombieLookLeft.png", UriKind.Absolute));
                        ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl2/ZombieLookRight.png", UriKind.Absolute));
                        break;
                    case 2:
                        ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl3/ZombieGoLeft.png", UriKind.Absolute));
                        ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl3/ZombieGoRight.png", UriKind.Absolute));
                        ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl3/ZombieLookLeft.png", UriKind.Absolute));
                        ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl3/ZombieLookRight.png", UriKind.Absolute));
                        ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl3/ZombieLookLeft.png", UriKind.Absolute));
                        ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl3/ZombieLookRight.png", UriKind.Absolute));
                        break;
                    case 3:
                        ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl4/ZombieGoLeft.png", UriKind.Absolute));
                        ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl4/ZombieGoRight.png", UriKind.Absolute));
                        ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl4/ZombieLookLeft.png", UriKind.Absolute));
                        ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl4/ZombieLookRight.png", UriKind.Absolute));
                        ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl4/ZombieLookLeft.png", UriKind.Absolute));
                        ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/lvl4/ZombieLookRight.png", UriKind.Absolute));
                        break;
                }
            }

            if (EnemyType == "Blind")
            {
                Damage = player.Level * 3 + 15;
                EnemyHP = player.Level * 3 + 3;
                ZombieSpeed = player.Level * 2 + 8;
                ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieGoLeft.png", UriKind.Absolute));
                ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieGoRight.png", UriKind.Absolute));
                ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieLookLeft.png", UriKind.Absolute));
                ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieLookRight.png", UriKind.Absolute));
                ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieLookLeft2.png", UriKind.Absolute));
                ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieLookRight2.png", UriKind.Absolute));
                EnemyModel.Source = ModelLookLeft;
                Collision.Tick -= CollusionCheck;
                Collision.Tick += RangeBlindMovement;
                RandomMovementTimer.Interval = TimeSpan.FromSeconds(1);
            }

            if (EnemyType == "Aggressive")
            {
                EnemyHP = 3 + player.Level * 2;
                ZombieSpeed = player.Level * 2 + 4;
                Damage = player.Level * 4 + 15;
                AbilityCD = ran.Next(3, 6);
                AbilityTime = 2;
                SpecialAbilityTimer.Start();
                ModelHeight *= 0.9;
                ModelWidth *= 0.9;
                ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieGoLeft.png", UriKind.Absolute));
                ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieGoRight.png", UriKind.Absolute));
                ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieLookLeft.png", UriKind.Absolute));
                ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieLookRight.png", UriKind.Absolute));
                ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieLookLeft2.png", UriKind.Absolute));
                ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieLookRight2.png", UriKind.Absolute));
                EnemyModel.Source = ModelLookLeft;
            }

            if (EnemyType == "Range")
            {
                EnemyHP = player.Level + 1;
                ZombieSpeed = player.Level * 2 + 10;
                ProjectileRange = (player.Level * 100 + 400) * player.Scaling;
                Damage = player.Level * 3 + 15;
                ModelHeight *= 0.7;
                ModelWidth *= 0.7;
                weapon = new Weapons(Damage, 2, "EnemyWeapon", ProjectileSpeed);
                StopRange = 200 * player.Scaling;
                EnemyModel.Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieRange/LookLeft.png", UriKind.Absolute));
            }

            if (EnemyType == "BlindRange")
            {
                EnemyHP = player.Level + 3;
                ZombieSpeed = player.Level * 2 + 7;
                Damage = 10;
                ProjectileDamage = 15 + player.Level * 3;
                ModelHeight *= 0.9;
                ModelWidth *= 0.9;
                ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/BlindRange/ZombieGoLeft.png", UriKind.Absolute));
                ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/BlindRange/ZombieGoRight.png", UriKind.Absolute));
                ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/BlindRange/ZombieLookLeft.png", UriKind.Absolute));
                ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/BlindRange/ZombieLookRight.png", UriKind.Absolute));
                ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/BlindRange/ZombieLookLeft2.png", UriKind.Absolute));
                ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/BlindRange/ZombieLookRight2.png", UriKind.Absolute));
                EnemyModel.Source = ModelLookLeft;
                SpecialAbilityTimer.Start();
                ProjectileRange = 400;
                ProjectileSpeed = 17;
                Collision.Tick -= CollusionCheck;
                Collision.Tick += RangeBlindMovement;
            }


            ZombieSpeed *= player.Scaling;
            EnemyModel.Height = ModelHeight * player.Scaling;
            EnemyModel.Width = ModelWidth * player.Scaling;
            enemyHPBar.Width *= player.Scaling;
            enemyHPBar.Height *= player.Scaling;
            enemyHPBar.FontSize *= player.Scaling;

            PlayerX = player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2;
            PlayerY = player.PlayerModel.Margin.Top + player.PlayerModel.Height / 2;
            ZombieX = EnemyModel.Margin.Left + EnemyModel.Width / 2;
            ZombieY = EnemyModel.Margin.Top + EnemyModel.Height / 2;
        }

        public Enemy(Player player, Grid grid, string Type)
        {
            EnemyType = Type;
            this.player = player;
            this.grid = grid;

            RandomMovementTimer = new DispatcherTimer();
            RandomMovementTimer.Tick += Enemy_Move;
            RandomMovementTimer.Interval = TimeSpan.FromSeconds(2);

            SpecialAbilityTimer = new DispatcherTimer();
            SpecialAbilityTimer.Tick += SpecialAbility;
            SpecialAbilityTimer.Interval = TimeSpan.FromSeconds(1);

            KnockbackingTimer = new DispatcherTimer();
            KnockbackingTimer.Tick += ExplodeKnockBacking;
            KnockbackingTimer.Interval = TimeSpan.FromMilliseconds(100);

            //ForAnimationTimer = new DispatcherTimer();
            //ForAnimationTimer.Tick += SecondsForAnimations;
            //ForAnimationTimer.Interval = TimeSpan.FromMilliseconds(300);
            //ForAnimationTimer.Start();

            AnimationTimer = new DispatcherTimer();
            AnimationTimer.Tick += ZombieAnimation;
            AnimationTimer.Interval = TimeSpan.FromMilliseconds(50);
            AnimationTimer.Start();

            Collision = new DispatcherTimer();
            Collision.Tick += CollusionCheck;
            Collision.Interval = TimeSpan.FromMilliseconds(100);
            Collision.Start();



            //EnemyMovement = new DispatcherTimer();
            //EnemyMovement.Tick += Enemy_Move;
            //EnemyMovement.Interval = TimeSpan.FromSeconds(2);
            //EnemyMovement.Start();

            //grid.Children.Add(check);
            grid.Children.Add(enemyHPBar);


            if (EnemyType == "Strong")
            {
                EnemyHP = player.Level * 3 + 8;
                ZombieSpeed = player.Level * 2 + 7;
                Damage = 15 + player.Level * 2;
                ModelHeight *= 1.2;
                ModelWidth *= 1.2;
                ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongGoLeft.png", UriKind.Absolute));
                ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongGoRight.png", UriKind.Absolute));
                ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongLookLeft.png", UriKind.Absolute));
                ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStrong/ZombieStrongLookRight.png", UriKind.Absolute));
                EnemyModel.Source = ModelLookLeft;


            }
            if (EnemyType == "BossStandart")
            {
                Damage = player.Level * 3 + 10;
                EnemyHP = player.Level * 3 + 3;
                ZombieSpeed = player.Level * 2 + 10;
                ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieGoLeft.png", UriKind.Absolute));
                ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieGoRight.png", UriKind.Absolute));
                ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookLeft.png", UriKind.Absolute));
                ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookRight.png", UriKind.Absolute));
                ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookLeft2.png", UriKind.Absolute));
                ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookRight2.png", UriKind.Absolute));
                if (player.Level == 1)
                {
                    // Поменять модельку
                }
            }

            if (EnemyType == "Blind")
            {
                Damage = player.Level * 3 + 15;
                EnemyHP = player.Level * 3 + 3;
                ZombieSpeed = player.Level * 2 + 7;
                ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieGoLeft.png", UriKind.Absolute));
                ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieGoRight.png", UriKind.Absolute));
                ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieLookLeft.png", UriKind.Absolute));
                ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieLookRight.png", UriKind.Absolute));
                ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieLookLeft2.png", UriKind.Absolute));
                ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieBlind/ZombieLookRight2.png", UriKind.Absolute));
            }

            if (EnemyType == "Aggressive")
            {
                EnemyHP = 3 + player.Level * 2;
                ZombieSpeed = player.Level * 2 + 4;
                Damage = player.Level * 4 + 15;
                AbilityCD = ran.Next(3, 6);
                AbilityTime = 2;
                SpecialAbilityTimer.Start();
                ModelHeight *= 0.9;
                ModelWidth *= 0.9;
                ModelGoLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieGoLeft.png", UriKind.Absolute));
                ModelGoRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieGoRight.png", UriKind.Absolute));
                ModelLookLeft = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieLookLeft.png", UriKind.Absolute));
                ModelLookRight = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieLookRight.png", UriKind.Absolute));
                ModelLookLeft2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieLookLeft2.png", UriKind.Absolute));
                ModelLookRight2 = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Aggressive/ZombieLookRight2.png", UriKind.Absolute));
                EnemyModel.Source = ModelLookLeft;
            }

            if (EnemyType == "Range")
            {
                EnemyHP = player.Level + 1;
                ZombieSpeed = player.Level * 2 + 10;
                ProjectileRange = (player.Level * 100 + 400) * player.Scaling;
                Damage = player.Level * 3 + 15;
                ModelHeight *= 0.7;
                ModelWidth *= 0.7;
                ProjectileSpeed = 5 + player.Level * 2;
                weapon = new Weapons(Damage, 2, "EnemyWeapon", ProjectileSpeed);
                StopRange = 200 * player.Scaling;
                EnemyModel.Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieRange/LookLeft.png", UriKind.Absolute));
            }


            ZombieSpeed *= player.Scaling;
            EnemyModel.Height = ModelHeight * player.Scaling;
            EnemyModel.Width = ModelWidth * player.Scaling;
            enemyHPBar.Width *= player.Scaling;
            enemyHPBar.Height *= player.Scaling;
            enemyHPBar.FontSize *= player.Scaling;
        }



        public void Enemy_Move(object sender, EventArgs e)
        {
            Random Move = new Random();
            MoveS = RunMove[Move.Next(0, 12)];
        }

        //public void Damege(Rect BulletHitBox, int PlayerDamage, Bullets Bullet)
        //{
        //    if (EnemyModel != null) // Мб переделать
        //    {
        //        bool isIntersect = false;
        //        EnemyHitBox = new Rect(EnemyModel.Margin.Left, EnemyModel.Margin.Top, EnemyModel.ActualWidth, EnemyModel.ActualHeight);
        //
        //        if (EnemyHitBox.IntersectsWith(BulletHitBox))
        //        {
        //            isIntersect = true;
        //            RectToDel.Add(Bullet.EnemyBullet);
        //            check.Text += "1";
        //        }
        //        if (isIntersect && Bullet.IsMakeDamage == false)
        //        {
        //            EnemyHP -= PlayerDamage;
        //            Bullet.IsMakeDamage = true;
        //        }
        //    }
        //}

        int timerAbilityShooting = 0;
        bool IsShoot = false;
        private void SpecialAbility(object sender, EventArgs e)
        {
            timerSpecialAbility += 1;
            if (EnemyType == "Aggressive")
            {
                if (timerSpecialAbility == AbilityCD && IsAbilityActive == false)
                {
                    ZombieSpeed += 15;
                    IsAbilityActive = true;
                    timerSpecialAbility = 0;
                }

                if (timerSpecialAbility == AbilityTime && IsAbilityActive == true)
                {
                    ZombieSpeed -= 15;
                    IsAbilityActive = false;
                    timerSpecialAbility = 0;
                    AbilityCD = ran.Next(2, 4);
                }

            }
            if (EnemyType == "BlindRange")
            {
                timerAbilityShooting += 1;

                if (timerAbilityShooting >= 2 && IsShoot == false)
                {
                    IsShoot = true;
                    timerAbilityShooting = 0;
                }
            }
        }

        //private void SecondsForAnimations(object sender, EventArgs e)
        //{
        //    //timer += 1;
        //}

        private void ZombieAnimation(object sender, EventArgs e)
        {
            timer += 1;

            if (EnemyModel != null && player.IsEnemyFrozen == false)
            {

                if (EnemyGoLeft == true)
                {
                    switch (timer % 3)
                    {
                        case 2:
                            EnemyModel.Source = ModelLookLeft;
                            break;
                        case 1:
                            EnemyModel.Source = ModelGoLeft;
                            break;
                        case 0:
                            EnemyModel.Source = ModelLookLeft2;
                            break;
                    }

                }

                if (EnemyGoRight == true)
                {
                    switch (timer % 3)
                    {
                        case 2:
                            EnemyModel.Source = ModelLookRight;
                            break;
                        case 1:
                            EnemyModel.Source = ModelGoRight;
                            break;
                        case 0:
                            EnemyModel.Source = ModelLookRight2;
                            break;
                    }
                }





                //if (EnemyGoRight == true)
                //{
                //    if (Attacking == true)
                //    {
                //        switch (timer % 3)
                //        {
                //            case 0:
                //                EnemyModel.Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieHitRight.png", UriKind.Absolute));
                //                break;
                //            case 1:
                //                EnemyModel.Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookRight.png", UriKind.Absolute));
                //                break;
                //            case 2:
                //                EnemyModel.Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieGoRight.png", UriKind.Absolute));
                //                break;
                //        }
                //        
                //        Attacking = false;
                //    }
                //    else
                //    {
                //        if (timer % 2 == 0)
                //        {
                //            EnemyModel.Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieLookRight.png", UriKind.Absolute));
                //        }
                //        else
                //        {
                //            EnemyModel.Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/ZombieStandart/ZombieGoRight.png", UriKind.Absolute));
                //        }
                //    }
                //    
                //}

            }
        }

        private void ExplodeKnockBacking(object sender, EventArgs e)
        {
            timerKnockbacking += 1;
            ZombieSpeedX *= 0.7;
            ZombieSpeedY *= 0.7;
            EnemyHP -= TakenDamage;
            if (timerKnockbacking == 6 && WillDead == false)
            {
                timerKnockbacking = 0;
                IsKnockbacking = false;
                KnockbackingTimer.Stop();
            }
            else if (timerKnockbacking == 6 && WillDead == true)
            {
                timerKnockbacking = 0;
                IsKnockbacking = false;
                KnockbackingTimer.Stop();
                EnemyHP -= 6;
            }
        }

        public void Knockbacking()
        {
            if (player.PlayerHitBox.IntersectsWith(EnemyHitBox))
            {
                if (EnemyModel.Margin.Left + EnemyModel.Width / 2 < player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth / 2)
                {
                    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - player.KnockBack, EnemyModel.Margin.Top, 0, 0);
                }

                if (EnemyModel.Margin.Left + EnemyModel.Width / 2 > player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth / 2)
                {
                    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + player.KnockBack, EnemyModel.Margin.Top, 0, 0);
                }

                if (EnemyModel.Margin.Top < player.PlayerModel.Margin.Top - player.PlayerModel.ActualHeight / 2)
                {
                    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - player.KnockBack, 0, 0);
                }

                if (EnemyModel.Margin.Top > player.PlayerModel.Margin.Top - player.PlayerModel.ActualHeight / 2)
                {
                    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top + player.KnockBack, 0, 0);
                }
            }
        } // Можно переделать под верхний метод

        public void CollusionCheck(object sender, EventArgs e)
        {
            if (EnemyModel != null)
            {
                EnemyHitBox = new Rect(EnemyModel.Margin.Left, EnemyModel.Margin.Top + EnemyModel.Height / 4, EnemyModel.ActualWidth, EnemyModel.ActualHeight * 3 / 4);
                enemyHPBar.Margin = new Thickness(EnemyModel.Margin.Left + (EnemyModel.Width / 2) - (enemyHPBar.Width / 2), EnemyModel.Margin.Top - enemyHPBar.ActualHeight, 0, 0);
                enemyHPBar.Content = EnemyHP.ToString();
                //enemyHPBar.Text = timer.ToString();

                if (EnemyType != "BossStandart")
                {
                    EnemyAgr = room.RoomHitBox;
                    EnemyAgro = room.RoomHitBox;
                }

                if (EnemyType == "BossStandart")
                {
                    EnemyAgr = new Rect(0, 0, grid.ActualWidth, grid.ActualHeight);
                    EnemyAgro = new Rect(0, 0, grid.ActualWidth, grid.ActualHeight);
                }

                if (EnemyAgr.IntersectsWith(player.PlayerHitBox) && player.IsEnemyFrozen == false && IsKnockbacking == false)
                {
                    RandomMovementTimer.Stop();

                    if (EnemyType == "Strong" || EnemyType == "Standart" || EnemyType == "Aggressive" || EnemyType == "Blind" || EnemyType == "BossStandart")
                    {
                        if (EnemyType == "Blind" && ZombieSpeed < player.Level + 7 + 7)
                        {
                            ZombieSpeed += 7;
                        }

                        SpeedCoef = ZombieSpeed / Math.Sqrt(Math.Pow(player.PlayerModel.Margin.Left - EnemyModel.Margin.Left, 2) + Math.Pow(player.PlayerModel.Margin.Top - EnemyModel.Margin.Top, 2));

                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + (player.PlayerModel.Margin.Left - EnemyModel.Margin.Left) * SpeedCoef, EnemyModel.Margin.Top + (player.PlayerModel.Margin.Top - EnemyModel.Margin.Top) * SpeedCoef, 0, 0);

                        //EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + (PlayerX - ZombieX) / 400, EnemyModel.Margin.Top + (PlayerY - ZombieY) / 400, 0, 0);

                        if (player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 - (EnemyModel.Margin.Left + EnemyModel.Width / 2) > 0)
                        {
                            //ZombieSpeedX = ZombieSpeed;
                            EnemyGoLeft = false;
                            EnemyGoRight = true;
                        }
                        else if (player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 - (EnemyModel.Margin.Left + EnemyModel.Width / 2) < 0)
                        {
                            //ZombieSpeedX = -ZombieSpeed;
                            EnemyGoLeft = true;
                            EnemyGoRight = false;
                        }
                        else
                        {
                            //ZombieSpeedX = 0;
                            //EnemyGoRight = false;
                            //EnemyGoLeft = true;
                        }

                        //if (player.PlayerModel.Margin.Top + player.PlayerModel.Height - (EnemyModel.Margin.Top + EnemyModel.Height) > 0)
                        //{
                        //    ZombieSpeedY = ZombieSpeed;
                        //}
                        //else if (player.PlayerModel.Margin.Top + player.PlayerModel.Height - (EnemyModel.Margin.Top + EnemyModel.Height) < 0)
                        //{
                        //    ZombieSpeedY = -ZombieSpeed;
                        //}
                        //else
                        //{
                        //    ZombieSpeedY = 0;
                        //}



                        //if (EnemyModel.Margin.Left + EnemyModel.Width / 2 < player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth / 2)
                        //{
                        //    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                        //    EnemyGoLeft = false;
                        //    EnemyGoRight = true;
                        //
                        //}
                        //
                        //if (EnemyModel.Margin.Left + EnemyModel.Width / 2 > player.PlayerModel.Margin.Left + player.PlayerModel.ActualWidth / 2)
                        //{
                        //    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                        //    EnemyGoRight = false;
                        //    EnemyGoLeft = true;
                        //
                        //}
                        //
                        //if (EnemyModel.Margin.Top < player.PlayerModel.Margin.Top - player.PlayerModel.ActualHeight / 2)
                        //{
                        //    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                        //    EnemyGoDown = true;
                        //    EnemyGoUp = false;
                        //    //AnimationType = 1;
                        //
                        //}
                        //
                        //if (EnemyModel.Margin.Top > player.PlayerModel.Margin.Top - player.PlayerModel.ActualHeight / 2)
                        //{
                        //    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                        //    EnemyGoUp = true;
                        //    EnemyGoDown = false;
                        //    //AnimationType = 0;
                        //
                        //}
                    }

                }

                if (IsKnockbacking == true && player.IsEnemyFrozen == false)
                {
                    EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeedX, EnemyModel.Margin.Top + ZombieSpeedY, 0, 0);

                    ZombieSpeedY = 0;
                    ZombieSpeedX = 0;

                    if (player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 - (EnemyModel.Margin.Left + EnemyModel.Width / 2) > 0)
                    {
                        ZombieSpeedX = ZombieSpeed;
                        EnemyGoLeft = false;
                        EnemyGoRight = true;
                    }
                    else if (player.PlayerModel.Margin.Left + player.PlayerModel.Width / 2 - (EnemyModel.Margin.Left + EnemyModel.Width / 2) < 0)
                    {
                        ZombieSpeedX = -ZombieSpeed;
                        EnemyGoLeft = true;
                        EnemyGoRight = false;
                    }
                    else
                    {
                        ZombieSpeedX = 0;
                        //EnemyGoRight = false;
                        //EnemyGoLeft = true;
                    }

                    if (player.PlayerModel.Margin.Top + player.PlayerModel.Height - (EnemyModel.Margin.Top + EnemyModel.Height) > 0)
                    {
                        ZombieSpeedY = ZombieSpeed;
                    }
                    else if (player.PlayerModel.Margin.Top + player.PlayerModel.Height - (EnemyModel.Margin.Top + EnemyModel.Height) < 0)
                    {
                        ZombieSpeedY = -ZombieSpeed;
                    }
                    else
                    {
                        ZombieSpeedY = 0;
                    }
                }

                if (EnemyType == "Blind")
                {
                    if (EnemyHitBox.IntersectsWith(EnemyAgro))
                    {
                        RandomMovementTimer.Start();

                        if (EnemyType == "Blind" && ZombieSpeed > player.Level + 7)
                        {
                            ZombieSpeed -= 7;
                        }
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


                if (IsShoot)
                {
                    Bullets bullet1 = new Bullets(EnemyModel.Margin.Left + EnemyModel.ActualWidth * 5 / 4, EnemyModel.Margin.Top + EnemyModel.ActualHeight / 4, "EnemyWeapom", "UpRight", this);
                    grid.Children.Add(bullet1.EnemyBullet);
                    bullets.Add(bullet1);

                    Bullets bullet2 = new Bullets(EnemyModel.Margin.Left - EnemyModel.ActualHeight / 2, EnemyModel.Margin.Top + EnemyModel.ActualHeight / 4, "EnemyWeapon", "UpLeft", this);
                    grid.Children.Add(bullet2.EnemyBullet);
                    bullets.Add(bullet2);

                    Bullets bullet3 = new Bullets(EnemyModel.Margin.Left - EnemyModel.ActualWidth / 2, EnemyModel.Margin.Top + EnemyModel.ActualHeight / 2, "EnemyWeapon", "DownLeft", this);
                    grid.Children.Add(bullet3.EnemyBullet);
                    bullets.Add(bullet3);

                    Bullets bullet4 = new Bullets(EnemyModel.Margin.Left + EnemyModel.ActualWidth * 5 / 4, EnemyModel.Margin.Top + EnemyModel.Height / 2, "EnemyWeapon", "DownRight", this);
                    grid.Children.Add(bullet4.EnemyBullet);
                    bullets.Add(bullet4);
                    IsShoot = false;
                }

                foreach (Bullets x in bullets)
                {
                    if (x.EnemyBullet != null)
                    {
                        if (x.Direction == "UpRight")
                        {
                            x.EnemyBullet.Margin = new Thickness(x.EnemyBullet.Margin.Left + x.enemy.ProjectileSpeed / 2, x.EnemyBullet.Margin.Top - x.enemy.ProjectileSpeed / 2, 0, 0);
                            if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.EnemyBullet.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.EnemyBullet.Margin.Left), 2)) >= x.enemy.ProjectileRange * 2)
                            {
                                grid.Children.Remove(x.EnemyBullet);
                                x.EnemyBullet = null;
                            }
                        }

                        if (x.Direction == "UpLeft")
                        {
                            x.EnemyBullet.Margin = new Thickness(x.EnemyBullet.Margin.Left - x.enemy.ProjectileSpeed / 2, x.EnemyBullet.Margin.Top - x.enemy.ProjectileSpeed / 2, 0, 0);
                            if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.EnemyBullet.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.EnemyBullet.Margin.Left), 2)) >= x.enemy.ProjectileRange * 2)
                            {
                                grid.Children.Remove(x.EnemyBullet);
                                x.EnemyBullet = null;
                            }
                        }

                        if (x.Direction == "DownLeft")
                        {
                            x.EnemyBullet.Margin = new Thickness(x.EnemyBullet.Margin.Left - x.enemy.ProjectileSpeed / 2, x.EnemyBullet.Margin.Top + x.enemy.ProjectileSpeed / 2, 0, 0);
                            if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.EnemyBullet.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.EnemyBullet.Margin.Left), 2)) >= x.enemy.ProjectileRange * 2)
                            {
                                grid.Children.Remove(x.EnemyBullet);
                                x.EnemyBullet = null;
                            }
                        }

                        if (x.Direction == "DownRight")
                        {
                            x.EnemyBullet.Margin = new Thickness(x.EnemyBullet.Margin.Left + x.enemy.ProjectileSpeed / 2, x.EnemyBullet.Margin.Top + x.enemy.ProjectileSpeed / 2, 0, 0);
                            if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.EnemyBullet.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.EnemyBullet.Margin.Left), 2)) >= x.enemy.ProjectileRange * 2)
                            {
                                grid.Children.Remove(x.EnemyBullet);
                                x.EnemyBullet = null;
                            }
                        }


                        if (x.EnemyBullet != null)
                        {
                            Rect BulletHB = new Rect(x.EnemyBullet.Margin.Left, x.EnemyBullet.Margin.Top, x.EnemyBullet.ActualWidth, x.EnemyBullet.ActualHeight);

                            Rect PlayerHitBox = new Rect(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top, player.PlayerModel.Width, player.PlayerModel.Height);
                            bool isIntersect = false;

                            if (BulletHB.IntersectsWith(PlayerHitBox))
                            {
                                isIntersect = true;
                                grid.Children.Remove(x.EnemyBullet);
                                x.EnemyBullet = null;
                            }
                            if (isIntersect && x.IsMakeDamage == false)
                            {
                                player.PlayerHP -= x.enemy.ProjectileDamage;
                                x.IsMakeDamage = true;









                                if (EnemyType == "Range")
                                {

                                    Rect Stopped = new Rect(EnemyModel.Margin.Left - StopRange, EnemyModel.Margin.Top - StopRange, StopRange * 2, StopRange * 2);
                                    Rect AttackRangeHitBox = new Rect(EnemyModel.Margin.Left - ProjectileSpeed, EnemyModel.Margin.Top - ProjectileRange, ProjectileRange * 2, ProjectileRange * 2);
                                    if (player.PlayerHitBox.IntersectsWith(Stopped))
                                    {
                                        if (EnemyModel.Margin.Left < player.PlayerModel.Margin.Left)
                                        {
                                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                                        }

                                        if (EnemyModel.Margin.Left > player.PlayerModel.Margin.Left)
                                        {
                                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                                        }

                                        if (EnemyModel.Margin.Top < player.PlayerModel.Margin.Top)
                                        {
                                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                                        }

                                        if (EnemyModel.Margin.Top > player.PlayerModel.Margin.Top)
                                        {
                                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }


        }

        public void RangeBlindMovement(object sender, EventArgs e)
        {
            foreach (Bullets x in bullets)
            {
                if (x.EnemyBullet != null)
                {
                    if (x.Direction == "UpRight")
                    {
                        
                        x.EnemyBullet.Margin = new Thickness(x.EnemyBullet.Margin.Left + ProjectileSpeed / 2, x.EnemyBullet.Margin.Top - ProjectileSpeed / 2, 0, 0);
                        if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.EnemyBullet.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.EnemyBullet.Margin.Top), 2)) >= ProjectileRange * 2)
                        {
                            grid.Children.Remove(x.EnemyBullet);
                            x.EnemyBullet = null;
                        }
                    }

                    if (x.Direction == "UpLeft")
                    {
                        x.EnemyBullet.Margin = new Thickness(x.EnemyBullet.Margin.Left - ProjectileSpeed / 2, x.EnemyBullet.Margin.Top - ProjectileSpeed / 2, 0, 0);
                            if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.EnemyBullet.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.EnemyBullet.Margin.Top), 2)) >= ProjectileRange * 2)
                            {
                            grid.Children.Remove(x.EnemyBullet);
                            x.EnemyBullet = null;
                        }
                    }

                    if (x.Direction == "DownLeft")
                    {
                        x.EnemyBullet.Margin = new Thickness(x.EnemyBullet.Margin.Left - ProjectileSpeed / 2, x.EnemyBullet.Margin.Top + ProjectileSpeed / 2, 0, 0);
                        if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.EnemyBullet.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.EnemyBullet.Margin.Top), 2)) >= ProjectileRange * 2)
                        {
                            grid.Children.Remove(x.EnemyBullet);
                            x.EnemyBullet = null;
                        }
                    }

                    if (x.Direction == "DownRight")
                    {
                        x.EnemyBullet.Margin = new Thickness(x.EnemyBullet.Margin.Left + x.enemy.ProjectileSpeed / 2, x.EnemyBullet.Margin.Top + ProjectileSpeed / 2, 0, 0);
                        if (Math.Sqrt(Math.Pow(Math.Abs(x.CoordX - x.EnemyBullet.Margin.Left), 2) + Math.Pow(Math.Abs(x.CoordY - x.EnemyBullet.Margin.Top), 2)) >= ProjectileRange * 2)
                        {
                            grid.Children.Remove(x.EnemyBullet);
                            x.EnemyBullet = null;
                        }
                    }


                    if (x.EnemyBullet != null)
                    {
                        Rect BulletHB = new Rect(x.EnemyBullet.Margin.Left, x.EnemyBullet.Margin.Top, x.EnemyBullet.ActualWidth, x.EnemyBullet.ActualHeight);

                        Rect PlayerHitBox = new Rect(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top, player.PlayerModel.Width, player.PlayerModel.Height);
                        bool isIntersect = false;

                        if (BulletHB.IntersectsWith(PlayerHitBox))
                        {
                            isIntersect = true;
                            grid.Children.Remove(x.EnemyBullet);
                            x.EnemyBullet = null;
                        }
                        if (isIntersect && x.IsMakeDamage == false)
                        {
                            player.PlayerHP -= ProjectileDamage;
                            x.IsMakeDamage = true;
                        }
                    }
                }
            }


            if (EnemyModel != null)
            {
                EnemyHitBox = new Rect(EnemyModel.Margin.Left, EnemyModel.Margin.Top + EnemyModel.Height / 4, EnemyModel.ActualWidth, EnemyModel.ActualHeight * 3 / 4);
                enemyHPBar.Margin = new Thickness(EnemyModel.Margin.Left + (EnemyModel.Width / 2) - (enemyHPBar.Width / 2), EnemyModel.Margin.Top - enemyHPBar.ActualHeight, 0, 0);
                enemyHPBar.Content = EnemyHP.ToString();
                EnemyAgro = new Rect(room.RoomModel.Margin.Left + EnemyModel.Width, room.RoomModel.Margin.Top + EnemyModel.Height, room.RoomModel.Width - EnemyModel.Width * 2, room.RoomModel.Height - EnemyModel.Height * 2);
                EnemyAgr = new Rect(EnemyModel.Margin.Left + EnemyModel.Width / 4 - EnemyModel.Width * 2, EnemyModel.Margin.Top + EnemyModel.Height / 2 - EnemyModel.Height*1.5, EnemyModel.ActualWidth / 2 + EnemyModel.Width * 4 + EnemyModel.Width / 4, EnemyModel.ActualHeight / 2 + EnemyModel.Height * 3);
                if (player.PlayerHitBox.IntersectsWith(EnemyAgr) && IsOut == false)
                {
                    IsOut = false;
                    if (IsSpeedChange == false)
                    {
                        ZombieSpeed += 7;
                        IsSpeedChange = true;
                    }
                    RandomMovementTimer.Stop();
                    if (EnemyType == "Blind")
                    {
                        SpeedCoef = ZombieSpeed / Math.Sqrt(Math.Pow(player.PlayerModel.Margin.Left - EnemyModel.Margin.Left, 2) + Math.Pow(player.PlayerModel.Margin.Top - EnemyModel.Margin.Top, 2));

                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + (player.PlayerModel.Margin.Left - EnemyModel.Margin.Left) * SpeedCoef, EnemyModel.Margin.Top + (player.PlayerModel.Margin.Top - EnemyModel.Margin.Top) * SpeedCoef, 0, 0);
                    }

                    if (EnemyType == "BlindRange")
                    {
                        SpeedCoef = ZombieSpeed / Math.Sqrt(Math.Pow(player.PlayerModel.Margin.Left - EnemyModel.Margin.Left, 2) + Math.Pow(player.PlayerModel.Margin.Top - EnemyModel.Margin.Top, 2));

                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - (player.PlayerModel.Margin.Left - EnemyModel.Margin.Left) * SpeedCoef, EnemyModel.Margin.Top - (player.PlayerModel.Margin.Top - EnemyModel.Margin.Top) * SpeedCoef, 0, 0);
                    }
                    
                    
                    if (EnemyModel.Margin.Top + EnemyModel.Height < EnemyAgro.Top + EnemyAgro.Height)
                    {
                        IsOut = true;
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                    }
                    if (EnemyModel.Margin.Top > EnemyAgro.Top)
                    {
                        IsOut = true;
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                    }
                    if (EnemyModel.Margin.Left + EnemyModel.Width < EnemyAgro.Left)
                    {
                        IsOut = true;
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                        EnemyGoRight = true;
                        EnemyGoLeft = false;
                    }
                    if (EnemyModel.Margin.Left > EnemyAgro.Left + EnemyAgro.Width)
                    {
                        IsOut = true;
                        EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                        EnemyGoRight = false;
                        EnemyGoLeft = true;
                    }

                    else
                    {
                        IsOut = false;
                    }
                }

                else 
                {
                    if (EnemyHitBox.IntersectsWith(EnemyAgro) && IsOut == false)
                    {
                        if (IsSpeedChange == true)
                        {
                            ZombieSpeed -= 7;
                            IsSpeedChange = false;
                        }
                        IsOut = false;
                        RandomMovementTimer.Start();

                        if (MoveS == "Up")
                        {
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                        }
                        if (MoveS == "RightUp")
                        {
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                            EnemyGoRight = true;
                            EnemyGoLeft = false;
                        }
                        if (MoveS == "LeftUp")
                        {
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                            EnemyGoRight = false;
                            EnemyGoLeft = true;
                        }
                        if (MoveS == "LeftDown")
                        {
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                            EnemyGoRight = false;
                            EnemyGoLeft = true;
                        }
                        if (MoveS == "RightDown")
                        {
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                            EnemyGoRight = true;
                            EnemyGoLeft = false;
                        }
                        if (MoveS == "Down")
                        {
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                        }
                        if (MoveS == "Left")
                        {
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                            EnemyGoRight = false;
                            EnemyGoLeft = true;
                        }
                        if (MoveS == "Right")
                        {
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                            EnemyGoRight = true;
                            EnemyGoLeft = false;
                        }

                        if (EnemyModel.Margin.Top + EnemyModel.Height < EnemyAgro.Top + EnemyAgro.Height)
                        {
                            IsOut = true;
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top + ZombieSpeed, 0, 0);
                        }
                        if (EnemyModel.Margin.Top > EnemyAgro.Top)
                        {
                            IsOut = true;
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left, EnemyModel.Margin.Top - ZombieSpeed, 0, 0);
                        }
                        if (EnemyModel.Margin.Left + EnemyModel.Width < EnemyAgro.Left)
                        {
                            IsOut = true;
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left + ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                            EnemyGoRight = true;
                            EnemyGoLeft = false;
                        }
                        if (EnemyModel.Margin.Left > EnemyAgro.Left + EnemyAgro.Width)
                        {
                            IsOut = true;
                            EnemyModel.Margin = new Thickness(EnemyModel.Margin.Left - ZombieSpeed, EnemyModel.Margin.Top, 0, 0);
                            EnemyGoRight = false;
                            EnemyGoLeft = true;
                        }

                        else
                        {
                            IsOut = false;
                        }
                    }
                
                }

                if (IsShoot)
                {
                    Bullets bullet1 = new Bullets(EnemyModel.Margin.Left + EnemyModel.ActualWidth * 5 / 4, EnemyModel.Margin.Top + EnemyModel.ActualHeight / 4, "EnemyWeapon", "UpRight", this);
                    grid.Children.Add(bullet1.EnemyBullet);
                    bullets.Add(bullet1);

                    Bullets bullet2 = new Bullets(EnemyModel.Margin.Left - EnemyModel.ActualHeight / 2, EnemyModel.Margin.Top + EnemyModel.ActualHeight / 4, "EnemyWeapon", "UpLeft", this);
                    grid.Children.Add(bullet2.EnemyBullet);
                    bullets.Add(bullet2);

                    Bullets bullet3 = new Bullets(EnemyModel.Margin.Left - EnemyModel.ActualWidth / 2, EnemyModel.Margin.Top + EnemyModel.ActualHeight / 2, "EnemyWeapon", "DownLeft", this);
                    grid.Children.Add(bullet3.EnemyBullet);
                    bullets.Add(bullet3);

                    Bullets bullet4 = new Bullets(EnemyModel.Margin.Left + EnemyModel.ActualWidth * 5 / 4, EnemyModel.Margin.Top + EnemyModel.Height / 2, "EnemyWeapon", "DownRight", this);
                    grid.Children.Add(bullet4.EnemyBullet);
                    bullets.Add(bullet4);
                    IsShoot = false;
                }
            }

        }

        public void Delete(Enemy enem)
        {
            foreach(Bullets x in bullets)
            {
                grid.Children.Remove(x.EnemyBullet);
            }
            grid.Children.Remove(EnemyModel);
            grid.Children.Remove(enemyHPBar);
            EnemyModel = null;
            EnemyHitBox = new Rect(0, 0, 0, 0);
            EnemyAgr = new Rect(0, 0, 0, 0);
            EnemyAgro = new Rect(0, 0, 0, 0);
            Collision.Stop();
            AnimationTimer.Stop();
            SpecialAbilityTimer.Stop();
            enem = null;
        }


    }
}



    

