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
using System.Windows.Ink;

namespace Menu
{
    internal class EnemySpawner
    {
        public int enemySpawn, enemyCount;
        Player player;
        Window1 window;
        Room room;

        Random ran = new Random();
        Random spawn = new Random();
        Grid grid;

        public EnemySpawner( Player player, Window1 window)
        {
            this.player = player;
            this.window = window;
        }

        public EnemySpawner(Room room, Player player, Grid grid)
        {
            this.player = player;
            this.room = room;
            this.grid = grid;
        }

        public void Enemy_Spawn(string Type, Grid grid)
        {
            enemySpawn = spawn.Next(1, 3);
            for (int i = 0; i < enemySpawn; i++)
            {
                int x = ran.Next(300, 500);
                int y = ran.Next(300, 500);
                Rect EnemyAgro = new Rect(x - 50, y - 50, 100, 100);
                Enemy zomb = new Enemy(player, grid, EnemyAgro, room, Type);
                grid.Children.Add(zomb.EnemyModel);
                window.Enemies(zomb);
            }
            enemyCount = enemySpawn;
        }
        public void Enemy_Spawn_Room(string EnemyType)
        {
             // Сделать рандомное кол-во спавна, если меньше чем должно быть в комнате, то после убийства спавнить еще
            int x, y;
            
            Rect EnemyAgro = room.RoomHitBox;
            Enemy zomb = new Enemy(player, grid, EnemyAgro, room, EnemyType);
            x = ran.Next(Convert.ToInt32(room.RoomModel.Margin.Left + zomb.EnemyModel.Width), Convert.ToInt32(room.RoomModel.Margin.Left + room.RoomModel.Width - zomb.EnemyModel.Width * 1.5));
            y = ran.Next(Convert.ToInt32(room.RoomModel.Margin.Top + zomb.EnemyModel.Height), Convert.ToInt32(room.RoomModel.Margin.Top + room.RoomModel.Height - zomb.EnemyModel.Height * 1.5));
            zomb.EnemyModel.Margin = new Thickness(x, y, 0, 0);
            grid.Children.Add(zomb.EnemyModel);
            room.EnemiesList.Add(zomb);
            

        }

        public void RandomEnemyTypes(int RoomEnemyCount)
        {
            int Standart = 0;
            int Strong = 0;
            int Aggressive = 0;
            int Blind = 0;
            int BlindRange = 0;
            bool IsSpawned = false;
            for (int i = 0; i < RoomEnemyCount; i++)
            {
                IsSpawned = false;
                int x;
                switch (player.Level)
                {
                    case 0:
                        x = ran.Next(0, 100);
                        if (x < 5 && Strong < 1)
                        {
                            Enemy_Spawn_Room("Strong");
                            Strong += 1;
                            break;
                        }
                        if (x >= 5 && x < 80 || (x < 80 && Strong > 0 && BlindRange < 2) || (x < 100 && Strong > 0 && BlindRange > 1))
                        {
                            Enemy_Spawn_Room("Standart");
                            Standart++;
                            break;
                        }

                        if (x >= 80 && BlindRange < 2)
                        {
                            Enemy_Spawn_Room("BlindRange");
                            BlindRange++;
                            break;
                        }
                    break;

                    case 1:
                        x = ran.Next(0, 100);
                        if (x < 5 && Strong < 1)
                        {
                            Enemy_Spawn_Room("Strong");
                            Strong += 1;
                            break;
                        }
                        if ((x >= 65 && x < 80) || ((x >= 65) && BlindRange > 1))
                        {
                            Enemy_Spawn_Room("Aggressive");
                            Aggressive += 1;
                            break;
                        }
                        if (x >= 80 && BlindRange < 2)
                        {
                            Enemy_Spawn_Room("BlindRange");
                            BlindRange++;
                            break;
                        }
                        else
                        {
                            Enemy_Spawn_Room("Standart");
                            Standart++;
                            break;
                        }

                    case 2:
                        x = ran.Next(0, 100);
                        if (x < 10 && Strong < 2)
                        {
                            Enemy_Spawn_Room("Strong");
                            Strong += 1;
                            break;
                        }
                        if (x >= 45 && Aggressive < 2 && x < 80)
                        {
                            Enemy_Spawn_Room("Aggressive");
                            Aggressive += 1;
                            break;
                        }
                        if (x >= 65 && x < 80)
                        {
                            Enemy_Spawn_Room("Blind");
                            Blind += 1;
                            break;
                        }
                        if (x >= 80 && BlindRange < 2)
                        {
                            Enemy_Spawn_Room("BlindRange");
                            BlindRange++;
                            break;
                        }
                        else
                        {
                            Enemy_Spawn_Room("Standart");
                            Standart++;
                            break;
                        }

                    case 3:

                        x = ran.Next(0, 100);
                        if (x < 10 && Strong < 2)
                        {
                            Enemy_Spawn_Room("Strong");
                            Strong += 1;
                            break;
                        }
                        if (x >= 45 && Aggressive < 2 && x < 80)
                        {
                            Enemy_Spawn_Room("Aggressive");
                            Aggressive += 1;
                            break;
                        }
                        if (x >= 65 && x < 80)
                        {
                            Enemy_Spawn_Room("Blind");
                            Blind += 1;
                            break;
                        }
                        if (x >= 80 && BlindRange < 2)
                        {
                            Enemy_Spawn_Room("BlindRange");
                            BlindRange++;
                            break;
                        }
                        else
                        {
                            Enemy_Spawn_Room("Standart");
                            Standart++;
                            break;
                        }
                }
            }
            
        }


    }


}
