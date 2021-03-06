﻿using Media.DataModel.Exceptions;
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
                                musicFile = (byte[])reader[mediaFilePos];
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

            return musicFile;
        }

        public Media AddMedia(Media newMedia)
        {
            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand("[dbo].[spAddSong]", conn))
                    {
                        try
                        {
                            conn.Open();

                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Title", ((Song)newMedia).Title);
                            cmd.Parameters.AddWithValue("@Singer", ((Song)newMedia).Singer);

                            if (newMedia.File != null)
                            {
                                cmd.Parameters.AddWithValue("@File", ((Song)newMedia).File);
                            }

                            newMedia.Id = (int)cmd.ExecuteNonQuery();
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
                    using (var cmd = new SqlCommand("[dbo].[spUpdateSong]", conn))
                    {
                        try
                        {
                            conn.Open();

                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Title", ((Song)updateMedia).Title);
                            cmd.Parameters.AddWithValue("@Singer", ((Song)updateMedia).Singer);
                            cmd.Parameters.AddWithValue("@Id", ((Song)updateMedia).Id);

                            if (updateMedia.File != null)
                            {
                                cmd.Parameters.AddWithValue("@File", ((Song)updateMedia).File);
                            }

                            updateMedia.Id = (int)cmd.ExecuteNonQuery();
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
                    using (var cmd = new SqlCommand("[dbo].[spDeleteSong]", conn))
                    {
                        try
                        {
                            conn.Open();

                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Id", ((Song)oldMedia).Id);

                            oldMedia.Id = (int)cmd.ExecuteNonQuery();
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
    }
}
