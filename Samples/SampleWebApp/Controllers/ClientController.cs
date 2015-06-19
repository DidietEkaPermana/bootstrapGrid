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
    public class ClientController : Controller
    {
        private Fabrics db;

        public ClientController()
        {
            db = new Fabrics();
        }

        // GET: Client
        public ActionResult Index()
        {
            return View();
        }

        #region API request
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetClientList()
        {
            JsonData data = new JsonData();

            try
            {
                data.payload = (await db.Clients.Take(100).ToListAsync())
                    .Select(p => new Client { 
                        ClientId = p.ClientId, 
                        FirstName = p.FirstName, 
                        MiddleName = p.MiddleName, 
                        LastName = p.LastName, 
                        Gender = p.Gender, 
                        DateOfBirth = p.DateOfBirth, 
                        CreditRating = p.CreditRating, 
                        XCode = p.XCode, 
                        OccupationId = p.OccupationId,
                        Occupation = new Occupation { OccupationId = p.Occupation.OccupationId, OccupationName = p.Occupation.OccupationName },
                        TelephoneNumber = p.TelephoneNumber, 
                        Street1 = p.Street1, 
                        Street2 = p.Street2, 
                        City = p.City, 
                        ZipCode = p.ZipCode, 
                        Latitude = p.Latitude, 
                        Longitude = p.Longitude, 
                        Notes = p.Notes
                    }).ToList();
                data.total = ((List<Client>)data.payload).Count();
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