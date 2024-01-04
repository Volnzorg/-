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
        int Difficult;

        int[,] Rooms;
        int RoomCounts;
        public Grid grid = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Width = 7 * 800,
            Height = 7 * 600,
            Margin = new Thickness(0, 0, 0, 0)
        };

        string[,] Doors;
        public List<Room> RoomList = new List<Room>();

        Player player;
        Caves cave;

        public CaveGenerations(int Difficult, Player player, Caves cave)
        {
            this.cave = cave;
            this.Difficult = Difficult;
            this.player = player;
            Rooms = new int[7, 7];
            Doors = new string[7, 7];
            Room_Location();
        }

        private void Room_Location()
        {
            Random ran = new Random();
            if (Difficult == 0)
            {
                RoomCounts = ran.Next(6,9);
            }


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
                        Doors[i, j] = " ";
                        if (Rooms[i, j] == 1 && RoomCounts > 0)
                        {
                            if (i + 1 < Rooms.GetLength(0))
                            {
                                if (Rooms[i + 1, j] == 0 && ran.Next(1, 3) == 2 && RoomCounts > 0)
                                {
                                    Rooms[i + 1, j] = 1;
                                    RoomCounts--;
                                    IsRoomCreate = true;
                                    Doors[i, j] += "Right ";
                                }
                            }

                            if (i - 1 >= 0)
                            {
                                if (Rooms[i - 1, j] == 0 && ran.Next(1, 3) == 2 && RoomCounts > 0)
                                {
                                    Rooms[i - 1, j] = 1;
                                    RoomCounts--;
                                    IsRoomCreate = true;
                                    Doors[i, j] += "Left ";
                                }
                            }

                            if (j + 1 < Rooms.GetLength(1))
                            {
                                if (Rooms[i, j + 1] == 0 && ran.Next(1, 3) == 2 && RoomCounts > 0)
                                {
                                    Rooms[i, j + 1] = 1;
                                    RoomCounts--;
                                    IsRoomCreate = true;
                                    Doors[i, j] += "Down ";
                                }
                            }

                            if (j - 1 >= 0)
                            {
                                if (Rooms[i, j - 1] == 0 && ran.Next(1, 3) == 2 && RoomCounts > 0)
                                {
                                    Rooms[i, j - 1] = 1;
                                    RoomCounts--;
                                    IsRoomCreate = true;
                                    Doors[i, j] += "Up ";
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

            Room firstRoom = new Room("1", 3, 3, cave, "3" + "3");
            firstRoom.RoomModel.Fill = Brushes.Azure;
            grid.Children.Add(firstRoom.RoomModel);
            firstRoom.IsCleared = true;
            RoomList.Add(firstRoom);
            Rooms[3, 3] = 0;

            bool IsLastRoomCreated = false;
            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int j = 0; j < Rooms.GetLength(1); j++)
                {
                    if (Rooms[i, j] == 2)
                    {
                        Room room = new Room(Doors[i,j], j, i, cave, Convert.ToString(i) + " " + Convert.ToString(j));
                        room.RoomModel.Fill = Brushes.Azure;
                        grid.Children.Add(room.RoomModel);
                        //room.RoomDoors(grid);
                        RoomList.Add(room);
                        Rooms[i, j] = 5;
                    }
                    if (Rooms[i, j] == 1 && IsLastRoomCreated == false)
                    {
                        IsLastRoomCreated = true;
                        Room LastRoom = new Room("Last", j, i, cave, Convert.ToString(i) + " " + Convert.ToString(j));
                        LastRoom.RoomModel.Fill = Brushes.Red;
                        grid.Children.Add(LastRoom.RoomModel);
                        //room.RoomDoors(grid);
                        RoomList.Add(LastRoom);
                        Rooms[i, j] = 5;
                    }
                    else if (Rooms[i, j] == 1)
                    {
                        Room room = new Room(Doors[i, j], j, i, cave, Convert.ToString(i) + " " + Convert.ToString(j));
                        room.RoomModel.Fill = Brushes.Azure;
                        grid.Children.Add(room.RoomModel);
                        //room.RoomDoors(grid);
                        RoomList.Add(room);
                        Rooms[i, j] = 5;
                    }
                }
            }


            //for (int i = 0; i < Doors.GetLength(0); i++)
            //{
            //    for(int j = 0;j < Doors.GetLength(1); j++)
            //    {
            //        if (Doors[i, j].Contains("Right") == true)
            //        {
            //            Rectangle Door = new Rectangle
            //            {
            //                Name = "Door",
            //                Tag = "HorizontalDoor",
            //                Height = 30,
            //                Width = 60,
            //                Fill = Brushes.Coral,
            //                Stroke = Brushes.White,
            //                HorizontalAlignment = HorizontalAlignment.Left,
            //                VerticalAlignment = VerticalAlignment.Top,
            //                Margin = new Thickness(0, 0, 0, 0)
            //            };
            //            Door.Margin = new Thickness(j * RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width - Door.Width, i * RoomList[0].RoomModel.Height + (RoomList[0].RoomModel.Height / 2), 0, 0);
            //            grid.Children.Add(Door);
            //        }
            //
            //        if (Doors[i, j].Contains("Left") == true)
            //        {
            //            Rectangle Door = new Rectangle
            //            {
            //                Name = "Door",
            //                Tag = "HorizontalDoor",
            //                Height = 30,
            //                Width = 60,
            //                Fill = Brushes.Coral,
            //                Stroke = Brushes.White,
            //                HorizontalAlignment = HorizontalAlignment.Left,
            //                VerticalAlignment = VerticalAlignment.Top,
            //                Margin = new Thickness(0, 0, 0, 0)
            //            };
            //            Door.Margin = new Thickness(j * RoomList[0].RoomModel.Width, i * RoomList[0].RoomModel.Height + (RoomList[0].RoomModel.Height / 2), 0, 0);
            //            grid.Children.Add(Door);
            //        }
            //
            //        if (Doors[i, j].Contains("Up") == true)
            //        {
            //            Rectangle Door = new Rectangle
            //            {
            //                Name = "Door",
            //                Tag = "VerticalDoor",
            //                Height = 60,
            //                Width = 30,
            //                Fill = Brushes.Coral,
            //                Stroke = Brushes.White,
            //                HorizontalAlignment = HorizontalAlignment.Left,
            //                VerticalAlignment = VerticalAlignment.Top,
            //                Margin = new Thickness(0, 0, 0, 0)
            //            };
            //            Door.Margin = new Thickness(j * RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2, i * RoomList[0].RoomModel.Height, 0, 0);
            //            grid.Children.Add(Door);
            //        }
            //
            //        if (Doors[i, j].Contains("Down") == true)
            //        {
            //            Rectangle Door = new Rectangle
            //            {
            //                Name = "Door",
            //                Tag = "VerticalDoor",
            //                Height = 60,
            //                Width = 30,
            //                Fill = Brushes.Coral,
            //                Stroke = Brushes.White,
            //                HorizontalAlignment = HorizontalAlignment.Left,
            //                VerticalAlignment = VerticalAlignment.Top,
            //                Margin = new Thickness(0, 0, 0, 0)
            //            };
            //            Door.Margin = new Thickness(j * RoomList[0].RoomModel.Width + (RoomList[0].RoomModel.Width / 2), i * RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height - Door.Height, 0, 0);
            //            grid.Children.Add(Door);
            //        }
            //    }
            //}


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
                                Rectangle Door = new Rectangle
                                {
                                    Name = "Door",
                                    Tag = Convert.ToString(i) + " " + Convert.ToString(j + 1),
                                    Height = 30,
                                    Width = 60,
                                    Fill = Brushes.Coral,
                                    Stroke = Brushes.White,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    VerticalAlignment = VerticalAlignment.Top,
                                    Margin = new Thickness(0, 0, 0, 0)
                                };
                                Door.Margin = new Thickness(j * RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width - Door.Width, i * RoomList[0].RoomModel.Height + (RoomList[0].RoomModel.Height / 2), 0, 0);
                                grid.Children.Add(Door);
                            }
                        }
            
            
                        if (j - 1 >= 0)
                        {
                            if (Rooms[i, j - 1] == 5)
                            {
                                Rectangle Door = new Rectangle
                                {
                                    Name = "Door",
                                    Tag = Convert.ToString(i) + " " + Convert.ToString(j-1),
                                    Height = 30,
                                    Width = 60,
                                    Fill = Brushes.Coral,
                                    Stroke = Brushes.White,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    VerticalAlignment = VerticalAlignment.Top,
                                    Margin = new Thickness(0, 0, 0, 0)
                                };
                                Door.Margin = new Thickness(j * RoomList[0].RoomModel.Width, i * RoomList[0].RoomModel.Height + (RoomList[0].RoomModel.Height / 2), 0, 0);
                                grid.Children.Add(Door);
                            }
                        }
            
            
                        if (i + 1 < Rooms.GetLength(0))
                        {
                            if (Rooms[i + 1, j] == 5)
                            {
                                Rectangle Door = new Rectangle
                                {
                                    Name = "Door",
                                    Tag = Convert.ToString(i + 1) + " " + Convert.ToString(j),
                                    Height = 60,
                                    Width = 30,
                                    Fill = Brushes.Coral,
                                    Stroke = Brushes.White,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    VerticalAlignment = VerticalAlignment.Top,
                                    Margin = new Thickness(0, 0, 0, 0)
                                };
                                Door.Margin = new Thickness(j * RoomList[0].RoomModel.Width + (RoomList[0].RoomModel.Width / 2), i * RoomList[0].RoomModel.Height + RoomList[0].RoomModel.Height - Door.Height, 0, 0);
                                grid.Children.Add(Door);
                            }
                        }
            
                        if (i - 1 >= 0)
                        {
                            if (Rooms[i - 1, j] == 5)
                            {
                                Rectangle Door = new Rectangle
                                {
                                    Name = "Door",
                                    Tag = Convert.ToString(i - 1) + " " + Convert.ToString(j),
                                    Height = 60,
                                    Width = 30,
                                    Fill = Brushes.Coral,
                                    Stroke = Brushes.White,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    VerticalAlignment = VerticalAlignment.Top,
                                    Margin = new Thickness(0, 0, 0, 0)
                                };
                                Door.Margin = new Thickness(j * RoomList[0].RoomModel.Width + RoomList[0].RoomModel.Width / 2, i * RoomList[0].RoomModel.Height, 0, 0);
                                grid.Children.Add(Door);
                            }
                        }
            
                    }
            
            
            
            
                    
                }
            }


            //for (int i = 0; i < RoomList.Count; i++)
            //{
            //    for(int j = 0; j < RoomList.Count; j++)
            //    {
            //        if (RoomList[i].RoomModel.Margin.Left + RoomList[i].RoomModel.Width == RoomList[j].RoomModel.Margin.Left)
            //        {
            //            Rectangle Door = new Rectangle
            //            {
            //                Tag = "Door",
            //                Height = 60,
            //                Width = 30,
            //                Fill = Brushes.Coral,
            //                Stroke = Brushes.White,
            //                HorizontalAlignment = HorizontalAlignment.Left,
            //                VerticalAlignment = VerticalAlignment.Top,
            //                Margin = new Thickness(0, 0, 0, 0)
            //            };
            //            Door.Margin = new Thickness(RoomList[i].RoomModel.Margin.Left + RoomList[i].RoomModel.Width - Door.Width, RoomList[i].RoomModel.Margin.Top + RoomList[i].RoomModel.Height / 2, 0, 0);
            //            grid.Children.Add(Door);
            //            break;
            //        }
            //        if (RoomList[i].RoomModel.Margin.Left == RoomList[j].RoomModel.Margin.Left + RoomList[j].RoomModel.Width)
            //        {
            //            Rectangle Door = new Rectangle
            //            {
            //                Tag = "Door",
            //                Height = 60,
            //                Width = 30,
            //                Fill = Brushes.Coral,
            //                Stroke = Brushes.White,
            //                HorizontalAlignment = HorizontalAlignment.Left,
            //                VerticalAlignment = VerticalAlignment.Top,
            //                Margin = new Thickness(0, 0, 0, 0)
            //            };
            //            Door.Margin = new Thickness(RoomList[i].RoomModel.Margin.Left + Door.Width, RoomList[i].RoomModel.Margin.Top + RoomList[i].RoomModel.Height / 2, 0, 0);
            //            grid.Children.Add(Door);
            //            break;
            //        }
            //
            //        if (RoomList[i].RoomModel.Margin.Top == RoomList[j].RoomModel.Margin.Top + RoomList[j].RoomModel.Height)
            //        {
            //            Rectangle Door = new Rectangle
            //            {
            //                Tag = "Door",
            //                Height = 60,
            //                Width = 30,
            //                Fill = Brushes.Coral,
            //                Stroke = Brushes.White,
            //                HorizontalAlignment = HorizontalAlignment.Left,
            //                VerticalAlignment = VerticalAlignment.Top,
            //                Margin = new Thickness(0, 0, 0, 0)
            //            };
            //            Door.Margin = new Thickness(RoomList[i].RoomModel.Margin.Left + RoomList[i].RoomModel.Width / 2, RoomList[i].RoomModel.Margin.Top, 0, 0);
            //            grid.Children.Add(Door);
            //            break;
            //        }
            //
            //        if (RoomList[i].RoomModel.Margin.Top + RoomList[i].RoomModel.Height == RoomList[j].RoomModel.Margin.Top)
            //        {
            //            Rectangle Door = new Rectangle
            //            {
            //
            //                Tag = "Door",
            //                Height = 60,
            //                Width = 30,
            //                Fill = Brushes.Coral,
            //                Stroke = Brushes.White,
            //                HorizontalAlignment = HorizontalAlignment.Left,
            //                VerticalAlignment = VerticalAlignment.Top,
            //                Margin = new Thickness(0, 0, 0, 0)
            //            };
            //            Door.Margin = new Thickness(RoomList[i].RoomModel.Margin.Left + RoomList[i].RoomModel.Width / 2, RoomList[i].RoomModel.Margin.Top + RoomList[i].RoomModel.Height - Door.Height, 0, 0);
            //            grid.Children.Add(Door);
            //            break;
            //        }
            //
            //
            //
            //    }
            //}

            



        }
    }
}
