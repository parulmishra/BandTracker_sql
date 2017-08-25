using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using BandTracker.Models;
namespace BandTracker.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
	[HttpGet("/allBands")]
    public ActionResult Bands()
    {
      List<Band> allBands = Band.GetAll();
      return View(allBands);
    }
	[HttpGet("/allVenues")]
    public ActionResult Venues()
    {
      List<Venue> allVenues = Venue.GetAll();
      return View(allVenues);
    }
	[HttpGet("/allPerformances")]
    public ActionResult Performances()
    {
      List<Performance> performances = Performance.GetAll();
      return View(performances);
    }
	[HttpGet("/bandForm")]
    public ActionResult BandForm()
    {
      return View();
    }
	[HttpPost("/bandForm/add")]
    public ActionResult AddBand()
    {
		string name = Request.Form["bandname"];
		int members = int.Parse(Request.Form["bandmember"]);
		Band newBand = new Band(name,members);
		newBand.Save();
		return RedirectToAction("Bands");
    }
	
	[HttpPost("/bands/{id}/addvenue")]
    public ActionResult AddVenueForBand(int id)
    {
		int venueId = int.Parse(Request.Form["venueid"]);
		DateTime date = DateTime.Parse(Request.Form["date"]);
		Performance perf = new Performance(id, venueId, date);
		perf.Save();
		Dictionary<string,object> model = new Dictionary<string,object>();
        Band selectedBand = Band.Find(id);
        model["band"] = selectedBand;
        model["venues"] = Venue.GetAll();
		return View("BandDetails", model);
    }
	
	[HttpPost("/venues/{id}/addband")]
    public ActionResult AddBandForVenue(int id)
    {
		int bandId = int.Parse(Request.Form["bandid"]);
		DateTime date = DateTime.Parse(Request.Form["date"]);
		Performance perf = new Performance(bandId, id, date);
		perf.Save();
		Dictionary<string,object> model = new Dictionary<string,object>();
        Venue selectedVenue = Venue.Find(id);
        model["bands"] = Band.GetAll();
        model["venue"] = selectedVenue;
		return View("VenueDetails", model);
    }
	
	[HttpGet("/bandDetails/{id}")]
    public ActionResult BandDetails(int id)
    {
      Dictionary<string,object> model = new Dictionary<string,object>();
      Band selectedBand = Band.Find(id);
      model["band"] = selectedBand;
      model["venues"] = Venue.GetAll();
      return View(model);
    }
	
	[HttpGet("/band/delete/{id}")]
    public ActionResult DeleteBandDetails(int id)
    {
	  var band = Band.Find(id);
	  band.Delete();
      return RedirectToAction("Bands");
    }
	
	[HttpGet("/venue/delete/{id}")]
    public ActionResult DeleteVenueDetails(int id)
    {
	  var venue = Venue.Find(id);
	  venue.Delete();
      return RedirectToAction("Venues");
    }
	
	[HttpGet("/venueForm")]
    public ActionResult VenueForm()
    {
		return View();
    }
	[HttpPost("/venueForm/add")]
    public ActionResult AddVenue()
    {
		string name = Request.Form["venuename"];
		string address = Request.Form["venueaddress"];
		Venue newVenue = new Venue(name,address);
		newVenue.Save();
		return RedirectToAction("Venues");
    }
	[HttpGet("/venueDetails/{id}")]
    public ActionResult VenueDetails(int id)
    {
      Dictionary<string,object> model = new Dictionary<string,object>();
      Venue selectedVenue = Venue.Find(id);
      model["venue"] = selectedVenue;
      model["bands"] = selectedVenue.GetBands();
      return View(model);
    }
  }
}