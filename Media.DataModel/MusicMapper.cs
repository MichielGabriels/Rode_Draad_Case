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
    }
}
