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
        public int PlayerHP = 100, PlayerHeight = 70, PlayerWidth = 40;
        public double Speed = 2, PlayerSpeedX, PlayerSpeedY, friction = 0.77;
        public Rect PlayerHitBox;
        public Rectangle PlayerModel = new Rectangle
        {
            Height = 70,
            Width = 40,
            Fill = Brushes.AntiqueWhite,
            Tag = "player",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

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
    }
}
