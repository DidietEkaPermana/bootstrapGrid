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
		public async Task<JsonResult> AddUpdateClient(Client dataClient)
        {
			JsonData data = new JsonData();

            try
            {
                if (dataClient.ClientId == 0)
                    return await CreateClient(dataClient);
                else
                    return await UpdateClient(dataClient);
            }
            catch (Exception ex)
            {
                data.errors = ex.Message;
            }

			return Json(data);
        }

        private async Task<JsonResult> CreateClient(Client dataClient)
        {
			JsonData data = new JsonData();

            try
            {
                db.Clients.Add(dataClient);

                await db.SaveChangesAsync();

                data.payload = dataClient;

				data.total = 1;
            }
            catch (Exception ex)
            {
                data.errors = ex.Message;
            }

			return Json(data);
        }

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

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ReadClient(int iPage, int iLength, string strSearch)
        {
            JsonData data = new JsonData();

            try
            {
                int iPageStart = (int)((iPage - 1) * iLength);

                data.payload = (await db.Clients.OrderBy(p => p.ClientId).Skip(iPageStart).Take((int)iLength).ToListAsync())
                    .Select(p => new Client
                    {
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
                data.total = (await db.Clients.CountAsync());
            }
            catch (Exception ex)
            {
                data.errors = ex.Message;
            }

            return Json(data);
        }

        private async Task<JsonResult> UpdateClient(Client dataClient)
        {
            JsonData data = new JsonData();

            try
            {
                db.Entry(dataClient).State = EntityState.Modified;
                await db.SaveChangesAsync();

                data.payload = dataClient;

				data.total = 1;
            }
            catch (Exception ex)
            {
                data.errors = ex.Message;
            }

            return Json(data);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteClient(int id)
        {
            JsonData data = new JsonData();

            try
            {
                Client dataClient = await db.Clients.FindAsync(id);
                db.Clients.Remove(dataClient);
                await db.SaveChangesAsync();

                data.payload = dataClient;

				data.total = 1;
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