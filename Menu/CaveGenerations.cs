using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Menu
{
    internal class CaveGenerations
    {

        static int[,] Rooms;
        int RoomCounts;
        public Grid grid = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Width = 25000 * (App.Current.Windows[0].ActualHeight / 1080),
            Height = 25000 * (App.Current.Windows[0].ActualHeight / 1080),
            Margin = new Thickness(0, 0, 0, 0)
        };

        static string[,] Doors;
        public List<Room> RoomList = new List<Room>();
        public List<Doors> DoorsList = new List<Doors>();

        Player player;
        Caves cave;

        public CaveGenerations(Player player, Caves cave)
        {
            this.cave = cave;
            this.player = player;
            Rooms = new int[7, 7];
            Doors = new string[7, 7];
            Room_Location();
        }

        private void Room_Location()
        {
            Random ran = new Random();

            RoomCounts = ran.Next(player.Level * 3 + 6,9 + player.Level * 3);

            Rooms[3, 3] = 1;


            //RoomTypes:
            //Up
            //Down
            //Right
            //Left


            while (RoomCounts > 0)
            {
                for (int i = 0; i < Rooms.GetLength(0); i++)
                {
                    for (int j = 0; j < Rooms.GetLength(1); j++)
                    {
                        bool IsRoomCreate = false;
                        //Doors[i, j] = " ";
                        if (Rooms[i, j] == 1 && RoomCounts > 0)
                        {
                            if (i + 1 < Rooms.GetLength(0))
                            {
                                if (Rooms[i + 1, j] == 0 && ran.Next(1, 3) == 2 && RoomCounts > 0)
                                {
                                    Rooms[i + 1, j] = 1;
                                    RoomCounts--;
                                    IsRoomCreate = true;
                                    //Doors[i, j] += "Right ";
                                }
                            }

                            if (i - 1 >= 0)
                            {
                                if (Rooms[i - 1, j] == 0 && ran.Next(1, 3) == 2 && RoomCounts > 0)
                                {
                                    Rooms[i - 1, j] = 1;
                                    RoomCounts--;
                                    IsRoomCreate = true;
                                    //Doors[i, j] += "Left ";
                                }
                            }

                            if (j + 1 < Rooms.GetLength(1))
                            {
                                if (Rooms[i, j + 1] == 0 && ran.Next(1, 3) == 2 && RoomCounts > 0)
                                {
                                    Rooms[i, j + 1] = 1;
                                    RoomCounts--;
                                    IsRoomCreate = true;
                                    //Doors[i, j] += "Down ";
                                }
                            }

                            if (j - 1 >= 0)
                            {
                                if (Rooms[i, j - 1] == 0 && ran.Next(1, 3) == 2 && RoomCounts > 0)
                                {
                                    Rooms[i, j - 1] = 1;
                                    RoomCounts--;
                                    IsRoomCreate = true;
                                    //Doors[i, j] += "Up ";
                                }
                            }

                            if (IsRoomCreate == false)
                            {
                                Rooms[3, 3] = 1;
                            }
                            Rooms[i, j] = 2;
                        }
                    }
                }
            }

            // Старый генератор подземелья
            //     while (RoomCounts != 0)
            // {
            //     int x = ran.Next(1, 4);
            //     int y = ran.Next(1, 4);
            //     if ((Rooms[x + 1, y] == 1 || Rooms[x - 1, y] == 1 || Rooms[x, y + 1] == 1 || Rooms[x + 1, y - 1] == 1) && Rooms[x, y] == 0)
            //     {
            //         if (RoomCounts > 1)
            //         {
            //             Rooms[x, y] = 1;
            //             RoomCounts--;
            //         }
            //         else
            //         {
            //             Rooms[x, y] = 2;
            //             RoomCounts--;
            //         }
            //     }
            // }

            Room firstRoom = new Room("1", 3, 3, cave, "3" + " " + "3", player.Scaling);
            firstRoom.RoomModel.Fill = Brushes.Azure;
            grid.Children.Add(firstRoom.RoomModel);
            grid.Children.Add(firstRoom.RoomView);
            firstRoom.IsCleared = true;
            RoomList.Add(firstRoom);
            firstRoom.RoomBorder(grid);
            Rooms[3, 3] = 5;

            bool IsLastRoomCreated = false;
            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int j = 0; j < Rooms.GetLength(1); j++)
                {
                    if (Rooms[i, j] == 2)
                    {
                        Room room = new Room(Doors[i,j], j, i, cave, Convert.ToString(i) + " " + Convert.ToString(j), player.Scaling);
                        room.RoomModel.Fill = Brushes.Azure;
                        grid.Children.Add(room.RoomModel);
                        grid.Children.Add(room.RoomView);
                        //room.RoomDoors(grid);
                        room.RoomBorder(grid);
                        RoomList.Add(room);
                        Rooms[i, j] = 5;
                    }
                    if (Rooms[i, j] == 1 && IsLastRoomCreated == false)
                    {
                        IsLastRoomCreated = true;
                        Room LastRoom = new Room("Last", j, i, cave, Convert.ToString(i) + " " + Convert.ToString(j), player.Scaling);
                        LastRoom.RoomModel.Fill = Brushes.Red;
                        grid.Children.Add(LastRoom.RoomModel);
                        grid.Children.Add(LastRoom.RoomView);
                        //room.RoomDoors(grid);
                        LastRoom.RoomBorder(grid);
                        RoomList.Add(LastRoom);
                        Rooms[i, j] = 5;
                    }
                    else if (Rooms[i, j] == 1)
                    {
                        Room room = new Room(Doors[i, j], j, i, cave, Convert.ToString(i) + " " + Convert.ToString(j), player.Scaling);
                        room.RoomModel.Fill = Brushes.Azure;
                        grid.Children.Add(room.RoomModel);
                        grid.Children.Add(room.RoomView);
                        //room.RoomDoors(grid);
                        room.RoomBorder(grid);
                        RoomList.Add(room);
                        Rooms[i, j] = 5;
                    }
                }
            }


            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int j = 0; j < Rooms.GetLength(1); j++)
                {
                    if (Rooms[i, j] == 5)
                    {
                        if (j + 1 < Rooms.GetLength(1))
                        {
                            if (Rooms[i, j + 1] == 5)
                            {
                                Doors door = new Doors(Convert.ToString(i) + " " + Convert.ToString(j), Convert.ToString(i) + " " + Convert.ToString(j + 1), "HorizontalRight");
                                door.Door.Margin = new Thickness(j * (RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2) + RoomList[0].RoomModel.Width - door.Door.Width, i * (RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height / 2) + (RoomList[0].RoomModel.Height / 2) - door.Door.Height * 1/4, 0, 0);
                                door.DoorView.Margin = new Thickness(j * (RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2) + RoomList[0].RoomModel.Width - door.DoorView.Width, i * (RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height / 2) + (RoomList[0].RoomModel.Height / 2) - door.DoorView.Height*1/4, 0, 0);
                                // Добавить изображение для дверей
                                grid.Children.Add(door.Door);
                                grid.Children.Add(door.DoorView);
                                DoorsList.Add(door);
                            }
                        }
            
            
                        if (j - 1 >= 0)
                        {
                            if (Rooms[i, j - 1] == 5)
                            {
                                Doors door = new Doors(Convert.ToString(i) + " " + Convert.ToString(j), Convert.ToString(i) + " " + Convert.ToString(j - 1), "HorizontalLeft");
                                door.Door.Margin = new Thickness(j * (RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2), i * (RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height / 2) + (RoomList[0].RoomModel.Height / 2) - door.Door.Height * 1 / 4, 0, 0);
                                door.DoorView.Margin = new Thickness(j * (RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2), i * (RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height / 2) + (RoomList[0].RoomModel.Height / 2) - door.DoorView.Height * 1 / 4, 0, 0);
                                grid.Children.Add(door.Door);
                                grid.Children.Add(door.DoorView);
                                // Добавить изображение для дверей
                                DoorsList.Add(door);
                            }
                        }
            
            
                        if (i + 1 < Rooms.GetLength(0))
                        {
                            if (Rooms[i + 1, j] == 5)
                            {
                                Doors door = new Doors(Convert.ToString(i) + " " + Convert.ToString(j), Convert.ToString(i+1) + " " + Convert.ToString(j), "VerticalDown");
                                door.Door.Margin = new Thickness(j * (RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2) + (RoomList[0].RoomModel.Width / 2) - door.Door.Width / 2, i * (RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height / 2) + RoomList[0].RoomModel.Height - door.Door.Height, 0, 0);
                                door.DoorView.Margin = new Thickness(j * (RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2) + (RoomList[0].RoomModel.Width / 2) - door.DoorView.Width / 2, i * (RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height / 2) + RoomList[0].RoomModel.Height - door.DoorView.Height, 0, 0);
                                grid.Children.Add(door.Door);
                                grid.Children.Add(door.DoorView);
                                // Добавить изображение для дверей
                                DoorsList.Add(door);
                            }
                        }
            
                        if (i - 1 >= 0)
                        {
                            if (Rooms[i - 1, j] == 5)
                            {
                                Doors door = new Doors(Convert.ToString(i) + " " + Convert.ToString(j), Convert.ToString(i-1) + " " + Convert.ToString(j), "VerticalUp");
                                door.Door.Margin = new Thickness(j * (RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2) + RoomList[0].RoomModel.Width / 2 - door.Door.Width / 2, i * (RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height / 2), 0, 0);
                                door.DoorView.Margin = new Thickness(j * (RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2) + RoomList[0].RoomModel.Width / 2 - door.DoorView.Width / 2, i * (RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height / 2), 0, 0);
                                grid.Children.Add(door.Door);
                                grid.Children.Add(door.DoorView);
                                // Добавить изображение для дверей
                                DoorsList.Add(door);
                            }
                        }
                    }
                }
            }
        }
    }
}
