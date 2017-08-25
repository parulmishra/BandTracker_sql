using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
	public class Venue
	{
		private int _id;
		private string _name;
		private string _address;
		public Venue(string name, string address, int id = 0)
		{
			_id = id;
			_name = name;
			_address = address;
		}
		public int GetId()
		{
			return _id;
		}
		public string GetName()
		{
			return _name;
		}
		public string GetAddress()
		{
			return _address;
		}
		public override bool Equals(Object otherVenue)
		{
			if(!(otherVenue is Venue))
			{
				return false;
			}
			else
			{
				Venue newVenue = (Venue) otherVenue;
				bool idEquality = newVenue.GetId() == this._id;
				bool nameEquality = newVenue.GetName() == this._name;
				bool addressEquality = newVenue.GetAddress() == this._address;
				return (idEquality && nameEquality && addressEquality);
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
			cmd.CommandText = @"INSERT INTO venues (name,address) VALUES (@name,@address);";

			MySqlParameter nameParameter = new MySqlParameter();
			nameParameter.ParameterName = "@name";
			nameParameter.Value = _name;
			cmd.Parameters.Add(nameParameter);
			
			MySqlParameter addressParameter = new MySqlParameter();
			addressParameter.ParameterName = "@address";
			addressParameter.Value = _address;
			cmd.Parameters.Add(addressParameter);

			cmd.ExecuteNonQuery();
			_id = (int) cmd.LastInsertedId;
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
		}
		public static List<Venue> GetAll()
		{
			List<Venue> allVenues = new List<Venue>{};
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"SELECT * FROM venues;";

			var rdr = cmd.ExecuteReader();
			while(rdr.Read())
			{
				int id = rdr.GetInt32(0);
				string name = rdr.GetString(1);
				string address = rdr.GetString(2);
				Venue newVenue = new Venue(name,address,id);
				allVenues.Add(newVenue);
			}
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
			return allVenues;
		}
		public static void DeleteAll()
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"DELETE FROM venues;";

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

			MySqlCommand cmd = new MySqlCommand(@"DELETE FROM venues WHERE id=@thisId; DELETE FROM performances WHERE venue_id =@thisId;",conn);

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
		public static Venue Find(int id)
		{
			MySqlConnection conn =DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"SELECT * FROM venues WHERE id=@thisId";

			MySqlParameter idParameter = new MySqlParameter();
			idParameter.ParameterName = "@thisId";
			idParameter.Value = id;
			cmd.Parameters.Add(idParameter);

			var rdr = cmd.ExecuteReader() as MySqlDataReader;

			int venueId = 0;
			string name = "";
			string address = "";

			while(rdr.Read())
			{
				venueId = rdr.GetInt32(0);
				name = rdr.GetString(1);
				address = rdr.GetString(2);
			}
			var venue = new Venue(name,address,venueId);
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
			return venue;
		}
		public void Update(string newName)
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"UPDATE venues SET name=@newName WHERE id=@thisId;";

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
		public List<Band> GetBands()
		{
			List<Band> bands = new List<Band>();
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"SELECT bands.* FROM venues JOIN performances ON(venues.id = performances.venue_id) JOIN bands ON(performances.band_id = bands.id) WHERE venues.id=@venueId;";

			MySqlParameter venueIdParameter= new MySqlParameter();
			venueIdParameter.ParameterName = "@venueId";
			venueIdParameter.Value = this._id;
			cmd.Parameters.Add(venueIdParameter);

			var rdr = cmd.ExecuteReader() as MySqlDataReader;
			while(rdr.Read())
			{
				int bandId = rdr.GetInt32(0);
				string bandName = rdr.GetString(1);
				int members = rdr.GetInt32(2);
				Band band = new Band(bandName,members,bandId);
				bands.Add(band);
			}
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
			return bands;
		}
	}
}

		