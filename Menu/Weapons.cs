using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Menu
{
    public class Weapons
    {


        public Rectangle BulletModel;
        public int Damage;
        public int BulletRange;
        public int AmmoCount;
        public double ShootSpeed;
        public double ReloadTime;
        public string name;
        public double BulletSpeed;


        public Weapons(int Damage, int AmmoCount, int BulletRange, string name, double ReloadTime, double ShootSpeed, double BulletSpeed) 
        {
            this.BulletSpeed = BulletSpeed;
            this.ShootSpeed = ShootSpeed;
            this.Damage = Damage;
            this.ReloadTime = ReloadTime;
            this.AmmoCount = AmmoCount;
            this.name = name;
            this.BulletRange = BulletRange;
        }

        public Weapons(int Damage, double ShootSpeed, string name, double BulletSpeed)
        {
            this.BulletSpeed = BulletSpeed;
            this.ShootSpeed = ShootSpeed;
            this.Damage = Damage;
            this.name = name;

        }

        public Weapons(string name, Player player)
        {
            this.name=name;
            if (name == "ShotGun")
            {
                BulletSpeed = 10;
                ShootSpeed = 7;
                AmmoCount = 2;
                ReloadTime = 2; 
                BulletRange = 200;
                Damage = 3;
            }

            if (name == "Rifle")
            {
                BulletSpeed = 15;
                ShootSpeed = 2;
                AmmoCount = 10;
                ReloadTime = 3;
                BulletRange = 700;
                Damage = 1;
            }

            if (name == "PlazmaGun")
            {
                BulletSpeed = 8;
                ShootSpeed = 5;
                AmmoCount = 7;
                ReloadTime = 2;
                BulletRange = 400;
                Damage = 2;
            }
            
            if (name == "Pistol")
            {
                BulletSpeed = 10;
                ShootSpeed = 4;
                AmmoCount = 10;
                ReloadTime = 4;
                BulletRange = 200;
                Damage = 1;
            }
        }

        
    }
}
