using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;

namespace Menu
{

    public class Bombs
    {
        static BitmapImage BombModelView = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/Bomb.png", UriKind.Absolute));

        public Image BombModel = new Image
        {
            Name = "Bomb",
            Source = BombModelView,
            Height = 40,
            Width = 40,
            Stretch = Stretch.Fill,
            Tag = "PlayerBomb",
            Margin = new Thickness(0, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        public int Damage;
        public double Knockback;
        public Rect ExplodeRadius;

        double Radius;

        int Seconds = 0;
        Engine engine;

        DispatcherTimer timer;

        public Bombs(Engine engine, string Whose) 
        {

            this.engine = engine;
            BombModel.Tag = Whose;

            BombModel.Height *= engine.player.Scaling;
            BombModel.Width *= engine.player.Scaling;

            Knockback = BombModel.Width / 3;
            Radius = 200 * engine.player.Scaling;

            Damage = 1;

            timer = new DispatcherTimer();
            timer.Tick += Counter;
            timer.Interval += TimeSpan.FromSeconds(1);
            timer.Start(); 
        }

        private void Counter(object sender, EventArgs e)
        {
            Seconds += 1;
            if (Seconds == 3)
            {
                ExplodeRadius = new Rect(BombModel.Margin.Left - Radius, BombModel.Margin.Top - Radius, Radius * 2, Radius * 2);
                engine.Explodes(this);
                timer.Stop();
            }  
        }
    }
}
