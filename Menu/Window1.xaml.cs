using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Menu
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        
        TextBox check = new TextBox
        {
            Height = 70,
            Width = 40,
            Margin = new Thickness(0, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Text = ""
        };


        Grid Grid1 = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
        };

        EnemySpawner Spawner;
        Engine engine;
        Player player;

        public Window1()
        {
            InitializeComponent();
            Windows.Content = Grid1;
            engine = new Engine(Windows, Grid1);

            DispatcherTimer EnemiesChecked = new DispatcherTimer();
            EnemiesChecked.Interval = TimeSpan.FromMilliseconds(20);
            EnemiesChecked.Tick += EnemiesKilled;
            EnemiesChecked.Start();

            player = engine.player;
            Spawner = new EnemySpawner(Grid1, player, Windows);
            Spawner.Enemy_Spawn("zomnie");
            engine.Element_Spawn();
            Grid1.Children.Add(check);
            player.PlayerModel.Margin = new Thickness(100, 100,0,0);
            Grid1.Children.Add(player.PlayerModel);
        }

        public void EnemiesKilled(object sender, EventArgs e)
        {
            foreach (Enemy x in engine.EnemyList)
            {
                if (x.Killed == true)
                {
                    x.Delete();
                    engine.EnemyDel.Add(x);
                    Spawner.enemyCount -= 1;
                }
            }
            foreach (Enemy x in engine.EnemyDel)
            {
                engine.EnemyList.Remove(x);
            }
            check.Text = player.PlayerHP.ToString();
        }

        public void Enemies(Enemy enem)
        {
            engine.EnemyList.Add(enem);
        }
    }
}
