using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace Menu
{
    public class Collectables
    {
        public int DropChance;
        private int level;
        


        public Collectables(int level) 
        {
            this.level = level;
        }

        public Image EnemiesRandomDropping(double CoordX, double CoordY, string EnemyType)
        {
            Image Item = null;
            Random ran = new Random();

            bool IsChoosed = false;
            if (EnemyType == "Standart" || EnemyType == "Blind" || EnemyType == "BlindRange")
            {

                for (int i = 0; i < 4; i++) // 0 - монета, 1 - расходник, 2 - БаффХарактеристик, 3 - спец. Улучшение
                {
                    if (i == 0)
                    {
                        if (ran.Next(0, 100) < 27)
                        {
                            Item = new Image
                            {
                                Name = "coin",
                                Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/coin.png", UriKind.Absolute)),

                                Height = 30,
                                Width = 30,
                                Stretch = Stretch.Fill,
                                Tag = "Collectables",
                                Margin = new Thickness(CoordX, CoordY, 0, 0),
                                VerticalAlignment = VerticalAlignment.Top,
                                HorizontalAlignment = HorizontalAlignment.Left,
                            };
                            IsChoosed = true;
                            break;
                        }
                    }
                    if (i == 1 && IsChoosed == false)
                    {
                        if (ran.Next(0, 100) < 30)
                        {
                            int x = (ran.Next(0, 100));

                            if (x < 33)
                            {
                                Item = new Image
                                {
                                    Name = "Healthy",
                                    Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/Healthy.png", UriKind.Absolute)),

                                    Height = 30,
                                    Width = 30,
                                    Stretch = Stretch.Fill,
                                    Tag = "Collectables",
                                    Margin = new Thickness(CoordX, CoordY, 0, 0),
                                    VerticalAlignment = VerticalAlignment.Top,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                };
                                IsChoosed = true;
                                break;
                            }
                            if (x >= 33)
                            {
                                Item = new Image
                                {
                                    Name = "Stop",
                                    Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/Stop.png", UriKind.Absolute)),

                                    Height = 30,
                                    Width = 30,
                                    Stretch = Stretch.Fill,
                                    Tag = "Collectables",
                                    Margin = new Thickness(CoordX, CoordY, 0, 0),
                                    VerticalAlignment = VerticalAlignment.Top,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                };
                                IsChoosed = true;
                                break;
                            }
                        }
                    }
                    if (i == 2 && IsChoosed == false)
                    {
                        if (ran.Next(0, 100) < 25 + level*3)
                        {
                            switch (ran.Next(1, 4)) // Выбор из БаффХарактеристик
                            {
                                case 1:
                                    Item = new Image
                                    {
                                        Name = "HealthUp",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Trava.png", UriKind.Absolute)),
                                        Height = 50,
                                        Width = 50,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    //Item = TravaSpeedUp;
                                    IsChoosed = true;
                                    break;
                                case 2:
                                    Item = new Image
                                    {
                                        Name = "SpeedRun",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/SpeedRun.png", UriKind.Absolute)),
                                        Height = 50,
                                        Width = 50,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    //Item = TravaSpeedUp;
                                    IsChoosed = true;
                                    break;
                                case 3:
                                    Item = new Image
                                    {
                                        Name = "RoseOfWind",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/RoseOfWind.png", UriKind.Absolute)),
                                        Height = 50,
                                        Width = 50,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    //Item = TravaSpeedUp;
                                    IsChoosed = true;
                                    break;

                            }

                        }
                    }
                    if (i == 3 && IsChoosed == false)
                    {
                        if (ran.Next(0, 100) < 7 + level*2)
                        {
                            switch (ran.Next(1, 3)) // Выбор из спец. Улучшений
                            {
                                case 1:
                                    Item = new Image
                                    {
                                        Name = "Bubble",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/Special/Shield.png", UriKind.Absolute)),

                                        Height = 40,
                                        Width = 40,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    //Item = Bubble;
                                    IsChoosed = true;
                                    break;
                                case 2:
                                    Item = new Image
                                    {
                                        Name = "Counterattack",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/Special/Shield.png", UriKind.Absolute)),

                                        Height = 40,
                                        Width = 40,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    IsChoosed = true;
                                    break;
                            }

                        }
                    }
                }
            }


            if (EnemyType == "Strong" || EnemyType == "Aggressive")
            {
                for (int i = 0; i < 4; i++) // 0 - монета, 1 - расходник, 2 - БаффХарактеристик, 3 - спец. Улучшение
                {
                    if (i == 0)
                    {
                        if (ran.Next(0, 100) < 25)
                        {
                            Item = new Image
                            {
                                Name = "coin",
                                Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/coin.png", UriKind.Absolute)),

                                Height = 30,
                                Width = 30,
                                Stretch = Stretch.Fill,
                                Tag = "Collectables",
                                Margin = new Thickness(CoordX, CoordY, 0, 0),
                                VerticalAlignment = VerticalAlignment.Top,
                                HorizontalAlignment = HorizontalAlignment.Left,
                            };
                            IsChoosed = true;
                            break;
                        }
                    }
                    if (i == 1 && IsChoosed == false)
                    {
                        if (ran.Next(0, 100) < 25)
                        {
                            int x = (ran.Next(0, 100));

                            if (x < 33)
                            {
                                Item = new Image
                                {
                                    Name = "Healthy",
                                    Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/Healthy.png", UriKind.Absolute)),

                                    Height = 30,
                                    Width = 30,
                                    Stretch = Stretch.Fill,
                                    Tag = "Collectables",
                                    Margin = new Thickness(CoordX, CoordY, 0, 0),
                                    VerticalAlignment = VerticalAlignment.Top,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                };
                                IsChoosed = true;
                                break;
                            }
                            if (x >= 33)
                            {
                                Item = new Image
                                {
                                    Name = "Stop",
                                    Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/Stop.png", UriKind.Absolute)),

                                    Height = 30,
                                    Width = 30,
                                    Stretch = Stretch.Fill,
                                    Tag = "Collectables",
                                    Margin = new Thickness(CoordX, CoordY, 0, 0),
                                    VerticalAlignment = VerticalAlignment.Top,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                };
                                IsChoosed = true;
                                break;
                            }
                        }
                    }
                    if (i == 2 && IsChoosed == false)
                    {
                        if (ran.Next(0, 100) < 12 + level * 3)
                        {
                            switch (ran.Next(1, 3)) // Выбор из БаффХарактеристик
                            {
                                case 1:
                                    Item = new Image
                                    {
                                        Name = "HealthUp",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Trava.png", UriKind.Absolute)),
                                        Height = 50,
                                        Width = 50,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    //Item = TravaSpeedUp;
                                    IsChoosed = true;
                                    break;
                                case 2:
                                    Item = new Image
                                    {
                                        Name = "SpeedRun",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/SpeedRun.png", UriKind.Absolute)),
                                        Height = 50,
                                        Width = 50,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    //Item = TravaSpeedUp;
                                    IsChoosed = true;
                                    break;
                                case 3:
                                    Item = new Image
                                    {
                                        Name = "RoseOfWind",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/RoseOfWind.png", UriKind.Absolute)),
                                        Height = 50,
                                        Width = 50,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    //Item = TravaSpeedUp;
                                    IsChoosed = true;
                                    break;

                            }

                        }
                    }
                    if (i == 3 && IsChoosed == false)
                    {
                        if (ran.Next(0, 100) < 6 + level * 2)
                        {
                            switch (ran.Next(1, 3)) // Выбор из спец. Улучшений
                            {
                                case 1:
                                    Item = new Image
                                    {
                                        Name = "Bubble",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/Special/Shield.png", UriKind.Absolute)),

                                        Height = 40,
                                        Width = 40,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    //Item = Bubble;
                                    IsChoosed = true;
                                    break;
                                case 2:
                                    Item = new Image
                                    {
                                        Name = "Counterattack",
                                        Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/CollectableItem/Knockout.png", UriKind.Absolute)),

                                        Height = 40,
                                        Width = 40,
                                        Stretch = Stretch.Fill,
                                        Tag = "Collectables",
                                        Margin = new Thickness(CoordX, CoordY, 0, 0),
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    };
                                    IsChoosed = true;
                                    break;
                            }

                        }
                    }



                }
            }   return Item;
        }
    }
}
