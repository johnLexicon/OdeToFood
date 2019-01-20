using System.ComponentModel.DataAnnotations;
using OdeToFood.Models;

namespace OdeToFood.ViewModels.Home
{
    public class RestaurantEditModel
    {
        [Required, MaxLength(8)]
        public string Name { get; set; }
        public CuisineType Cuisine { get; set; }
    }
}