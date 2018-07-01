using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicRecommenderMVC.Models
{
    public class FormData
    {
        public string ArtistName { get; set; }
        public List<string> Genres { get; set; }
    }
}