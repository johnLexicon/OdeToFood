using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Models;
using OdeToFood.Services;
using OdeToFood.ViewModels.Home;

namespace OdeToFood.Controllers
{
    public class HomeController : Controller
    {

        private readonly IRestaurantData _restaurantData;
        private readonly IGreeter _greeter;

        public HomeController(IRestaurantData restaurantData, IGreeter greeter)
        {
            _restaurantData = restaurantData;
            _greeter = greeter;
        }

        // public IActionResult Index()
        // {
        //     var model = new Restaurant {Id = 1, Name = "Fois Gras" };
        //     return new ObjectResult(model);
        // }

        public IActionResult Index(){
            var model = new HomeIndexViewModel()
            {
                Restaurants = _restaurantData.GetAll(),
                CurrentMessage = _greeter.GetMessageOfTheDay()
            };

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var restaurant = _restaurantData.Get(id);

            if(restaurant is null){
                return RedirectToAction(nameof(this.Index));
            }

            var model = new DetailsViewModel()
            {
                Restaurant = restaurant
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RestaurantEditModel model)
        {
            var newRestaurant = new Restaurant()
            {
                Name = model.Name,
                Cuisine = model.Cuisine
            };

            newRestaurant = _restaurantData.Add(newRestaurant);

            return View("Details", new DetailsViewModel{Restaurant = newRestaurant});
        }
    }
}
