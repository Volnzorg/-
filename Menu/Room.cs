using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using System.CodeDom;

namespace Menu
{
    public class Room
    {
        string RoomType;
        public string Id;
        public bool IsCleared = false; // Проверка на то, что комната уже была зачищена.
        public bool IsPlayerIntersect = false;

        bool IsChestSpawned = true; // true - Будет иметь шнас заспавнится,  false - нет;

        bool IsRolled = false;
        bool IsKeySpawned = false;

        bool IsDoorClosed = false;
        int enemyCount;

        public Rectangle RoomModel = new Rectangle
        {
            Width = 1200,
            Height = 750,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Stroke = Brushes.Black,
        };

        public Rect RoomHitBox;

        public List<Enemy> EnemiesList = new List<Enemy>();
        public List<Enemy> EnemyToDel = new List<Enemy>();

        EnemySpawner spawner;
        Caves cave;

        Random ran = new Random();

        TextBox check = new TextBox
        {
            Height = 70,
            Width = 70,
            Margin = new Thickness(0, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Text = ""
        };
        public Image RoomView = new Image
        {
            Width = 1200,
            Height = 750,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Stretch = Stretch.Fill,
            Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Rooms/CaveRoom.png", UriKind.Absolute)), // Переделать в далеком будущем
        };

        BitmapImage ClosedChest = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Chest/Chest_Closed.png", UriKind.Absolute));

        BitmapImage OpenedUpDoor = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Doors/OpenedUpDoor.png", UriKind.Absolute));
        BitmapImage OpenedLeftDoor = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Doors/OpenLeftDoor.png", UriKind.Absolute));
        BitmapImage OpenedRightDoor = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Doors/OpenRightDoor.png", UriKind.Absolute));

        double Scaling;

        public Room(String RoomType, double x, double y, Caves cave, string Id, double Scaling) 
        {
            this.Id = Id;
            this.cave = cave;
            this.RoomType=RoomType;
            this.Scaling = Scaling;

            RoomModel.Width *= Scaling;
            RoomModel.Height *= Scaling;

            RoomView.Width = RoomModel.Width;
            RoomView.Height = RoomModel.Height;

            RoomModel.Margin = new Thickness(0 + (x * (RoomModel.Width + RoomModel.Width / 2)),0 + (y * (RoomModel.Height + RoomModel.Height / 2)),0,0);
            RoomHitBox = new Rect(RoomModel.Margin.Left, RoomModel.Margin.Top, RoomModel.Width, RoomModel.Height);
            RoomView.Margin = new Thickness(0 + (x * (RoomModel.Width + RoomModel.Width / 2)), 0 + (y * (RoomModel.Height + RoomModel.Height / 2)), 0, 0);

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            timer.Tick += RoomPlanning;
            timer.Start();
            //RoomFill("Last");
        }

        // Сделать через кейсы изображение комнаты

        Rectangle DownBorder, LeftBorder, RightBorder, UpBorder;

        public void RoomBorder(Grid grid)
        {
            //Rect LeftBorder = new Rect(RoomModel.Margin.Left, RoomModel.Margin.Top, 10, RoomModel.Height);
            LeftBorder = new Rectangle
            {
                Tag = "Border",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 10 * Scaling + RoomModel.Width / 4,
                Height = RoomModel.Height,
                Margin = new Thickness(RoomModel.Margin.Left - RoomModel.Width / 4, RoomModel.Margin.Top, 0, 0),
                //Stroke = Brushes.White

            };
            RightBorder = new Rectangle
            {
                Tag = "Border",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 10 * Scaling + RoomModel.Width / 4,
                Height = RoomModel.Height,
                Margin = new Thickness(RoomModel.Margin.Left + RoomModel.Width - 10 * Scaling, RoomModel.Margin.Top, 0, 0),
                //Stroke = Brushes.White
            };
            UpBorder = new Rectangle
            {
                Tag = "Border",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = RoomModel.Width,
                Height = 50 * Scaling + RoomModel.Height / 4,
                Margin = new Thickness(RoomModel.Margin.Left, RoomModel.Margin.Top - RoomModel.Height / 4, 0, 0),
                //Stroke = Brushes.White
            };
            DownBorder = new Rectangle
            {
                Tag = "Border",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = RoomModel.Width,
                Height = 10 * Scaling + RoomModel.Height / 4,
                Margin = new Thickness(RoomModel.Margin.Left, RoomModel.Margin.Top + RoomModel.Height - 10*Scaling, 0, 0),
                //Stroke = Brushes.White
            };

            check.Margin = new Thickness(RoomModel.Margin.Left, RoomModel.Margin.Top, 0, 0);
            grid.Children.Add(UpBorder); grid.Children.Add(DownBorder); grid.Children.Add(LeftBorder); grid.Children.Add(RightBorder); grid.Children.Add(check);
            
        }

        private void RoomPlanning(object sender, EventArgs e)
        {
            
            if (IsCleared == false && cave.IsIntersectRoom(RoomHitBox) == true && IsPlayerIntersect == false)
            {
                cave.player.Immunity = true;
                cave.player.Shield = cave.player.MaxShield;
                cave.player.IsEnemyFrozen = false;
                cave.engine.RoomObjectsList.Add(UpBorder); cave.engine.RoomObjectsList.Add(DownBorder); cave.engine.RoomObjectsList.Add(LeftBorder); cave.engine.RoomObjectsList.Add(RightBorder);
                
                spawner = new EnemySpawner(this, cave.PlayerUpdater(), cave.GridUpdater());      
                enemyCount = ran.Next(cave.player.Level + 3, cave.player.Level + 6);
                spawner.RandomEnemyTypes(enemyCount);

                if (RoomType == "Last")
                {
                    spawner.Enemy_Spawn_Room("Strong");
                    enemyCount++;
                }
                cave.engine.EnemyList = EnemiesList; // Или оставить cave.engine.EnemyList = EnemiesList, для взаимодействия только с противниками данной комнаты?
                // Возможна проблема, что лист захломится
                IsPlayerIntersect = true;
            }

            if (cave.IsIntersectRoom(RoomHitBox))
            {
                check.Text = enemyCount.ToString();

                if (enemyCount == 0)
                {
                    IsCleared = true;
                    EnemiesList.Clear();
                    cave.engine.EnemyList.Clear();

                    if (RoomType != "Last" && IsRolled == false && IsChestSpawned == false && RoomType != "1")
                    {
                        IsRolled = true;
                        if (ran.Next(0, 100) < 15 + cave.player.Level + 2)
                        {
                            double x = 60 * cave.player.Scaling;
                            Image chest = new Image()
                            {
                                Height = x,
                                Width = x,
                                VerticalAlignment = VerticalAlignment.Top,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Tag = "Closed",
                                Name = "Chest",
                                Source = ClosedChest,
                                Stretch = Stretch.Fill,
                                Margin = new Thickness(RoomModel.Margin.Left + RoomModel.Width / 2 - x / 2, RoomModel.Margin.Top + RoomModel.Height / 2 - x / 2, 0, 0)
                            };
                            cave.grid.Children.Add(chest);
                            IsChestSpawned = true;
                        }
                        
                    }

                    foreach (Doors door in cave.DoorList)
                    {
                        if (door.ThisRoomId == Id)
                        {
                            if (door.Type == "VerticalUp")
                            {
                                door.DoorView.Source = OpenedUpDoor;
                            }

                            if (door.Type == "VerticalDown")
                            {
                                door.DoorView.Source = OpenedUpDoor;
                            }

                            if (door.Type == "HorizontalRight")
                            {
                                door.DoorView.Source = OpenedRightDoor;
                            }

                            if (door.Type == "HorizontalLeft")
                            {
                                door.DoorView.Source = OpenedLeftDoor;
                            }
                        }
                    }
                }

                CameraUpdater();

            }

            if (RoomType == "Last" && IsCleared == true && IsKeySpawned == false)
            {
                IsKeySpawned = true;
                Image Coin = new Image
                {
                    Name = "Key",
                    Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/key.png", UriKind.Absolute)),

                    Height = 30,
                    Width = 30,
                    Stretch = Stretch.Fill,
                    Tag = "Collectables",
                    Margin = new Thickness(RoomModel.Margin.Left + RoomModel.Width / 2, RoomModel.Margin.Top + RoomModel.Height / 2, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };

                Image Ledder = new Image()
                {
                    Height = 100,
                    Width = 100,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Stretch = Stretch.Fill,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Doors/CaveExit_Ladder.png", UriKind.Absolute)),
                    Tag = "CaveExit",
                    Margin = new Thickness(RoomModel.Margin.Left + RoomModel.Width / 4, RoomModel.Margin.Top - 10 * Scaling, 0, 0)
                };

                cave.grid.Children.Add(Ledder);
                cave.grid.Children.Add(Coin);


            }

            foreach (Enemy x in EnemiesList)
            {
                if (x.EnemyHP <= 0 && x.Killed == false)
                {
                    x.Killed = true;
                    Image DropItem = cave.Items.EnemiesRandomDropping(x.EnemyModel.Margin.Left, x.EnemyModel.Margin.Top, x.EnemyType);
                    if (DropItem != null)
                    {
                        cave.grid.Children.Add(DropItem);
                    }
                    EnemyToDel.Add(x);
                    x.Delete(x);
                    enemyCount--;
                    
                }
            }

        }

        //public void PlayerCameraPosition(Player player, Caves cave)
        //{
        //    if (player.PlayerHitBox.IntersectsWith(RoomHitBox))
        //    {
        //        cave.Content = RoomGrid;
        //    }
        //}

        private void CameraUpdater()
        {
            cave.CameraUpdate(RoomHitBox, RoomView.Margin.Left, RoomView.Margin.Top);
        }
    }
}
