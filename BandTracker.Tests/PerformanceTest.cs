using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BandTracker.Models;

namespace BandTracker.Tests
{
	[TestClass]
	public class PerformanceTest: IDisposable
	{
		public PerformanceTest()
		{
			DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=bandtracker_test;";
		}
		public void Dispose()
		{
			Performance.DeleteAll();
			Venue.DeleteAll();
			Band.DeleteAll();
		}
		[TestMethod]
		public void Equals_TrueForSamePerformance_True()
		{
			DateTime newPerformanceDate = default(DateTime);
		//Arrange, Act
			Performance firstPerformance = new Performance(1,1,newPerformanceDate,1);
			Performance secondPerformance = new Performance(1,1,newPerformanceDate,1);
		//Assert
			Assert.AreEqual(firstPerformance, secondPerformance);
		}
		[TestMethod]
		public void Save_SavesPerformanceToDatabase_PerformanceList()
		{
		//Arrange	
			DateTime newPerformanceDate = default(DateTime);
			Performance firstPerformance = new Performance(1,1,newPerformanceDate,1);
			firstPerformance.Save();
		//Act
			List<Performance> expected = new List<Performance> {firstPerformance};
			List<Performance> result = Performance.GetAll();
		//Assert
			CollectionAssert.AreEqual(expected, result);	
		}
	}
}
		