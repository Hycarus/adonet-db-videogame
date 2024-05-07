using System;
using System.Data.SqlClient;

namespace adonet_db_videogame
{
	public class Videogame
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Overview { get; set; }
		public DateTime ReleaseDate { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public Videogame(string name, string overview, DateTime releaseData)
		{
			Name = name;
			Overview = overview;
			ReleaseDate = releaseData;
		}
        public Videogame(int id, string name, string overview, DateTime releaseDate)
        {
            Id = id;
            Name = name;
            Overview = overview;
            ReleaseDate = releaseDate;
        }
        public override string ToString()
        {
            return $"ID: {Id}, Nome: {Name}, Descrizione: {Overview}, Data di rilascio: {ReleaseDate.ToString("dd/MM/yyyy")}";
        }
    }
	public class VideogameManager
	{
		private readonly string connectionDatabase = "Data Source=localhost,1433;Database=videogames_db;User Id=sa;Password=dockerStrongPwd123;";

		public VideogameManager(string connectionDatabase)
		{
			this.connectionDatabase = connectionDatabase;
		}

        public void InsertVideogame(Videogame videogame)
		{
            if (videogame == null)
            {
                throw new ArgumentNullException(nameof(videogame), "Videogame cannot be null");
            }
            using SqlConnection connection = new SqlConnection(connectionDatabase);
			connection.Open();
			string query = "INSERT INTO videogames (name, overview, release_date, created_at, updated_at, software_house_id) VALUES (@name, @overview, @release_date, @created_at, @updated_at, @sh_id)";
			using SqlCommand cmd = new SqlCommand(query, connection);
			Random rng = new Random();
            cmd.Parameters.Add(new SqlParameter("@name", videogame.Name));
            cmd.Parameters.Add(new SqlParameter("@overview", videogame.Overview));
            cmd.Parameters.Add(new SqlParameter("@release_date", videogame.ReleaseDate));
            cmd.Parameters.Add(new SqlParameter("@created_at", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@updated_at", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@sh_id", rng.Next(1, 7)));
            int affectedRows = cmd.ExecuteNonQuery();
			Console.WriteLine($"{affectedRows} righe inserite");
		}

		public Videogame GetVideogameById(int id)
		{
			using SqlConnection connection = new SqlConnection(connectionDatabase);
			try
			{
                connection.Open();
                string query = "SELECT id, name, overview, release_date FROM videogames WHERE id = @id";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
					int gameId = (int)reader.GetInt64(0);
                    string? name = reader.IsDBNull(1) ? null : reader.GetString(1);
                    string? overview = reader.IsDBNull(1) ? null : reader.GetString(2);
                    DateTime releaseDate = (DateTime)(reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3));

                    Videogame videogame = new Videogame(gameId, name, overview, releaseDate);
                    return videogame;
                }
                else
                {
                    return null;
                }
            }
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return null;
			}
			finally
			{
				connection.Close();
			}
			
		}

		public void DeleteVideogame(string name)
		{
            using SqlConnection connection = new SqlConnection(connectionDatabase);
			try
			{
				connection.Open();
				string query = @"DELETE FROM videogames WHERE name=@name;";

				using SqlCommand cmd = new SqlCommand(query, connection);
				cmd.Parameters.Add(new SqlParameter("@name", name));

				int affectedRows = cmd.ExecuteNonQuery();
				Console.WriteLine($"{affectedRows} righe eliminate");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			finally
			{
				connection.Close();
			}
        }

		public List<Videogame> SearchVideogameByName(string name)
		{
			List<Videogame> results = new List<Videogame>();
			using SqlConnection connection = new SqlConnection(connectionDatabase);
			try
			{
                connection.Open();
                string query = "SELECT id, name, overview, release_date FROM videogames WHERE name LIKE @name";
				using SqlCommand cmd = new SqlCommand(query, connection);
				cmd.Parameters.AddWithValue("@name", $"%{name}%");
				using SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					int gameId = (int)reader.GetInt64(0);
                    string? gameName = reader.IsDBNull(1) ? null : reader.GetString(1);
                    string? overview = reader.IsDBNull(1) ? null : reader.GetString(2);
                    DateTime releaseDate = (DateTime)(reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3));

                    Videogame videogame = new Videogame(gameId, gameName, overview, releaseDate);
                    results.Add(videogame);
                }
            }
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			finally
			{
				connection.Close();
			}
			return results;
        }
	}
}

