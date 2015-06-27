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
        public async Task<JsonResult> AddUpdateOccupation(Occupation dataOccupation)
        {
            JsonData data = new JsonData();

            try
            {
                if (dataOccupation.OccupationId == 0)
                    return await CreateOccupation(dataOccupation);
                else
                    return await UpdateOccupation(dataOccupation);
            }
            catch (Exception ex)
            {
                data.errors = ex.Message;
            }

            return Json(data);
        }

        private async Task<JsonResult> CreateOccupation(Occupation dataOccupation)
        {
            JsonData data = new JsonData();

            try
            {
                db.Occupations.Add(dataOccupation);

                await db.SaveChangesAsync();

                data.payload = dataOccupation;

                data.total = 1;
            }
            catch (Exception ex)
            {
                data.errors = ex.Message;
            }

            return Json(data);
        }

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

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ReadOccupation(int iPage, int iLength, string strSearch)
        {
            JsonData data = new JsonData();

            try
            {
                int iPageStart = (int)((iPage - 1) * iLength);

                data.payload = (await db.Occupations.OrderBy(p => p.OccupationId).Skip(iPageStart).Take((int)iLength).ToListAsync())
                    .Select(p => new Occupation
                    {
                        OccupationId = p.OccupationId,
                        OccupationName = p.OccupationName
                    }).ToList();
                data.total = (await db.Occupations.CountAsync());
            }
            catch (Exception ex)
            {
                data.errors = ex.Message;
            }

            return Json(data);
        }

        private async Task<JsonResult> UpdateOccupation(Occupation dataOccupation)
        {
            JsonData data = new JsonData();

            try
            {
                db.Entry(dataOccupation).State = EntityState.Modified;
                await db.SaveChangesAsync();

                data.payload = dataOccupation;

                data.total = 1;
            }
            catch (Exception ex)
            {
                data.errors = ex.Message;
            }

            return Json(data);
        }

        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteOccupation(int id)
        {
            JsonData data = new JsonData();

            try
            {
                Occupation dataOccupation = await db.Occupations.FindAsync(id);
                db.Occupations.Remove(dataOccupation);
                await db.SaveChangesAsync();

                data.payload = dataOccupation;

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