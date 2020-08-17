using System.Collections.Generic;
using System.Linq;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;

namespace S.I.A.C.Service
{
    public class PeopleQueriesService : IPeopleQueries
    {
        private dbSIACEntities _database;

        /// <summary>
        ///     Get a user by email and password in the database.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>object person or null depending if email and password match with a active User</returns>
        public people SearchPeople(string email, string password)
        {
            _database = new dbSIACEntities();
            people activePerson = null;
            var truePassword = Encrypt.GetSHA256(password.Trim());
            using (_database)
            {
                var objPeople =
                    _database.people.FirstOrDefault(e => e.email == email.Trim() && e.pass == truePassword);

                activePerson = objPeople;
            }

            return activePerson;
        }

        public PeopleViewModel GetDetailsPeople(int peopleId)
        {
            _database = new dbSIACEntities();
            PeopleViewModel searchPeople = null;
            using (_database)
            {
                var currentPeople = _database.people.FirstOrDefault(current => current.id == peopleId);
                searchPeople = new PeopleViewModel
                {
                    addressClient = currentPeople.address,
                    keyClient = currentPeople.id,
                    lastNameClient = currentPeople.lastname,
                    nameClient = currentPeople.name
                };
            }


            return searchPeople;
        }

        public List<PeopleViewModel> GetListClients()
        {
            _database = new dbSIACEntities();
            var clientsIdRol = 3;

            List<PeopleViewModel> listOfClients = null;
            using (_database)
            {
                listOfClients = (from people in _database.people
                        where people.idRol == clientsIdRol
                        select new PeopleViewModel
                        {
                            keyClient = people.id,
                            nameClient = people.name,
                            addressClient = people.address,
                            lastNameClient = people.lastname
                        }
                    ).ToList();
            }

            _database.Dispose();
            return listOfClients;
        }

        public List<TechniciansViewModel> GetListTechnicians()
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

            _database.Dispose();
            return listOfTechnicians;
        }

        public List<AdminsViewModel> GetListAdmins()
        {
            _database = new dbSIACEntities();
            var AdminIdRol = 1;

            List<AdminsViewModel> listOfAdmins = null;
            using (_database)
            {
                listOfAdmins = (from admins in _database.people
                        where admins.idRol == AdminIdRol
                        select new AdminsViewModel
                        {
                            keyAdmin = admins.id,
                            emailAdmin = admins.email
                        }
                    ).ToList();
            }

            _database.Dispose();
            return listOfAdmins;
        }
    }
}