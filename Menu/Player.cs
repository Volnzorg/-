using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Menu
{
    public class Player
    {
        public int PlayerMaxHP = 100; // Для бафов на хп и прверки того, что хп не регенится больше, чем возможно(100 по стандарту)
        public int PlayerHP = 100, PlayerHeight = 70, PlayerWidth = 40;
        public int Level = 0; //Число пройденных пещер
        public int PlayerCurrentSpeed = 2; // Переменная запоминающая скорость персонажа
        public double Speed = 2, PlayerSpeedX, PlayerSpeedY, friction = 0.77;
        public double DamageReduction; // Броня
      //public weapon - добавить оружие
        public Rect PlayerHitBox;
        public Rectangle PlayerModel;

        public Player()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Correction;
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Start();
        }

        public void DamageCheck(Rect enemy)
        {
            
            if (enemy.IntersectsWith(PlayerHitBox))
            {
                PlayerHP -= 10;
            }

        }
        private void Correction(object sender, EventArgs e)
        {
            PlayerModel.Width = PlayerWidth;
            PlayerModel.Height = PlayerHeight;
            PlayerHitBox = new Rect(PlayerModel.Margin.Left, PlayerModel.Margin.Top, PlayerWidth, PlayerHeight);
        }

        public void PlayerModelCreator()
        {
            PlayerModel = new Rectangle
            {
                Height = PlayerHeight,
                Width = PlayerWidth,
                Fill = Brushes.AntiqueWhite,
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
