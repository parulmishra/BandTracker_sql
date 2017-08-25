using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
	public class Band
	{
		private int _id;
		private string _name;
		private int _numberOfPlayers;
		public Band(string name, int numberOfPlayers, int id = 0)
		{
			_id = id;
			_name = name;
			_numberOfPlayers = numberOfPlayers;
		}
		public int GetId()
		{
			return _id;
		}
		public string GetName()
		{
			return _name;
		}
		public int GetNumberOfPlayers()
		{
			return _numberOfPlayers;
		}
		public override bool Equals(Object otherBand)
		{
			if(!(otherBand is Band))
			{
				return false;
			}
			else
			{
				Band newBand = (Band) otherBand;
				bool idEquality = newBand.GetId() == this._id;
				bool nameEquality = newBand.GetName() == this._name;
				bool numberOfPlayersEquality = newBand.GetNumberOfPlayers() == this._numberOfPlayers;
				return (idEquality && nameEquality && numberOfPlayersEquality);
			}
		}
		public override int GetHashCode()
		{
			return this.GetName().GetHashCode();
		}
		public void Save()
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();
			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"INSERT INTO bands (name,numberofplayers) VALUES (@name,@numberofplayers);";

			MySqlParameter nameParameter = new MySqlParameter();
			nameParameter.ParameterName = "@name";
			nameParameter.Value = _name;
			cmd.Parameters.Add(nameParameter);
			
			MySqlParameter numberOfPlayersParameter = new MySqlParameter();
			numberOfPlayersParameter.ParameterName = "@numberofplayers";
			numberOfPlayersParameter.Value = _numberOfPlayers;
			cmd.Parameters.Add(numberOfPlayersParameter);

			cmd.ExecuteNonQuery();
			_id = (int) cmd.LastInsertedId;
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
		}
		public static List<Band> GetAll()
		{
			List<Band> allBands = new List<Band>{};
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"SELECT * FROM bands;";

			var rdr = cmd.ExecuteReader();
			while(rdr.Read())
			{
				int id = rdr.GetInt32(0);
				string name = rdr.GetString(1);
				int numberOfPlayers = rdr.GetInt32(2);
				Band newBand = new Band(name,numberOfPlayers,id);
				allBands.Add(newBand);
			}
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
			return allBands;
		}
		public static void DeleteAll()
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"DELETE FROM bands;";

			cmd.ExecuteNonQuery();
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
		}
		public void Delete()
		{
			MySqlConnection conn =DB.Connection();
			conn.Open();

			MySqlCommand cmd = new MySqlCommand(@"DELETE FROM bands WHERE id=@thisId; DELETE FROM performances WHERE band_id =@thisId;",conn);

			MySqlParameter idParameter = new MySqlParameter();
			idParameter.ParameterName = "@thisId";
			idParameter.Value = _id;
			cmd.Parameters.Add(idParameter);

			cmd.ExecuteNonQuery();
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
		}
		public static Band Find(int id)
		{
			MySqlConnection conn =DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"SELECT * FROM bands WHERE id=@thisId";

			MySqlParameter idParameter = new MySqlParameter();
			idParameter.ParameterName = "@thisId";
			idParameter.Value = id;
			cmd.Parameters.Add(idParameter);

			var rdr = cmd.ExecuteReader() as MySqlDataReader;

			int bandId = 0;
			string name = "";
			int players = 0;

			while(rdr.Read())
			{
				bandId = rdr.GetInt32(0);
				name = rdr.GetString(1);
				players = rdr.GetInt32(2);
			}
			var band = new Band(name,players,bandId);
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
			return band;
		}
		public void Update(string newName)
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"UPDATE bands SET name=@newName WHERE id=@thisId;";

			MySqlParameter nameParameter = new MySqlParameter();
			nameParameter.ParameterName = "@newName";
            nameParameter.Value = newName;
            cmd.Parameters.Add(nameParameter);

			MySqlParameter idParameter = new MySqlParameter();
			idParameter.ParameterName = "@thisId";
			idParameter.Value = _id;
			cmd.Parameters.Add(idParameter);

			cmd.ExecuteNonQuery();
			conn.Close();
			if(conn != null)
			{
				conn.Dispose();
			}
		}
		public void AddVenue(Venue newVenue)
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"INSERT INTO performances(band_id, venue_id) VALUES (@bandId, @venueId);";

			MySqlParameter venueIdParameter = new MySqlParameter();
			venueIdParameter.ParameterName = "@venueId";
			venueIdParameter.Value = newVenue.GetId();
			cmd.Parameters.Add(venueIdParameter);

			MySqlParameter bandIdParameter = new MySqlParameter();
			bandIdParameter.ParameterName = "@bandId";
			bandIdParameter.Value = this._id;
			cmd.Parameters.Add(bandIdParameter);

			cmd.ExecuteNonQuery();
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
		}
		public List<Venue> GetVenues()
		{
			List<Venue> venues = new List<Venue>{};
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"SELECT venues.* FROM bands JOIN performances ON(bands.id = performances.band_id) JOIN venues ON(performances.venue_id = venues.id) WHERE bands.id=@bandId;";

			MySqlParameter bandIdParameter= new MySqlParameter();
			bandIdParameter.ParameterName = "@bandId";
			bandIdParameter.Value = this._id;
			cmd.Parameters.Add(bandIdParameter);

			var rdr = cmd.ExecuteReader() as MySqlDataReader;
			while(rdr.Read())
			{
				int venueId = rdr.GetInt32(0);
				string venueName = rdr.GetString(1);
				string venueAddress = rdr.GetString(2);
				Venue venue = new Venue(venueName,venueAddress,venueId);
				venues.Add(venue);
			}
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
			return venues;
		}
		public void AddToPerformance(Venue newVenue)
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"INSERT INTO performances(band_id, venue_id, date) VALUES (@bandId, @venueId,@date);";

			MySqlParameter bandIdParameter = new MySqlParameter();
			bandIdParameter.ParameterName = "@bandId";
			bandIdParameter.Value = this._id;
			cmd.Parameters.Add(bandIdParameter);

			MySqlParameter venueIdParameter = new MySqlParameter();
			venueIdParameter.ParameterName = "@venueId";
			venueIdParameter.Value = newVenue.GetId();
			cmd.Parameters.Add(venueIdParameter);

			MySqlParameter dateParameter = new MySqlParameter();
			dateParameter.ParameterName = "@date";
			dateParameter.Value = DateTime.Now;
			cmd.Parameters.Add(dateParameter);

			cmd.ExecuteNonQuery();
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
		}
	}
}

		