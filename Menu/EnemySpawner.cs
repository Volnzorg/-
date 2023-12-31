using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Menu.Window1;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Threading;

namespace Menu
{
    internal class EnemySpawner
    {
        public int enemySpawn, enemyCount;
        string EnemyType;
        Grid grid;
        Player player;
        Window1 window;

        public EnemySpawner(Grid grid, Player player, Window1 window)
        {
            this.grid = grid;
            this.player = player;
            this.window = window;

            DispatcherTimer enemyExist = new DispatcherTimer();
            enemyExist.Tick += DoEnemyExist;
            enemyExist.Interval = TimeSpan.FromSeconds(5);
            enemyExist.Start();
        }

        public void Enemy_Spawn(string Type)
        {
            Random ranX = new Random();
            Random ranY = new Random();
            Random spawn = new Random();
            enemySpawn = spawn.Next(1, 3);
            int x = 0; int y = 0;
            for (int i = 0; i < enemySpawn; i++)
            {
                x = ranX.Next(300, 500);
                y = ranY.Next(300, 500);
                Rect EnemyAgro = new Rect(x - 50, y - 50, 100, 100);
                Enemy zomb = new Enemy(player, grid, EnemyAgro, x, y, window);
                grid.Children.Add(zomb.EnemyModel);
                window.Enemies(zomb);
            }
            enemyCount = enemySpawn;
        }

        public void DoEnemyExist(object sender, EventArgs e)
        {
            if (enemyCount == 0)
            {
                Enemy_Spawn("zombie");
            }
        }


    }


}
