using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Menu
{
    public class Bullets
    {
        public Image BulletModel;
        public Image BulletModelIm;
        public Rectangle EnemyBullet;
        public double CoordX;
        public double CoordY;
        public string Direction;
        public bool IsMakeDamage = false;
        public string WeaponName;

        BitmapImage BossProjectile = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Enemies/Boss/BossProjectileRight.png", UriKind.Absolute));
        BitmapImage Shotgun = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Weapon/ShotGun/SGbullet2.png", UriKind.Absolute));
        BitmapImage Rifle = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Weapon/Rifle/RFLbullet.png", UriKind.Absolute));
        BitmapImage PlazmaGun = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Weapon/BFG/PGbullet.png", UriKind.Absolute));

        RotateTransform DownRight = new RotateTransform()
        {
            Angle = 45,
        };

        RotateTransform Down = new RotateTransform()
        {
            Angle = 90,
        };

        RotateTransform DownLeft = new RotateTransform()
        {
            Angle = 135,
        };

        RotateTransform Left = new RotateTransform()
        {
            Angle = 180,
        };

        RotateTransform UpLeft = new RotateTransform()
        {
            Angle = 225,
        };

        RotateTransform Up = new RotateTransform()
        {
            Angle = 270,
        };

        RotateTransform UpRight = new RotateTransform()
        {
            Angle = 315,
        };

        public Bullets(double CoordX, double CoordY, string WeaponName, string Direction) 
        {
            this.WeaponName = WeaponName;
            this.Direction = Direction;
            this.CoordX = CoordX;
            this.CoordY = CoordY;
            
            
            BulletModel = new Image
            {
                Width = 20,
                Height = 10,
                Margin = new Thickness(0, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Stretch = Stretch.Fill,
            };

            if (WeaponName == "ShotGun")
            {
                BulletModel.Source = Shotgun;
            }

            if (WeaponName == "Pistol")
            {
                BulletModel.Source = Shotgun;
            }
            if (WeaponName == "Rifle")
            {
                BulletModel.Source = Rifle;
            }

            if (WeaponName == "PlazmaGun")
            {
                BulletModel.Source = PlazmaGun;
            }

            if (Direction == "Down")
            {
                BulletModel.RenderTransform = Down;
                BulletModel.Height = 20;
                BulletModel.Width = 10;
            }
            if (Direction == "Left")
            {
                BulletModel.RenderTransform = Left;
            }
            if (Direction == "Up")
            {
                BulletModel.RenderTransform = Up;
                BulletModel.Height = 20;
                BulletModel.Width = 10;
            }
            BulletModel.Margin = new Thickness(CoordX, CoordY, 0, 0);


        }

        public Enemy enemy;

        public Bullets(double CoordX, double CoordY, string WeaponName, string Direction, Enemy enemy)
        {
            this.Direction = Direction;
            this.CoordX = CoordX;
            this.CoordY = CoordY;
            this.enemy = enemy;

           if (WeaponName == "EnemyWeapon")
           {
               EnemyBullet = new Rectangle
               {
                   Width = 20,
                   Height = 10,
                   Margin = new Thickness(0, 0, 0, 0),
                   VerticalAlignment = VerticalAlignment.Top,
                   HorizontalAlignment = HorizontalAlignment.Left,
                   Fill = Brushes.Red,
                   Stroke = Brushes.Black
               };
               EnemyBullet.Margin = new Thickness(CoordX, CoordY, 0, 0);
           }
        }

        public LastBoss Boss;

        public Bullets(double CoordX, double CoordY, string WeaponName, string Direction, LastBoss Boss)
        {
            this.Direction = Direction;
            this.CoordX = CoordX;
            this.CoordY = CoordY;
            this.Boss = Boss;

            if (WeaponName == "BossWeapon")
            {
                BulletModelIm = new Image
                {
                    Width = 15,
                    Height = 60,
                    Margin = new Thickness(0, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Source = BossProjectile,
                    Stretch = Stretch.Fill,
                };

                if (Direction == "DownRight")
                {
                    BulletModelIm.RenderTransform = DownRight;
                }
                if (Direction == "Down")
                {
                    BulletModelIm.RenderTransform = Down;
                }
                if (Direction == "DownLeft")
                {
                    BulletModelIm.RenderTransform = DownLeft;
                }
                if (Direction == "Left")
                {
                    BulletModelIm.RenderTransform = Left;
                }
                if (Direction == "UpLeft")
                {
                    BulletModelIm.RenderTransform = UpLeft;
                }
                if (Direction == "Up")
                {
                    BulletModelIm.RenderTransform = Up;
                }
                if (Direction == "UpRight")
                {
                    BulletModelIm.RenderTransform = UpRight;
                }
                BulletModelIm.Margin = new Thickness(CoordX, CoordY, 0, 0);
                
            }
        }
    }
}
