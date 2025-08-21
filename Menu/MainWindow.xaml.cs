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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Menu
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static Image StartImage = new Image()
        {
            Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Menu/Play.png", UriKind.Absolute)),
            Stretch = Stretch.Fill
        };
        
        static StackPanel Start = new StackPanel()
        {
            Margin = new Thickness(0, 0, 0, 0),
            Children = {StartImage},
        };

        static Image ExitImage = new Image()
        {
            Source = new BitmapImage(new Uri("pack://application:,,,/Menu;component/Images/Menu/Quit.png", UriKind.Absolute)),
            Stretch = Stretch.Fill
        };

        static StackPanel ExitB = new StackPanel()
        {
            Margin = new Thickness(0, 0, 0, 0),
            Children = { ExitImage },
        };

        Button NewGame = new Button
        {
            Margin = new Thickness(0, -100, 0, 0),
            Width = 220,
            Height = 100,
            Content = Start,
            
        };
        Button Options = new Button
        {
            Margin = new Thickness(0, 100, 0, 0),
            Width = 100,
            Height = 60,
            Foreground = Brushes.Black,
            Content = "Options",


        };
        Button Exit = new Button
        {
            Margin = new Thickness(0, 300, 0, 0),
            Width = 220,
            Height = 100,
            Content = ExitB,
        };
        Button New = new Button
        {
            Margin = new Thickness(0, -100, 0, 0),
            Width = 100,
            Height = 60,
            Foreground = Brushes.Black,
            Content = "New",

        };
        Button Cont = new Button
        {
            Margin = new Thickness(0, 100, 0, 0),
            Width = 100,
            Height = 60,
            Foreground = Brushes.Black,
            Content = "Continius"

        };
        Button Back = new Button
        {
            Margin = new Thickness(0, 300, 0, 0),
            Width = 100,
            Height = 60,
            Foreground = Brushes.Black,
            Content = "Back"
        };
        Button Graphics = new Button
        {
            Margin = new Thickness(0, -100, 0, 0),
            Width = 100,
            Height = 60,
            Foreground = Brushes.Black,
            Content = "Graphics"

        };
        Button Contrlos = new Button
        {
            Margin = new Thickness(0, 100, 0, 0),
            Width = 100,
            Height = 60,
            Foreground = Brushes.Black,
            Content = "Controls"

        };

        public MainWindow()
        {
            InitializeComponent();
            MenuScreen.Children.Add(NewGame); MenuScreen.Children.Add(Exit);
            NewGame.Click += NewGame_Menu;
            //Exit.Click += ;
        }
        private void Start_Menu(object sender, RoutedEventArgs e)
        {
            MenuScreen.Children.Clear();
            MenuScreen.Children.Add(NewGame); MenuScreen.Children.Add(Options); MenuScreen.Children.Add(Exit);
            NewGame.Click += NewGame_Menu;
            Options.Click += Options_Menu;
        }
        private void NewGame_Menu(object sender, RoutedEventArgs e)
        {
            Window1 a = new Window1();
            this.Close();
            a.Show();
        }
        private void Options_Menu(object sender, RoutedEventArgs e)
        {
            MenuScreen.Children.Clear();
            MenuScreen.Children.Add(Graphics); MenuScreen.Children.Add(Contrlos); MenuScreen.Children.Add(Back);
            Back.Click += Start_Menu;
        }
        private void Start_Game(object sender, RoutedEventArgs e)
        {
            Window1 a = new Window1();
            this.Close();
            a.Show();
        }
    }
}
