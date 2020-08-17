using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service
{
    public class ViewUtilityServices
    {
        private dbSIACEntities _database;

        public List<SelectListItem> GetListOfCategories()
        {
            _database = new dbSIACEntities();
            List<CategoriesViewModel> listOfCategories = null;

            listOfCategories = (from cat in _database.category
                    select new CategoriesViewModel
                    {
                        keyCategories = cat.id,
                        nameCategories = cat.name
                    }
                ).ToList();

            var categoriesList = listOfCategories.ConvertAll(data => new SelectListItem
            {
                Text = data.nameCategories,
                Value = data.keyCategories.ToString(),
                Selected = false
            });
            _database.Dispose();
            return categoriesList;
        }

        public List<SelectListItem> GetListOfPriorities()
        {
            _database = new dbSIACEntities();
            List<PriorityViewModel> listOfPriorities = null;
            using (_database)
            {
                listOfPriorities = (from priority in _database.priority
                        select new PriorityViewModel
                        {
                            keyPriority = priority.id,
                            valuePriority = priority.name
                        }
                    ).ToList();
            }

            var prioritiesList = listOfPriorities.ConvertAll(data => new SelectListItem
            {
                Text = data.valuePriority,
                Value = data.keyPriority.ToString(),
                Selected = false
            });

            _database.Dispose();
            return prioritiesList;
        }


        public List<SelectListItem> GetListOfRols()
        {
            _database = new dbSIACEntities();
            List<RolsViewModel> listOfRols = null;
            using (_database)
            {
                listOfRols = (from rol in _database.rol
                        select new RolsViewModel
                        {
                            keyRols = rol.id,
                            valueRols = rol.name
                        }
                    ).ToList();
            }

            var rolList = listOfRols.ConvertAll(data => new SelectListItem
            {
                Text = data.valueRols,
                Value = data.keyRols.ToString(),
                Selected = false
            });

            _database.Dispose();
            return rolList;
        }

        public List<SelectListItem> GetListOfTechnicians()
        {
            var peopleQueriesService = new PeopleQueriesService();
            var listOfTechnicians = peopleQueriesService.GetListTechnicians();

            var techniciansList = listOfTechnicians.ConvertAll(data => new SelectListItem
            {
                Text = data.nameTechnician,
                Value = data.keyTechnician.ToString(),
                Selected = false
            });

            return techniciansList;
        }

        public List<SelectListItem> GetListOfClients()
        {
            var peopleQueriesService = new PeopleQueriesService();
            var listOfClients = peopleQueriesService.GetListClients();

            var clientList = listOfClients.ConvertAll(data => new SelectListItem
            {
                Text = data.GetClientFullViewModel(),
                Value = data.keyClient.ToString(),
                Selected = false
            });

            return clientList;
        }

        public List<SelectListItem> GetListOfStatus()
        {
            _database = new dbSIACEntities();
            List<StatusViewModel> listOfStatus = null;
            using (_database)
            {
                listOfStatus = (from status in _database.status
                        select new StatusViewModel
                        {
                            keyStatus = status.id,
                            valueStatus = status.name
                        }
                    ).ToList();
            }

            var statusList = listOfStatus.ConvertAll(data => new SelectListItem
            {
                Text = data.valueStatus,
                Value = data.keyStatus.ToString(),
                Selected = false
            });

            _database.Dispose();
            return statusList;
        }
    }
}