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
	[HttpGet("/bandDetails/{id}")]
    public ActionResult BandDetails(int id)
    {
      Dictionary<string,object> model = new Dictionary<string,object>();
      Band selectedBand = Band.Find(id);
      model["band"] = selectedBand;
      model["venues"] = selectedBand.GetVenues();
      return View(model);
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