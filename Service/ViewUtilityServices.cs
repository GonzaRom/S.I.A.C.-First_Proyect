using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S.I.A.C.Models;

namespace S.I.A.C.Service
{
    public class ViewUtilityServices
    {
        private  dbSIACEntities database;

        public List<SelectListItem> GetListOfCategories()
        {
            database = new dbSIACEntities();
            List<CategoriesViewModel> listOfCategories = null;
            
                listOfCategories = (from cat in database.category
                        select new CategoriesViewModel()
                        {
                            keyCategories = cat.id,
                            nameCategories = cat.name
                        }
                    ).ToList();

                List<SelectListItem> categoriesList = listOfCategories.ConvertAll(data => new SelectListItem
            {
                Text = data.nameCategories,
                Value = data.keyCategories.ToString(),
                Selected = false
            });
            database.Dispose();
            return categoriesList;
        }

        public List<SelectListItem> GetListOfPriorities()
        {
            database = new dbSIACEntities();
            List<PriorityViewModel> listOfPriorities = null;
            using (database)
            {
                listOfPriorities = (from priority in database.priority
                        select new PriorityViewModel
                        {
                            keyPriority = priority.id,
                            valuePriority = priority.name
                        }
                    ).ToList();
            }

            List<SelectListItem> prioritiesList = listOfPriorities.ConvertAll(data => new SelectListItem
            {
                Text = data.valuePriority,
                Value = data.keyPriority.ToString(),
                Selected = false
            });

            database.Dispose();
            return prioritiesList;
        }

        public List<SelectListItem> GetListOfTechnicians()
        {
            database = new dbSIACEntities();
            var technicianIdRol = 2;

            List<TechniciansViewModel> listOfTechnicians = null;
            using (database)
            {
                listOfTechnicians = (from technicians in database.people
                        where technicians.idRol == technicianIdRol
                        select new TechniciansViewModel()
                        {
                            keyTechnician = technicians.id,
                            nameTechnician = technicians.name
                        }
                    ).ToList();
            }

            List<SelectListItem> techniciansList = listOfTechnicians.ConvertAll(data => new SelectListItem
            {
                Text = data.nameTechnician,
                Value = data.keyTechnician.ToString(),
                Selected = false
            });
            database.Dispose();
            return techniciansList;
        }
    }
}