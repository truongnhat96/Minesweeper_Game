using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameDomin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainGame : Window, INotifyPropertyChanged
    {
        Random rdBom;
        HashSet<int> BoomList = new HashSet<int>();
        int boom, point;
        string imagePath;
        Timer timer;

        int[] dx = new int[] {0, 1, -1, -1, -1, 0, 0, 1, 1, 1};
        int[] dy = new int[] {1, -1, -1, 0, 1, -1, 1, -1, 0, 1};

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void TextChanged(string text)
        {
            tblTime.Text = text;
        }

        public int Boom { get => boom; set { boom = value; OnPropertyChanged("Boom"); } }
        public int Point { get => point; set { point = value; OnPropertyChanged("Point"); } }
        public string ImagePath { get => imagePath; set { imagePath = value; OnPropertyChanged("ImagePath"); } }

        private void Loang(int x, int y)
        {
            Button btn = GetButtons(grBox, x, y);
            if(btn.Background == Brushes.Green) return;
            int cnt = 0;
            for(int k = 2; k < 10; k++)
            {
                int x1 = x + dx[k];
                int y1 = y + dy[k];
                if (x1 >= 0 && x1 < 15 && y1 >= 0 && y1 < 15 && Exploded((int)GetButtons(grBox, x1, y1).Tag)) cnt++;
            }
            btn.Content = cnt.ToString() + "          ";
            btn.Background = Brushes.Green;
            Point += 50;
            for(int k = 0;k < 2; k++)
            {
                int x1 = x + dx[k];
                int y1 = y + dy[k];
                Button btn1 = GetButtons(grBox, x1, y1);
                if(x1 >= 0 && x1 < 15 && y1 >= 0 && y1 < 15 && !Exploded((int)btn1.Tag)) Loang(x1, y1);
            }
        }

        public MainGame()
        {
            InitializeComponent();
            this.DataContext = this;
            rdBom = new Random();

            boom = BoomList.Count;
            for(int i = 0;i < 15; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                grBox.RowDefinitions.Add(row);
            }

            for (int i = 0; i < 15; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                grBox.ColumnDefinitions.Add(col);
            }
            
        }
        
        private Button GetButtons(Grid gr, int x, int y)
        {
            foreach (Button item in gr.Children)
            {
                if (x == Grid.GetRow(item) && y == Grid.GetColumn(item)) return item;
            }
            return null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grBox.Children.Clear();
            BoomList.Clear();
            ImagePath = @"D:\smile.png";
            for (int i = 0; i < 24; i++)
            {
                BoomList.Add(rdBom.Next(100, 225));
                BoomList.Add(rdBom.Next(1, 79));
                BoomList.Add(rdBom.Next(80, 99));
            }
            Boom = BoomList.Count;
            timer = new Timer();
            timer.ActTime = TextChanged;
            timer.Time.Start();
            prgTime.IsIndeterminate = true;
            int dem = 1;
            Point = 0;
            for (int i = 0; i < 15; i++)
            {
                 for(int j = 0;  j < 15; j++)
                 {
                    Button btn = new Button() { FontSize= 24, FontWeight = FontWeights.Bold, Content = "", Cursor = Cursors.Hand, Foreground = Brushes.Pink};
                    btn.Tag = dem;
                    btn.Click += Btn_Click;
                    btn.MouseRightButtonDown += Btn_MouseRightButtonDown;
                    Grid.SetRow(btn,i);
                    Grid.SetColumn(btn,j);
                    grBox.Children.Add(btn);
                    dem++;
                 }
            }
        }

        private void Btn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            Image ime = new Image();
            ime.Source = new BitmapImage(new Uri(@"D:\File DownLoads\point.png"));
            if (btn.Content.ToString() == "")
            {
                Boom--;
                btn.Content = ime;
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            
            if(button.Content.ToString() != "" && button.Background == Brushes.DarkGray)
            {
                button.Content = "";
                Boom++;
                return;
            }

            int i = (int)button.Tag;
            if (Exploded(i))
            {
                Image ime = new Image();
                ime.Source = new BitmapImage(new Uri(@"D:\File DownLoads\bomb.png"));
                button.Content = ime;
                button.Background = Brushes.Red;
                ImagePath = @"D:\sad.png";
                timer.Stop();
                --Boom;
                prgTime.IsIndeterminate = false;
                MessageBox.Show($"Dẫm trúng mìn rồi  ಥ_ಥ\nĐiểm: {Point}", "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                Window_Loaded(this, new RoutedEventArgs());
                return;
            }
            else
            {
                int x = Grid.GetRow(button);
                int y = Grid.GetColumn(button);
                Loang(x, y);
            }

            if (Point == 11250 - (BoomList.Count * 50))
            {
                timer.Stop();
                prgTime.IsIndeterminate = false;
                MessageBox.Show($"Chúc mừng bạn đã chiến thắng (≧▽≦)\nĐiểm: {Point}", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                Window_Loaded(this, new RoutedEventArgs());
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát trò chơi không?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) e.Cancel = false;
            else e.Cancel = true;
        }

        private bool Exploded(int x)
        {
            foreach (int item in BoomList)
            {
                if(x == item) return true;
            }
            return false;
        }
    }
    public class Timer
    {
        DispatcherTimer time;
        TimeSpan totaltime;
        Stopwatch stopwatch;

        Action<string> actTime;

        public Timer()
        {
            Time = new DispatcherTimer();
            Time.Interval = TimeSpan.FromMilliseconds(1);
            totaltime = TimeSpan.FromMinutes(10);
            stopwatch = Stopwatch.StartNew();
            Time.Tick += Time_Tick;
        }

        public void Stop()
        {
            Time.Stop();
            stopwatch.Stop();
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            TimeSpan Remaining = totaltime - stopwatch.Elapsed;
            if (Remaining.TotalMilliseconds > 0)
            {
                ActTime.Invoke(Remaining.ToString(@"mm\:ss\:ff"));
            }
            else
            {
                time.Stop();
                stopwatch.Restart();
                ActTime.Invoke("00:00:00");
                MessageBox.Show("Hết giờ rồi ლ(ಠ益ಠლ)", "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public DispatcherTimer Time { get => time; set => time = value; }
        public Action<string> ActTime { get => actTime; set => actTime = value; }
    }
}
