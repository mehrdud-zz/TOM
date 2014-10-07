using System.Linq;
using Factories;
using ModelsLayer; 
using System.Collections.Generic; 
using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace ClaimsPoC.Claims.Controllers
// ReSharper restore CheckNamespace
{
    public class CountriesController : Controller
    {
        private readonly ICountryFactory _countryFactory;

        public CountriesController(ICountryFactory countryFactory)
        {
            _countryFactory = countryFactory;
        }

        public List<Country> GetCountryList()
        {
            return _countryFactory.GetCountries();
        }
        public ActionResult GetCountries()
        {
            var countries = _countryFactory.GetCountries();
            return Json(countries.Select(role => new { Value = role.CountryID, Title = role.Name }), JsonRequestBehavior.AllowGet);
        }
    }
}
