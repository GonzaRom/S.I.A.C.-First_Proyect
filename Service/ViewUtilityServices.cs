using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using S.I.A.C.Models;

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

        public List<SelectListItem> GetListOfTechnicians()
        {
            _database = new dbSIACEntities();
            var technicianIdRol = 2;

            List<TechniciansViewModel> listOfTechnicians = null;
            using (_database)
            {
                listOfTechnicians = (from technicians in _database.people
                        where technicians.idRol == technicianIdRol
                        select new TechniciansViewModel
                        {
                            keyTechnician = technicians.id,
                            nameTechnician = technicians.name
                        }
                    ).ToList();
            }

            var techniciansList = listOfTechnicians.ConvertAll(data => new SelectListItem
            {
                Text = data.nameTechnician,
                Value = data.keyTechnician.ToString(),
                Selected = false
            });
            _database.Dispose();
            return techniciansList;
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

        public List<SelectListItem> GetListOfClients()
        {
            _database = new dbSIACEntities();
            var clientsIdRol = 3;

            List<ClientsViewModel> listOfClients = null;
            using (_database)
            {
                listOfClients = (from people in _database.people
                        where people.idRol == clientsIdRol
                        select new ClientsViewModel()
                        {
                            keyClient = people.id,
                            nameClient = people.name,
                            addressClient= people.address,
                            lastNameClient = people.lastname
                        }
                    ).ToList();
            }

            var clientList = listOfClients.ConvertAll(data => new SelectListItem
            {
                Text = data.GetClientViewModel(),
                Value = data.keyClient.ToString(),
                Selected = false
            });
            
            return clientList;
        }
    }
}