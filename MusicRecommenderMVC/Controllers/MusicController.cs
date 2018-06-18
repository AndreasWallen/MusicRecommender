using System.Linq;
using MusicRecommenderMVC.Models;
using SpotifyLibrary;
using System.Web.Mvc;

namespace MusicRecommenderMVC.Controllers
{
    public class MusicController : Controller
    {
        // GET: Music
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Recommendation()
        {
            return RedirectToAction("Index", "Music");
        }

        [HttpPost]
        public ActionResult Recommendation(FormData formData)
        {
            var spotifyService = new SpotifyService();
            var artists = spotifyService.SearchArtists(formData.ArtistName);
            var artistsResult = new ArtistsResult(formData.ArtistName, artists.ToList());
            return View(artistsResult);
        }
    }
}