using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace S.I.A.C.Service
{
    public class PeopleService
    {
        private readonly dbSIACEntities _database;

        public PeopleService()
        {
            _database = new dbSIACEntities();
        }

        /// <summary>
        /// Get a user by email and password in the database.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>object person or null depending if email and password match with a active User</returns>
        public people SearchPeople(string email, string password)
        {
            people activePerson = null;
            try
            {
                using (_database)
                {
                    var objPeople =
                        _database.people.FirstOrDefault(e => e.email == email.Trim() && e.pass == password.Trim());

                    activePerson = objPeople;
                }
                _database.Dispose();
            }
            catch (Exception ex)
            {
                return activePerson;
            }
            return activePerson;
        }

        /// <summary>
        /// Include in the view model only the properties you want to update. After the MVC model linker is finished,
        /// copy the properties of the view model into the entity instance and use the tool as an automapper.
        /// </summary>
        /// <param name="registrationViewModel"></param>
        /// <returns>True all good, False something goes wrong </returns>
        [HandleError]
        public bool CreatePeople(RegistrationViewModel registrationViewModel)
        {
            try
            {
                using (_database)
                {
                    var newPeople = new people();
                    newPeople.creationDate = DateTime.Now;
                    newPeople.address = registrationViewModel.address;
                    newPeople.dni = registrationViewModel.dni;
                    newPeople.name = registrationViewModel.name;
                    newPeople.lastname = registrationViewModel.lastname;
                    newPeople.email = registrationViewModel.email;
                    newPeople.pass = registrationViewModel.pass;
                    newPeople.idRol = registrationViewModel.idRol;
                    _database.people.Add(newPeople);
                    _database.SaveChanges();
                }
                _database.Dispose();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}