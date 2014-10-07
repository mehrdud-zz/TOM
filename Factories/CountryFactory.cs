using ModelsLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace Factories
{
    public interface ICountryFactory
    {
        void Initialize();
        Country GetCountry(int countryId);
        List<Country> GetCountries();
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        void Dispose(bool disposing);
    }

    public class CountryFactory : ICountryFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public Country GetCountry(int countryId)
        {
            return _db.Countries.Find(countryId);
        }

        public List<Country> GetCountries()
        {
            return _db.Countries.ToList();
        }

        public bool CreateCountry(Country country)
        {
            _db.Countries.Add(country);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateCountry(Country country)
        {
            _db.Entry(country).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteCountry(Country country)
        {
            _db.Countries.Remove(country);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        }
    }
}