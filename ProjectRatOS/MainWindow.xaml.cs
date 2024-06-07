using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace ProjectRatOS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string curDir = Directory.GetCurrentDirectory();
        static DirectoryInfo dirExist = new DirectoryInfo($"{curDir}\\MusicFolder");
        MediaPlayer playMusic = new MediaPlayer();
        MediaPlayer closePlay = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            UpdateMusic();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (playMusic.Source != null)
                lblStatus.Content = String.Format("{0} / {1}", playMusic.Position.ToString(@"mm\:ss"), playMusic.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            else
                lblStatus.Content = "Файл не выбран...";
        }

        private void DragMe(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
                //exception
            }
        }
        private void LoadMusic_Click(object sender, RoutedEventArgs e)
        {
            UpdateMusic();
            Process.Start($"{curDir}\\MusicFolder");
            //Process.Start(Directory.GetParent(Directory.GetParent(curDir).ToString()).ToString());
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(3000);
            
            this.Close();
        }

        private void UpdateMusic()
        {
            //для проверки существования $"{curDir}\\MusicFolder"(путь)
            if (Directory.Exists($"{curDir}\\MusicFolder"))
            {
                MusicCount.Content = $"Треков в папке: {dirExist.GetFiles().Count()}";
                chooseMusic.ItemsSource = dirExist.GetFiles();
            }
            else 
            {
                Directory.CreateDirectory($"{curDir}\\MusicFolder");
            }
        }

        private void chooseMusic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (chooseMusic.SelectedIndex >= 0)
                {
                    var item = chooseMusic.SelectedItem.ToString();
                    if (item != null)
                    {
                        string musicPath = $"{dirExist}\\{item}";
                        playMusic.Open(new Uri(musicPath));
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            playMusic.Play();
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            playMusic.Pause();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            playMusic.Stop();
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            playMusic.Volume = volumeSlider.Value;
        }
    }
}
