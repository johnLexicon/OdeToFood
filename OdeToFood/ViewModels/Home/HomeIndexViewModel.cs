using System.Collections.Generic;
using OdeToFood.Models;

namespace OdeToFood.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public string CurrentMessage { get; set; }
    }
}