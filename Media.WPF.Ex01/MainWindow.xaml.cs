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
        private MediaController _activeController;

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

        #region Events
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

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (musicTabItem.IsSelected)
            {
                _activeController = _musicController;
                this.LoadMusicData();
            }
            else if (moviesTabItem.IsSelected)
            {
                _activeController = _movieController;
                this.LoadMovieData();
            }

            this.ClearSelected();
        }

        private void MusicListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var song = (Song)musicListBox.SelectedItem;

            if (song != null)
            {
                this.SelectMusicItem(song);
                this.SetMusicForm();
            }

            e.Handled = true;
        }

        private void MovieListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var movie = (Movie)movieListBox.SelectedItem;

            if (movie != null)
            {
                this.SelectMovieItem(movie);
                this.SetMovieForm();
            }
        }
        #endregion

        #region Other methods
        private void LoadMusicData()
        {
            musicListBox.Items.Refresh();
        }

        private void LoadMovieData()
        {
            movieListBox.Items.Refresh();
        }

        private void ClearSelected()
        {
            _activeController.ClearSelected();
            this.SetMusicPlayState();
        }

        private void SelectMusicItem(Song song)
        {
            textBoxSinger.Text = song.Singer;
            textBoxSongTitle.Text = song.Title;

            _musicController.ChangeSelected(song);
        }

        private void SetMusicForm()
        {
            if (_musicController.Selected != null)
            {
                buttonAddMusicFile.IsEnabled = true;
                checkBoxMusicFilePresent.IsEnabled = _musicController.Selected.File != null;
                buttonDeleteSong.IsEnabled = true;
                buttonAddToPlaylist.IsEnabled = _musicController.Selected.File != null;
            }
            else
            {
                buttonAddMusicFile.IsEnabled = true;
                checkBoxMusicFilePresent.IsEnabled = false;
                buttonDeleteSong.IsEnabled = false;
                buttonAddToPlaylist.IsEnabled = false;

                textBoxSinger.Text = "";
                textBoxSongTitle.Text = "";
                musicListBox.SelectedItem = null;
            }
        }

        private void SelectMovieItem(Movie movie)
        {
            textBoxDirector.Text = movie.Director;
            textBoxMovieTitle.Text = movie.Title;

            _movieController.ChangeSelected(movie);
        }

        private void SetMovieForm()
        {
            if (_movieController.Selected != null)
            {
                buttonAddMovieFile.IsEnabled = true;
                checkBoxMovieFilePresent.IsEnabled = _movieController.Selected.File != null;
                buttonDeleteMovie.IsEnabled = true;
            }
            else
            {
                buttonAddMovieFile.IsEnabled = true;
                checkBoxMovieFilePresent.IsEnabled = false;
                buttonDeleteMovie.IsEnabled = false;

                textBoxDirector.Text = "";
                textBoxSongTitle.Text = "";
                movieListBox.SelectedItem = null;
            }
        }
        #endregion
    }
}
