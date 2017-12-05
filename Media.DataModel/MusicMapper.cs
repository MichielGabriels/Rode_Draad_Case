using Media.DataModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.DataModel
{
    public class MusicMapper : IMapper
    {
        private string connectionstring = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=MediaDB; Integrated Security=True";

        public List<DataModel.Media> GetAllMedia()
        {
            var allMusic = new List<DataModel.Media>();
            var query = "SELECT [Id], [Title], [Singer] " +
                        "FROM [dbo].[Song]";

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = null;

                        try
                        {
                            conn.Open();

                            reader = cmd.ExecuteReader();

                            int mediaIdPos = reader.GetOrdinal("Id");
                            int mediaTitlePos = reader.GetOrdinal("Title");
                            int mediaSingerPos = reader.GetOrdinal("Singer");

                            while (reader.Read())
                            {
                                Song song = new Song();
                                song.Id = (int)reader[mediaIdPos];
                                song.Title = reader[mediaTitlePos].ToString();
                                song.Singer = reader[mediaSingerPos].ToString();

                                allMusic.Add(song);
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

            return allMusic;
        }

        public byte[] GetMediaFile(int id)
        {
            byte[] musicFile = null;
            var query = "SELECT [File] " +
                        "FROM [dbo].[Song]" +
                        "WHERE [Id] = " + id;

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = null;

                        try
                        {
                            conn.Open();
                            
                            reader = cmd.ExecuteReader();
                            reader.Read();

                            int mediaFilePos = reader.GetOrdinal("File");
                            
                            musicFile = (byte[])reader[mediaFilePos];
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

            return musicFile;
        }

        public Media AddMedia(Media newMedia)
        {
            string query;

            if (newMedia.File == null)
            {
                query = "INSERT INTO [dbo].[Song] ([Title], [Singer]) " +
                        "VALUES ('" + newMedia.Title + "', '" + ((Song)newMedia).Singer + "'); " +
                        "SELECT CAST(scope_identity() AS int);";
            }
            else
            {
                query = "INSERT INTO [dbo].[Song] ([Title], [Singer], [File]) " +
                        "VALUES ('" + newMedia.Title + "', '" + ((Song)newMedia).Singer + "', 0x" + BitConverter.ToString(newMedia.File).Replace("-", "") + "); " +
                        "SELECT CAST(scope_identity() AS int);";
            }

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        newMedia.Id = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (SqlException e)
            {
                throw new SaveMediaFailedException(e);
            }

            return newMedia;
        }

        public bool UpdateMusic(Song updateSong)
        {
            int updateCount = 0;

            string updateQuery;

            if (updateSong.File == null)
            {
                updateQuery = "UPDATE [dbo].[Song] SET [Title] = @Title, [Singer] = @Singer " +
                              "WHERE [Id] = @Id";
            }
            else
            {
                updateQuery = "UPDATE [dbo].[Song] SET [Title] = @Title, [Singer] = @Singer, [File] = @File " +
                              "WHERE [Id] = @Id";
            }

            using (var conn = new SqlConnection(connectionstring))
            {
                using (var cmd = new SqlCommand(updateQuery, conn))
                {
                    conn.Open();

                    updateSong.Id = (int)cmd.ExecuteScalar();
                }
            }

            return updateCount > 0;
        }

        public bool UpdateMedia(Media updateMedia)
        {
            int updateCount = 0;

            string updateQuery;

            if (updateMedia.File == null)
            {
                updateQuery = "UPDATE [dbo].[Song] SET [Title] = '" + updateMedia.Title + "', [Singer] = '" + ((Song)updateMedia).Singer + "' " +
                              "WHERE [Id] = " + updateMedia.Id;
            }
            else
            {
                updateQuery = "UPDATE [dbo].[Song] SET [Title] = '" + updateMedia.Title + "', [Singer] = '" + ((Song)updateMedia).Singer + "' " + "', [File] = '" + ("0x" + BitConverter.ToString(updateMedia.File).Replace("-", "")) + "' " +
                              "WHERE [Id] = " + updateMedia.Id;
            }

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(updateQuery, conn))
                    {
                        conn.Open();

                        updateMedia.Id = (int)cmd.ExecuteScalar();
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

            string deleteQuery = "DELETE FROM [dbo].[Song] " +
                                 "WHERE Id = " + oldMedia.Id;

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(deleteQuery, conn))
                    {
                        conn.Open();

                        oldMedia.Id = (int)cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                throw new RemoveMediaFailedException(e);
            }

            return updateCount > 0;
        }
    }
}
