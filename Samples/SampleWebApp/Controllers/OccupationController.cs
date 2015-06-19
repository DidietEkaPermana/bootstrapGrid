using SampleWebApp.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SampleWebApp.Controllers
{
    public class OccupationController : Controller
    {
        private Fabrics db;

        public OccupationController()
        {
            db = new Fabrics();
        }

        // GET: Occupation
        public ActionResult Index()
        {
            return View();
        }

        #region API request
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetOccupationList()
        {
            JsonData data = new JsonData();

            try
            {
                data.payload = (await db.Occupations.ToListAsync())
                    .Select(p => new Occupation {
                        OccupationId = p.OccupationId, 
                        OccupationName = p.OccupationName
                    }).ToList();
                data.total = ((List<Occupation>)data.payload).Count();
            }
            catch (Exception ex)
            {
                data.errors = ex.Message;
            }

            return Json(data);
        }
        #endregion API request

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}