using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BandTracker.Models;

namespace BandTracker.Tests
{
	[TestClass]
	public class BandTest: IDisposable
	{
		public BandTest()
		{
			DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=bandtracker_test;";
		}
		public void Dispose()
		{
			Band.DeleteAll();
		}
		[TestMethod]
		public void Equals_TrueForSameBandName_True()
		{
		//Arrange, Act
			Band firstBand = new Band("Euphoria",5);
			Band secondBand = new Band("Euphoria",5);
		//Assert
			Assert.AreEqual(firstBand, secondBand);
		}
		[TestMethod]
		public void Save_SavesBookToDatabase_BookList()
		{
		//Arrange, Act
			Band testBand = new Band("Euphoria",5);
			testBand.Save();
			List<Band> expected = new List<Band> {testBand};
			List<Band> result = Band.GetAll();
		//Assert
			CollectionAssert.AreEqual(expected, result);
		}
		[TestMethod]
		public void Save_AssignsIdToObject_id()
		{
		//Arrange
			Band testBand = new Band("Band Of Boys",4);
			testBand.Save();
		//Act
			Band savedBand = Band.GetAll()[0];
			int result = savedBand.GetId();
			int testId = testBand.GetId();
		//Assert
			Assert.AreEqual(testId, result);
		}
		[TestMethod]
		public void Find_FindsBandInDatabase_Band()
		{
		//Arrange
			Band testBand = new Band("Spice Girls",4);
			testBand.Save();
		//Act
			Band result = Band.Find(testBand.GetId());
		//Assert
			Assert.AreEqual(testBand, result);
		}
		[TestMethod]
		public void Delete_DeletesBandFromDatabase_BandList()
		{
		//Arrange
			Band testBand1 = new Band("Aryans",4);
			testBand1.Save();
			Band testBand2 = new Band("Silk Route",4);
			testBand2.Save();
		//Act
			testBand1.Delete();
			List<Band> resultBandList = Band.GetAll();
			List<Band> testBandList = new List<Band> {testBand2};
		//Assert
			CollectionAssert.AreEqual(testBandList, resultBandList);
		}
		[TestMethod]
		public void Update_ReturnsAllBandsMatchingUpdateTerm_BandList()
		{
		//Arrange
			Band testBand = new Band("Aryans",4);
			testBand.Save();
			testBand.Update("Aryans Reunited");
			Band expected = new Band("Aryans Reunited",4, testBand.GetId());
		//Act
			Band actual = Band.GetAll()[0];
		//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}