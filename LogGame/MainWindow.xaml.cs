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
using GameDomin;

namespace LogGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Image ime = new Image();
            ime.Source = new BitmapImage(new Uri(@"D:\File DownLoads\information.png"));
            btnInfor.Content = ime;
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            MainGame game = new MainGame();
            this.Hide();
            game.ShowDialog();
            this.Show();
        }
    }
}
