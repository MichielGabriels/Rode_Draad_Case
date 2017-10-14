using Media.Controller.Ex01;
using Media.DataModel;
using Media.Player;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Media.WPF.Ex01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicController _musicController;
        private MovieController _movieController;

        public MainWindow()
        {
            InitializeComponent();

            _musicController = new MusicController(new AudioPlayer(), new AudioPlaylist());
            _movieController = new MovieController();

            _musicController.Player.IsStarted += SetMusicPlayState;
            _musicController.Player.IsFinished += SetMusicPlayState;

            musicListBox.ItemsSource = _musicController.List;
            movieListBox.ItemsSource = _movieController.List;
        }

        private void SetMusicPlayState()
        {
            if (_musicController.IsPlaying)
            {
                buttonPlay.IsEnabled = false;
                buttonPause.IsEnabled = true;
                buttonNext.IsEnabled = _musicController.HasSongsInPlaylist;
                buttonStop.IsEnabled = true;
                sliderVolume.IsEnabled = true;
            }
            else
            {
                buttonPlay.IsEnabled = _musicController.HasSongsInPlaylist;
                buttonPause.IsEnabled = false;
                buttonNext.IsEnabled = _musicController.HasSongsInPlaylist;
                buttonStop.IsEnabled = false;
                sliderVolume.IsEnabled = false;
            }
        }

        private void OnCloseEvent(object sender, RoutedEventArgs e)
        {
            _musicController.Dispose();
            _movieController.Dispose();

            this.Close();
        }

        private void Cleanup(object sender, EventArgs e)
        {
            _musicController.Dispose();
            _movieController.Dispose();
        }
    }
}
