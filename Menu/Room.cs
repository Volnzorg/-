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

namespace Menu
{
    public class Room
    {
        string RoomType;
        public string Id;
        public bool IsCleared = false; // Проверка на то, что комната уже была зачищена.
        public bool IsPlayerIntersect = false;

        public Rectangle RoomModel = new Rectangle
        {
            Width = 800,
            Height = 600,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Stroke = Brushes.Black,
        };

        public Rect RoomHitBox;

        public List<Enemy> EnemiesList = new List<Enemy>();
        public List<Enemy> EnemyToDel = new List<Enemy>();

        double x, y;
        EnemySpawner spawner;
        Caves cave;

        public Room(String RoomType, double x, double y, Caves cave, string Id) 
        {
            this.Id = Id;
            this.cave = cave;
            this.RoomType=RoomType;
            this.x=x; this.y=y;
            RoomModel.Margin = new Thickness(0 + (x * RoomModel.Width),0 + (y * RoomModel.Height),0,0);
            RoomHitBox = new Rect(RoomModel.Margin.Left, RoomModel.Margin.Top, RoomModel.Width, RoomModel.Height);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += CameraUpdater;
            timer.Tick += RoomPlanning;
            timer.Start();
            //RoomFill("Last");
        }

        // Сделать через кейсы изображение комнаты


        public void RoomDoors()
        {
            
        }

        private void RoomPlanning(object sender, EventArgs e)
        {
            
            if (IsCleared == false && cave.IsIntersectRoom(RoomHitBox) == true && IsPlayerIntersect == false)
            {
                spawner = new EnemySpawner(this, cave.PlayerUpdater(), cave);
                spawner.Enemy_Spawn_Room(5, 1, cave.GridUpdater());
                cave.engine.EnemyList = EnemiesList; // Или оставить cave.engine.EnemyList = EnemiesList, для взаимодействия только с противниками данной комнаты?
                // Возможна проблема, что лист захломится
                IsPlayerIntersect = true;
            }

            if (cave.IsIntersectRoom(RoomHitBox))
            {
                if (cave.engine.EnemyList.Count == 0)
                {
                    IsCleared = true;
                }
            }

            if (IsCleared = false && cave.IsIntersectRoom(RoomHitBox))
            {
                // сделать стенки у комнат
            }

            if (RoomType == "Last" && IsCleared == true)
            {
                Image Coin = new Image
                {
                    Name = "Key",
                    Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/coin.png", UriKind.Absolute)),

                    Height = 30,
                    Width = 30,
                    Stretch = Stretch.Fill,
                    Tag = "Collectables",
                    Margin = new Thickness(RoomModel.Margin.Left + RoomModel.Width / 2, RoomModel.Margin.Top + RoomModel.Height / 2, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                cave.grid.Children.Add(Coin);
            }

            foreach (Enemy x in EnemiesList)
            {
                if (x.EnemyHP <= 0)
                {
                    x.Delete();
                    spawner.enemyCount--;
                }
            }
            foreach (Enemy x in EnemyToDel)
            {
                EnemiesList.Remove(x);
            }

        }

        //public void PlayerCameraPosition(Player player, Caves cave)
        //{
        //    if (player.PlayerHitBox.IntersectsWith(RoomHitBox))
        //    {
        //        cave.Content = RoomGrid;
        //    }
        //}

        private void CameraUpdater(object sender, EventArgs e)
        {
            cave.CameraUpdate(RoomHitBox);
        }
    }
}
