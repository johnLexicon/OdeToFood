
using System.Collections.Generic;
using OdeToFood.Models;
using System.Linq;

namespace  OdeToFood.Services
{

    public class InMemoryRestaurantData : IRestaurantData
    {

        private readonly List<Restaurant> _restaurants;

        public InMemoryRestaurantData()
        {
            _restaurants = new List<Restaurant>{
                new Restaurant { Id = 1, Name = "Il Bambino"},
                new Restaurant { Id = 2, Name = "La Scala" },
                new Restaurant { Id = 3, Name = "El sombrero"}
            };
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            newRestaurant.Id = _restaurants.Max(r => r.Id) + 1;
            _restaurants.Add(newRestaurant);
            return newRestaurant;
        }

        public Restaurant Get(int id)
        {
            return _restaurants.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurants.OrderBy(r => r.Name);
        }
    }

}