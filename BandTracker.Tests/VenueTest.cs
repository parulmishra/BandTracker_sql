using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BandTracker.Models;

namespace BandTracker.Tests
{
	[TestClass]
	public class VenueTest: IDisposable
	{
		public VenueTest()
		{
			DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=bandtracker_test;";
		}
		public void Dispose()
		{
			Venue.DeleteAll();
			Band.DeleteAll();
		}
		[TestMethod]
		public void Equals_TrueForSameVenueName_True()
		{
		//Arrange, Act
			Venue firstVenue = new Venue("Vankhede Stadium","Mumbai, India");
			Venue secondVenue = new Venue("Vankhede Stadium","Mumbai, India");
		//Assert
			Assert.AreEqual(firstVenue, secondVenue);
		}
		[TestMethod]
		public void Save_SavesVenueToDatabase_VenueList()
		{
		//Arrange, Act
			Venue testVenue = new Venue("Vankhede Stadium","Mumbai, India");
			testVenue.Save();
			List<Venue> expected = new List<Venue> {testVenue};
			List<Venue> result = Venue.GetAll();
		//Assert
			CollectionAssert.AreEqual(expected, result);
		}
		[TestMethod]
		public void Save_AssignsIdToObject_id()
		{
		//Arrange
			
			Venue testVenue = new Venue("Eden Gardens","Kolkata, India");
			testVenue.Save();
		//Act
			Venue savedVenue = Venue.GetAll()[0];
			int result = savedVenue.GetId();
			int testId = testVenue.GetId();
		//Assert
			Assert.AreEqual(testId, result);
		}
		[TestMethod]
		public void Find_FindsVenueInDatabase_Venue()
		{
		//Arrange
			Venue testVenue = new Venue("HPCA","Chelian, India");
			testVenue.Save();
		//Act
			Venue result = Venue.Find(testVenue.GetId());
		//Assert
			Assert.AreEqual(testVenue, result);
		}
		[TestMethod]
		public void Delete_DeletesVenueFromDatabase_VenueList()
		{
		//Arrange
			Venue testVenue1 = new Venue("MCAS","Pune, India");
			testVenue1.Save();
			Venue testVenue2 = new Venue("JN Stadium", "Delhi, India");
			testVenue2.Save();
		//Act
			testVenue1.Delete();
			List<Venue> resultVenueList = Venue.GetAll();
			List<Venue> testVenueList = new List<Venue> {testVenue2};
		//Assert
			CollectionAssert.AreEqual(testVenueList, resultVenueList);
		}
		[TestMethod]
		public void Update_ReturnsAllVenuesMatchingUpdateTerm_VenueList()
		{
		//Arrange
			Venue testVenue = new Venue("DY Patil Stadium","Mumbai, India");
			testVenue.Save();
			testVenue.Update("Dyandeo Yashwantrao");
			Venue expected = new Venue("Dyandeo Yashwantrao","Mumbai, India", testVenue.GetId());
		//Act
			Venue actual = Venue.GetAll()[0];
		//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}