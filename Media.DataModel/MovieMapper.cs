﻿using Media.DataModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                            movieFile = (byte[])reader[mediaFilePos];
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
            string query;

            if (newMedia.File == null)
            {
                query = "INSERT INTO [dbo].[Movie] ([Title], [Director]) " +
                        "VALUES ('" + newMedia.Title + "', '" + ((Movie)newMedia).Director + "'); " +
                        "SELECT CAST(scope_identity() AS int);";
            }
            else
            {
                query = "INSERT INTO [dbo].[Movie] ([Title], [Director], [File]) " +
                        "VALUES ('" + newMedia.Title + "', '" + ((Movie)newMedia).Director + "', 0x" + BitConverter.ToString(newMedia.File).Replace("-", "") + "); " +
                        "SELECT CAST(scope_identity() AS int);";
            }

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(null, conn))
                    {
                        conn.Open();

                        cmd.CommandText = query;
                        cmd.Prepare();

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

        public bool UpdateMedia(Media updateMedia)
        {
            int updateCount = 0;

            string updateQuery;

            if (updateMedia.File == null)
            {
                updateQuery = "UPDATE [dbo].[Movie] SET [Title] = '" + updateMedia.Title + "', [Director] = '" + ((Movie)updateMedia).Director + "' " +
                              "WHERE [Id] = " + updateMedia.Id;
            }
            else
            {
                updateQuery = "UPDATE [dbo].[Movie] SET [Title] = '" + updateMedia.Title + "', [Director] = '" + ((Movie)updateMedia).Director + "' " + "', [File] = '" + ("0x" + BitConverter.ToString(updateMedia.File).Replace("-", "")) + "' " +
                              "WHERE [Id] = " + updateMedia.Id;
            }

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(null, conn))
                    {
                        conn.Open();

                        cmd.CommandText = updateQuery;
                        cmd.Prepare();

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

            string deleteQuery = "DELETE FROM [dbo].[Movie] " +
                                 "WHERE Id = " + oldMedia.Id;

            try
            {
                using (var conn = new SqlConnection(connectionstring))
                {
                    using (var cmd = new SqlCommand(null, conn))
                    {
                        conn.Open();

                        cmd.CommandText = deleteQuery;
                        cmd.Prepare();

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
