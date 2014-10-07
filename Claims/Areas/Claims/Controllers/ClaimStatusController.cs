using System.Linq;
using System.Web.Mvc;
using Factories;
using ModelsLayer;


// ReSharper disable CheckNamespace
namespace ClaimsPoC.Claims.Controllers
// ReSharper restore CheckNamespace
{
    public class ClaimStatusController : Controller
    {
        private readonly IClaimStatusFactory _claimStatusFactory;

        public ClaimStatusController(IClaimStatusFactory claimStatusFactory)
        {
            _claimStatusFactory = claimStatusFactory;



        }
        //
        // GET: /Claims/ClaimStatus/

        public ActionResult Index()
        {
            return View(_claimStatusFactory.GetClaimStatus());
        }

        //
        // GET: /Claims/ClaimStatus/Details/5

        public ActionResult Details(int id = 0)
        {
            return View(_claimStatusFactory.GetClaimStatu(id));
        }

        //
        // GET: /Claims/ClaimStatus/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Claims/ClaimStatus/Create

        [HttpPost]
        public ActionResult Create(ClaimStatu claimstatu)
        {
            _claimStatusFactory.CreateClaimStatu(claimstatu);
            return View(claimstatu);
        }

        //
        // GET: /Claims/ClaimStatus/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var claimstatu = _claimStatusFactory.GetClaimStatu(id);
            return View(claimstatu);
        }

        //
        // POST: /Claims/ClaimStatus/Edit/5

        [HttpPost]
        public ActionResult Edit(ClaimStatu claimstatu)
        {
            _claimStatusFactory.UpdateClaimStatu(claimstatu);
            return View(claimstatu);
        }

        //
        // GET: /Claims/ClaimStatus/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var claimstatu = _claimStatusFactory.GetClaimStatu(id);
            return View(claimstatu);
        }

        //
        // POST: /Claims/ClaimStatus/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var claimstatu = _claimStatusFactory.GetClaimStatu(id);
            _claimStatusFactory.DeleteClaimStatu(claimstatu);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _claimStatusFactory.Dispose(disposing);
        }

        public ActionResult GetClaimStatus()
        {
            var claimStatu = _claimStatusFactory.GetClaimStatus();
            return Json(claimStatu.Select(role => new { Value = role.ClaimStatusID, Title = role.Name }), JsonRequestBehavior.AllowGet);
        }
    }
}