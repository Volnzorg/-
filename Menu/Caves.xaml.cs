using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Menu
{
    /// <summary>
    /// Логика взаимодействия для Caves.xaml
    /// </summary>
    public partial class Caves : Window
    {
        public Player player;
        public Engine engine;
        CaveGenerations generations;
        public Grid grid;

        public List<Doors> DoorList;
        List<Room> RoomList;

        public Collectables Items;

        public Caves(Player player)
        {
            InitializeComponent();
            generations = new CaveGenerations(player, this);
            grid = generations.grid;
            this.player = player;
            GameScreen.Content = grid;
            engine = new Engine(player, grid, this);

            Items = new Collectables(player.Level);

            DispatcherTimer EnemiesChecked = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            //EnemiesChecked.Tick += EnemiesKilled;
            //EnemiesChecked.Tick += gridMoving;
            EnemiesChecked.Tick += DoorsConnection;
            EnemiesChecked.Start();
            if(DoorList != null)
            {
                DoorList.Clear();
            }
            
            DoorList = generations.DoorsList;
            RoomList = generations.RoomList;

            player.PlayerModelCreator();
            //player.PlayerModel.Margin = new Thickness((1200 + 300) * 3 + 400, (750 + 300) * 3 + 300, 0, 0); // Переделать под координаты первой комнаты, тк координата у нее постояннаая;
            player.PlayerModel.Margin = new Thickness(RoomList[0].RoomModel.Margin.Left + RoomList[0].RoomModel.Width / 2 - player.PlayerModel.Width / 2, RoomList[0].RoomModel.Margin.Top + RoomList[0].RoomModel.Height / 2 - player.PlayerModel.Height / 2, 0, 0);
            grid.Children.Add(player.PlayerModel);
            engine.GoUp = false; engine.GoDown = false; engine.GoLeft = false; engine.GoRight = false;
            player.Speed = player.PlayerCurrentSpeed;
            generations = null;
            grid.Background = Brushes.Black;
            player.IsKeyUp = false;
        }

        //public void Enemies(Enemy enem)
        //{
        //    engine.EnemyList.Add(enem);
        //}

        //public void EnemiesKilled(object sender, EventArgs e)
        //{
        //   foreach (Enemy x in engine.EnemyList)
        //   {
        //       if (x.Killed == true)
        //       {
        //           x.Delete(x);
        //           engine.EnemyDel.Add(x);
        //           //Spawner.enemyCount -= 1;
        //       }
        //   }
        //   foreach (Enemy x in engine.EnemyDel)
        //   {
        //       engine.EnemyList.Remove(x);
        //   }
        //}

        public void DoorsConnection(object sender, EventArgs e)
        {
           foreach (Doors door in DoorList)
           {
               Rect DoorHitBox = new Rect(door.Door.Margin.Left, door.Door.Margin.Top, door.Door.Width, door.Door.Height);
               if (player.PlayerHitBox.IntersectsWith(DoorHitBox))
               {
                   foreach (Room CurrentRoom in RoomList)
                   {
                       if (door.ThisRoomId == CurrentRoom.Id && CurrentRoom.IsCleared == true)
                       {
                           foreach (Room room in RoomList)
                           {
                                if (door.NextRoomId == room.Id)
                                {
                                    engine.RoomObjectsList.Clear();

                                    if (door.Type == "VerticalUp")
                                    {
                                        player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, room.RoomModel.Margin.Top + room.RoomModel.Height - door.DoorView.Height - player.PlayerModel.Height, 0, 0);
                                    }
                                    if (door.Type == "VerticalDown")
                                    {
                                        player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, room.RoomModel.Margin.Top + door.DoorView.Height, 0, 0);
                                    }

                                    if (door.Type == "HorizontalLeft")
                                    {
                                        player.PlayerModel.Margin = new Thickness(room.RoomModel.Margin.Left + room.RoomModel.Width - door.DoorView.Width - player.PlayerModel.Width - 20, player.PlayerModel.Margin.Top, 0, 0);
                                    }
                                    if (door.Type == "HorizontalRight")
                                    {
                                        player.PlayerModel.Margin = new Thickness(room.RoomModel.Margin.Left + door.DoorView.Width, player.PlayerModel.Margin.Top, 0, 0);
                                    }
                                }  
                           }
           
                           //player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left - (x.Margin.Left - (room.RoomModel.Margin.Left + room.RoomModel.Width / 2)), player.PlayerModel.Margin.Top - (x.Margin.Top - (room.RoomModel.Margin.Top + room.RoomModel.Height / 2)), 0, 0);
                           // По любому можно сделать красивое уравнение
                       }
                   }
               }
           }

           
           
           
           
           
           //foreach (Rectangle x in grid.Children.OfType<Rectangle>())
           //{
           //    if ((string)x.Name == "Door")
           //    {
           //        Rect DoorHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.Width, x.Height);
           //        if (player.PlayerHitBox.IntersectsWith(DoorHitBox))
           //        {
           //            foreach (Room room in RoomList)
           //            {
           //                if ((string)x.Tag == room.Id)
           //                {
           //                    if ((x.Margin.Left - (room.RoomModel.Margin.Left + room.RoomModel.Width/2)) == 0)
           //                    {
           //                        if ((x.Margin.Top - room.RoomModel.Margin.Top) > 0)
           //                        {
           //                            player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top - (x.Height * 3) - 20, 0, 0);
           //                        }
           //                        else
           //                        {
           //                            player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, player.PlayerModel.Margin.Top + (x.Height * 3) + 20, 0, 0);
           //                        }
           //                    }
           //                    else
           //                    {
           //                        if (x.Margin.Left - room.RoomModel.Margin.Left > 0)
           //                        {
           //                            player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left - (x.Width * 3) - 20, player.PlayerModel.Margin.Top, 0, 0);
           //                        }
           //                        else
           //                        {
           //                            player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left + (x.Width * 3) + 20, player.PlayerModel.Margin.Top, 0, 0);
           //                        }
           //                    }
           //                    //player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left - (x.Margin.Left - (room.RoomModel.Margin.Left + room.RoomModel.Width / 2)), player.PlayerModel.Margin.Top - (x.Margin.Top - (room.RoomModel.Margin.Top + room.RoomModel.Height / 2)), 0, 0);
           //                    // По любому можно сделать красивое уравнение
           //                }
           //            }
           //        }
           //    }
           //    
           //}


            //Room nextRoom = null;
            //double newRoomCoordX = 0;
            //double newRoomCoordY = 0;
            //double minX = 100000;
            //double minY = 100000;
            //foreach (Rectangle x in grid.Children.OfType<Rectangle>())
            //{
            //    if ((string)x.Name == "Door")
            //    {
            //        Rect DoorHitBox = new Rect(x.Margin.Left, x.Margin.Top, x.Width, x.Height);
            //        if (player.PlayerHitBox.IntersectsWith(DoorHitBox)) // Переделать, чтобы пока в комнате враги, через дверь двигаться нельзя, иначе сквозь дверь можно проходить. Пока персонаж взаимодействует с комнатой, то грид находиться, на комнате.
            //        {
            //            if ((string)x.Tag == "HorizontalDoor")
            //            {
            //                foreach (Rectangle y in grid.Children.OfType<Rectangle>())
            //                {
            //                    if((string)y.Tag == "HorizontalDoor" && y != x)
            //                    {
            //                        if (minX < Math.Abs(x.Margin.Left - y.Margin.Left))
            //                        {
            //                            newRoomCoordX = y.Margin.Left;
            //                            minX = Math.Abs(x.Margin.Left - y.Margin.Left);
            //                        }
            //                    }
            //                }
            //                player.PlayerModel.Margin = new Thickness(newRoomCoordX, player.PlayerModel.Margin.Top, 0, 0);
            //            }
            //            else
            //            {
            //                foreach (Rectangle y in grid.Children.OfType<Rectangle>())
            //                {
            //                    if ((string)y.Tag == "VerticalDoor" && y != x)
            //                    {
            //                        if (minY < Math.Abs(x.Margin.Top - y.Margin.Top))
            //                        {
            //                            newRoomCoordY = y.Margin.Top;
            //                            minY = Math.Abs(x.Margin.Top - y.Margin.Top);
            //                        }
            //                    }
            //                }
            //
            //                player.PlayerModel.Margin = new Thickness(player.PlayerModel.Margin.Left, newRoomCoordY, 0, 0);
            //            }
            //        }
            //    }
            //}
        }

        public void CameraUpdate(Rect RoomHB, double x, double y)
        {
            if (player.PlayerHitBox.IntersectsWith(RoomHB))
            {
                grid.Margin = new Thickness(0 - RoomHB.X + RoomHB.Width / 4, 0 - RoomHB.Y + RoomHB.Height / 4, 0, 0);
            }
            engine.InterfaceCoordX = x; 
            engine.InterfaceCoordY = y;
        }

        public bool IsIntersectRoom(Rect RoomHB)
        {
            if (player.PlayerHitBox.IntersectsWith(RoomHB))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Grid GridUpdater()
        {
            return grid;
        }

        public Player PlayerUpdater()
        {
            return player;
        }
    }
}
