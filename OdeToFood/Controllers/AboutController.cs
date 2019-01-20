using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OdeToFood.Controllers
{
    //Explicitly giving the name of the controller
    //[Route("about")]
    //Example using special tokens
    [Route("company/[controller]/[action]")]
    public class AboutController
    {

        public string Phone()
        {
            return "070-0490691545";
        }

        //Explicitly giving the name of the action
        //[Route("address")]
        public string Address()
        {
            return "Svedjegatan 33 lgh 1101 Borås";
        }
    }
}
