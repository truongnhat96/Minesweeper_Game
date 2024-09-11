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
using System.Windows.Shapes;

namespace GameDomin
{
    /// <summary>
    /// Interaction logic for LogGame.xaml
    /// </summary>
    public partial class LogGame : Window
    {
        public LogGame()
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

        private void btnInfor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Luật chơi rất đơn giản, bạn chỉ cần mở các ô và tránh mìn\nDùng lá cờ để đánh dấu những điểm bạn cho là có mìn \n(Nhấp chuột phải)", "Hướng dẫn chơi", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
