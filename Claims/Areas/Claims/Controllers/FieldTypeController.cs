using System.Linq;
using System.Web.Mvc;
using Factories;

// ReSharper disable CheckNamespace
namespace ClaimsPoC.Claims.Controllers
// ReSharper restore CheckNamespace
{
    public class FieldTypeController : Controller
    {
        private readonly IFieldTypeFactory _fieldTypeFactory;

        public FieldTypeController(IFieldTypeFactory fieldTypeFactory)
        {

            _fieldTypeFactory = fieldTypeFactory;
        }

        public ActionResult GetFieldTypes()
        {
            var fieldTypes = _fieldTypeFactory.GetFieldTypes();
            return Json(fieldTypes.Select(role => new { Value = role.FieldTypeID, Title = role.Name }), JsonRequestBehavior.AllowGet);
        }
    }
}