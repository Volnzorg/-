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
        Player player;
        Window1 window;
        Caves cave;
        Room room;
        Grid grid;

        public EnemySpawner( Player player, Window1 window)
        {
            this.player = player;
            this.window = window;

            DispatcherTimer enemyExist = new DispatcherTimer();
            enemyExist.Tick += DoEnemyExist;
            enemyExist.Interval = TimeSpan.FromSeconds(5);
            enemyExist.Start();
        }

        public EnemySpawner(Room room, Player player, Caves cave)
        {
            this.player = player;
            this.cave = cave;
            this.room = room;

            DispatcherTimer enemyExist = new DispatcherTimer();
            enemyExist.Tick += DoEnemyExist;
            enemyExist.Interval = TimeSpan.FromSeconds(5);
            enemyExist.Start();
        }

        public void Enemy_Spawn(string Type, Grid grid)
        {
            this.grid = grid;
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
                Enemy zomb = new Enemy(player, grid, EnemyAgro, x, y, room);
                grid.Children.Add(zomb.EnemyModel);
                window.Enemies(zomb);
            }
            enemyCount = enemySpawn;
        }
        public void Enemy_Spawn_Room(int RoomEnemyCount, int RoomDifficult, Grid grid)
        {
            this.grid = grid;
            Random ranX = new Random();
            Random ranY = new Random();
            Random spawn = new Random(); // Сделать рандомное кол-во спавна, если меньше чем должно быть в комнате, то после убийства спавнить еще
            int x, y;
            for (int i = 0; i < RoomEnemyCount; i++)
            {
                x = ranX.Next(Convert.ToInt32(room.RoomModel.Margin.Left + 20), Convert.ToInt32(room.RoomModel.Margin.Left + room.RoomModel.Width - 20));
                y = ranY.Next(Convert.ToInt32(room.RoomModel.Margin.Top + 20), Convert.ToInt32(room.RoomModel.Margin.Top + room.RoomModel.Height - 20));
                Rect EnemyAgro = room.RoomHitBox;
                Enemy zomb = new Enemy(player, grid, EnemyAgro, x, y, room);
                grid.Children.Add(zomb.EnemyModel);
                room.EnemiesList.Add(zomb);
            };
            enemyCount = RoomEnemyCount;
        }

        public void DoEnemyExist(object sender, EventArgs e)
        {
            if (enemyCount == 0 && room.IsCleared == false)
            {
                Enemy_Spawn("zombie", grid);
            }
        }


    }


}
