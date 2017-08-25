using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
	public class Performance
	{
		private int _id;
		private int _bandId;
		private int _venueId;
		private DateTime _performanceDate;
		public Performance(int bandId, int venueId, DateTime performanceDate, int id = 0)
		{
			_id = id;
			_bandId  = bandId;
			_venueId = venueId;
			_performanceDate = performanceDate;
		}
		public int GetId()
		{
			return _id;
		}
		public int GetBandId()
		{
			return _bandId;
		}
		public int GetVenueId()
		{
			return _venueId;
		}
		public DateTime GetPerformanceDate()
		{
			return _performanceDate;
		}
		public override bool Equals(Object otherPerformance)
		{
			if(!(otherPerformance is Performance))
			{
				return false;
			}
			else
			{
				Performance newPerformance = (Performance) otherPerformance;
				bool idEquality = newPerformance.GetId() == this._id;
				bool bandIdEquality = newPerformance.GetBandId() == this._bandId;
				bool venueIdEquality = newPerformance.GetVenueId() == this._venueId;
				bool performanceDateEquality  = newPerformance.GetPerformanceDate() == this._performanceDate;
				return (idEquality && bandIdEquality && venueIdEquality && performanceDateEquality);
			}
		}
		public override int GetHashCode()
		{
			return this.GetId().GetHashCode();
		}
		public void Save()
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();
			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"INSERT INTO performances(band_id, venue_id,date) VALUES(@band_id,@venue_id,@date);";

			MySqlParameter bandIdParameter = new MySqlParameter();
			bandIdParameter.ParameterName = "@band_id";
			bandIdParameter.Value = _bandId;
			cmd.Parameters.Add(bandIdParameter);

			MySqlParameter venueIdParameter = new MySqlParameter();
			venueIdParameter.ParameterName = "@venue_id";
			venueIdParameter.Value = _venueId;
			cmd.Parameters.Add(venueIdParameter);

			MySqlParameter performanceDateParameter = new MySqlParameter();
			performanceDateParameter.ParameterName = "@date";
			performanceDateParameter.Value = _performanceDate;
			cmd.Parameters.Add(performanceDateParameter);

			cmd.ExecuteNonQuery();
			_id = (int) cmd.LastInsertedId;
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
		}
		public static List<Performance> GetAll()
		{
			List<Performance> allPerformances = new List<Performance>{};
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"SELECT * FROM performances;";

			var rdr = cmd.ExecuteReader();
			while(rdr.Read())
			{
				int id = rdr.GetInt32(0);
				int bandId = rdr.GetInt32(1);
				int venueId = rdr.GetInt32(2);
				DateTime performanceDate = rdr.GetDateTime(3);
				Performance newPerformance = new Performance(bandId,venueId,performanceDate,id);
				allPerformances.Add(newPerformance);
			}
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
			return allPerformances;
		}
		public static void DeleteAll()
		{
			MySqlConnection conn = DB.Connection();
			conn.Open();

			var cmd = conn.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"DELETE FROM performances;";

			cmd.ExecuteNonQuery();
			conn.Close();
			if (conn != null)
			{
				conn.Dispose();
			}
		}
	}
}