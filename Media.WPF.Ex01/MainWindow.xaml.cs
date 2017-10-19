using Media.Controller.Ex01;
using Media.DataModel;
using Media.Player;
using Media.Utils;
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

        private byte[] _newFile;

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

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = _activeController.FileFilter();
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                var file = LoadConvert.ImportFile(dialog.FileName);
                _newFile = file;
            }

            if (_activeController.GetType() == typeof(MusicController))
            {
                checkBoxMusicFilePresent.IsChecked = _newFile != null;
            }
            else
            {
                checkBoxMovieFilePresent.IsChecked = _newFile != null;
            }
        }

        private void MusicSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxSinger.Text != "" && textBoxSongTitle.Text != "")
            {
                if (checkBoxMusicFilePresent.IsChecked == false)
                {
                    var newSong = new Song()
                    {
                        Singer = textBoxSinger.Text,
                        Title = textBoxSongTitle.Text,
                        File = _newFile
                    };

                    _activeController.AddMedia(newSong);
                }
                else
                {
                    var selectedSong = (Song)_activeController.Selected;

                    if (_newFile == null)
                    {
                        _newFile = selectedSong.File;
                    }

                    _activeController.ClearSelected();
                    textBoxSinger.Text = "";
                    textBoxSongTitle.Text = "";
                }

                musicListBox.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please fill in all the fields");
            }
        }

        private void MovieSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxDirector.Text != "" && textBoxMovieTitle.Text != "")
            {
                if (checkBoxMovieFilePresent.IsChecked == false)
                {
                    var newMovie = new Movie()
                    {
                        Director = textBoxDirector.Text,
                        Title = textBoxMovieTitle.Text,
                        File = _newFile
                    };

                    _activeController.AddMedia(newMovie);
                }
                else
                {
                    var selectedMovie = (Movie)_activeController.Selected;

                    if (_newFile == null)
                    {
                        _newFile = selectedMovie.File;
                    }

                    _activeController.ClearSelected();
                    textBoxDirector.Text = "";
                    textBoxMovieTitle.Text = "";
                }

                movieListBox.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please fill in all the fields");
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
                checkBoxMusicFilePresent.IsChecked = _musicController.Selected.File != null || _newFile != null;
                buttonDeleteSong.IsEnabled = true;
                buttonAddToPlaylist.IsEnabled = _musicController.Selected.File != null;
            }
            else
            {
                buttonAddMusicFile.IsEnabled = true;
                checkBoxMusicFilePresent.IsChecked = false;
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
                checkBoxMovieFilePresent.IsChecked = _movieController.Selected.File != null || _newFile != null;
                buttonDeleteMovie.IsEnabled = true;
                buttonPlayMovie.IsEnabled = _movieController.Selected.File != null;
            }
            else
            {
                buttonAddMovieFile.IsEnabled = true;
                checkBoxMovieFilePresent.IsChecked = false;
                buttonDeleteMovie.IsEnabled = false;
                buttonPlayMovie.IsEnabled = false;

                textBoxDirector.Text = "";
                textBoxSongTitle.Text = "";
                movieListBox.SelectedItem = null;
            }
        }
        #endregion
    }
}
