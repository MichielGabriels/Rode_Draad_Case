using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;

namespace Media.WPF.Ex01
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {
        private const string PATH = @"C:\PXLTemp\";
        private const string FILENAME = "movie.avi";

        private bool _fullscreen;
        private DispatcherTimer _doubleClickTimer;
        private DispatcherTimer _videoTimer;

        public VideoPlayer(byte[] videoFile)
        {
            _doubleClickTimer = new DispatcherTimer();
            _videoTimer = new DispatcherTimer();
            _fullscreen = false;

            InitializeComponent();

            if (Directory.Exists(PATH))
            {
                CleanUp(null, null);
            }

            InitializeVideoTimer(50);
            InitializeVideoMediaElement(videoFile);
        }

        private void InitializeVideoTimer(int intervalInMilliseconds)
        {
            _videoTimer.Interval = TimeSpan.FromMilliseconds(intervalInMilliseconds);
            _videoTimer.Tick += VideoTimer_Tick;
        }

        private void InitializeVideoMediaElement(byte[] videoFile)
        {
            Directory.CreateDirectory(PATH);
            using (var fs = new FileStream(PATH + FILENAME, FileMode.CreateNew, FileAccess.Write))
            {
                fs.Write(videoFile, 0, videoFile.Length);
            }
            VideoMediaElement.Source = new Uri(PATH + FILENAME, UriKind.Absolute);
            VideoMediaElement.Volume = VolumeSlider.Value;
            VideoMediaElement.Play();
        }

        private void VideoTimer_Tick(object sender, EventArgs e)
        {
            SeekSlider.Value = VideoMediaElement.Position.TotalSeconds;
        }

        private void VideoMediaElement_MediaEnding(object sender, RoutedEventArgs e)
        {
            VideoMediaElement.Pause();
            VideoMediaElement.Position = TimeSpan.Zero;
        }

        private void VideoMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = VideoMediaElement.NaturalDuration.TimeSpan;
            SeekSlider.Maximum = ts.TotalSeconds;
            _videoTimer.Start();
        }

        private void CleanUp(object sender, EventArgs e)
        {
            try
            {
                VideoMediaElement.Close();
                var files = Directory.GetFiles(PATH);
                foreach (var f in files)
                {
                    File.Delete(f);
                }
                Directory.Delete(PATH);
            }
            catch (IOException)
            {
                CleanUp(sender, e);
            }
            catch (UnauthorizedAccessException)
            {
                CleanUp(sender, e);
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            VideoMediaElement.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            VideoMediaElement.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            VideoMediaElement.Stop();
        }

        private void VideoMediaElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //    if (!_doubleClickTimer.IsEnabled)
            //    {
            //        _doubleClickTimer.Start();
            //    }
            //    else
            //    {
            //        if (!_fullscreen)
            //        {
            //            this.WindowStyle = WindowStyle.None;
            //            this.WindowState = WindowState.Maximized;
            //        }
            //        else
            //        {
            //            this.WindowStyle = WindowStyle.SingleBorderWindow;
            //            this.WindowState = WindowState.Normal;
            //        }
            //        _fullscreen = !_fullscreen;
            //    }
        }

        private void SeekSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoMediaElement.Position = TimeSpan.FromSeconds(SeekSlider.Value);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoMediaElement.Volume = VolumeSlider.Value;
        }

        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_fullscreen)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
            }
            _fullscreen = !_fullscreen;
        }

        [DllImport("user32.dll")]
        private static extern uint GetDoubleClickTime();
    }
}
