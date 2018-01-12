using Media.DataModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Media.DataModel
{
    public class MovieMapper : IMapper
    {
        private string connectionstring = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=MediaDB; Integrated Security=True";

        public List<DataModel.Media> GetAllMedia()
        {
            var allMovies = new List<DataModel.Media>();
            var query = "SELECT [Id], [Title], [Director] " +
                        "FROM [dbo].[Movie]";

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(null, conn))
                    {
                        SqlDataReader reader = null;

                        try
                        {
                            conn.Open();

                            cmd.CommandText = query;
                            cmd.Prepare();

                            reader = cmd.ExecuteReader();

                            int mediaIdPos = reader.GetOrdinal("Id");
                            int mediaTitlePos = reader.GetOrdinal("Title");
                            int mediaDirectorPos = reader.GetOrdinal("Director");

                            while (reader.Read())
                            {
                                Movie movie = new Movie();
                                movie.Id = (int)reader[mediaIdPos];
                                movie.Title = reader[mediaTitlePos].ToString();
                                movie.Director = reader[mediaDirectorPos].ToString();

                                allMovies.Add(movie);
                            }
                        }
                        finally
                        {
                            conn?.Close();
                            reader?.Close();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new MediaReadFailedException(e);
            }

            return allMovies;
        }

        public byte[] GetMediaFile(int id)
        {
            byte[] movieFile = null;
            var query = "SELECT [File] " +
                        "FROM [dbo].[Movie]" +
                        "WHERE [Id] = " + id;

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(null, conn))
                    {
                        SqlDataReader reader = null;

                        try
                        {
                            conn.Open();

                            cmd.CommandText = query;
                            cmd.Prepare();

                            reader = cmd.ExecuteReader();
                            reader.Read();

                            int mediaFilePos = reader.GetOrdinal("File");

                            if (!reader.IsDBNull(mediaFilePos))
                            {
                                movieFile = (byte[])reader[mediaFilePos];
                            }
                        }
                        finally
                        {
                            conn?.Close();
                            reader?.Close();
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw new LoadMediaFileException();
            }

            return movieFile;
        }

        public Media AddMedia(Media newMedia)
        {
            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand("[dbo].[spAddMovie]", conn))
                    {
                        try
                        {
                            conn.Open();

                            SqlTransaction transaction = conn.BeginTransaction();
                            String savepoint = "AddMediaSP";

                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Title", ((Movie)newMedia).Title);
                            cmd.Parameters.AddWithValue("@Director", ((Movie)newMedia).Director);

                            if (newMedia.File != null)
                            {
                                cmd.Parameters.AddWithValue("@File", ((Movie)newMedia).File);
                            }

                            cmd.Transaction = transaction;
                            transaction.Save(savepoint);

                            newMedia.Id = (int)cmd.ExecuteNonQuery();

                            if (this.GetConfirmation(newMedia, "toevoegen") == MessageBoxResult.No)
                            {
                                transaction.Rollback(savepoint);
                            }

                            transaction.Commit();
                        }
                        finally
                        {
                            conn?.Close();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new SaveMediaFailedException(e);
            }

            return newMedia;
        }

        public bool UpdateMedia(Media updateMedia)
        {
            int updateCount = 0;

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand("[dbo].[spUpdateMovie]", conn))
                    {
                        try
                        {
                            conn.Open();

                            SqlTransaction transaction = conn.BeginTransaction();
                            String savepoint = "UpdateMediaSP";

                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Id", ((Movie)updateMedia).Id);
                            cmd.Parameters.AddWithValue("@Title", ((Movie)updateMedia).Title);
                            cmd.Parameters.AddWithValue("@Director", ((Movie)updateMedia).Director);

                            if (updateMedia.File != null)
                            {
                                cmd.Parameters.AddWithValue("@File", ((Movie)updateMedia).File);
                            }

                            cmd.Transaction = transaction;
                            transaction.Save(savepoint);

                            updateMedia.Id = (int)cmd.ExecuteNonQuery();

                            if (this.GetConfirmation(updateMedia, "wijzigen") == MessageBoxResult.No)
                            {
                                transaction.Rollback(savepoint);
                            }

                            transaction.Commit();
                        }
                        finally
                        {
                            conn?.Close();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new UpdateMediaFailedException(e);
            }

            return updateCount > 0;
        }

        public bool DeleteMedia(Media oldMedia)
        {
            int updateCount = 0;

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand("[dbo].[spDeleteMovie]", conn))
                    {
                        try
                        {
                            conn.Open();

                            SqlTransaction transaction = conn.BeginTransaction();
                            String savepoint = "DeleteMediaSP";

                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Id", ((Movie)oldMedia).Id);

                            cmd.Transaction = transaction;
                            transaction.Save(savepoint);

                            oldMedia.Id = (int)cmd.ExecuteNonQuery();

                            if (this.GetConfirmation(oldMedia, "verwijderen") == MessageBoxResult.No)
                            {
                                transaction.Rollback(savepoint);
                            }

                            transaction.Commit();
                        }
                        finally
                        {
                            conn?.Close();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new RemoveMediaFailedException(e);
            }

            return updateCount > 0;
        }

        private MessageBoxResult GetConfirmation(Media media, String action)
        {
            MessageBoxResult result = MessageBox.Show($"Bent u zeker dat u de movie met titel '{media.Title}' wilt {action}?", $"Movie {action}", MessageBoxButton.YesNo);
            return result;
        }
    }
}
