using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Menu
{
    public class Doors
    {
        public string ThisRoomId;
        public string NextRoomId;
        public string Type;

        public Rectangle Door = new Rectangle()
        {
            Name = "Door",
            Height = 30,
            Width = 60,
            Fill = Brushes.Coral,
            Stroke = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 0, 0, 0)
        };

        BitmapImage ClosedRightDoor = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Doors/ClosedRightDoor.png", UriKind.Absolute));
        BitmapImage ClosedLeftDoor = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Doors/ClosedLeftDoor.png", UriKind.Absolute));
        BitmapImage ClosedUpDoor = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Cave/Doors/ClosedUpDoor.png", UriKind.Absolute));

        public Image DoorView = new Image()
        {
            Height = 100,
            Width = 100,
            Stretch = Stretch.Fill,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 0, 0, 0)
        };

        public Doors(string ThisRoomId, string NextRoomId, string Type) 
        {
            this.Type = Type;
            this.NextRoomId = NextRoomId;
            this.ThisRoomId = ThisRoomId;
            if (Type == "HorizontalLeft")
            {
                Door.Height = 60; Door.Width = 25;
                DoorView.Source = ClosedLeftDoor;
                DoorView.Width = 30; DoorView.Height = 120;
            }

            if (Type == "HorizontalRight")
            {
                Door.Height = 60; Door.Width = 30;
                DoorView.Source = ClosedRightDoor;
                DoorView.Width = 30; DoorView.Height = 120;
            }

            if (Type == "VerticalUp")
            {
                Door.Height = 60; Door.Width = 60;
                DoorView.Source = ClosedUpDoor;
            }

            if (Type == "VerticalDown")
            {
                Door.Height = 60; Door.Width = 60;
                DoorView.Source = ClosedUpDoor;
            }
        }
    }
}
