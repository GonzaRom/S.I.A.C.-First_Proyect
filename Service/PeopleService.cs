using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S.I.A.C.Models;

namespace S.I.A.C.Service
{
    public class PeopleService
    {
        private readonly dbSIACEntities _database;

        public PeopleService()
        {
            _database=new dbSIACEntities();
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
        /// <param name="peopleViewModel"></param>
        /// <returns>True all good, False something goes wrong </returns>
        [HandleError]
        public bool CreatePeople(PeopleViewModel peopleViewModel)
        {
            try
            {
                using (_database)
                {
                    var newPeople = new people();
                    newPeople.creationDate=DateTime.Now;
                    newPeople.address = peopleViewModel.address;
                    newPeople.dni = peopleViewModel.dni;
                    newPeople.name = peopleViewModel.name;
                    newPeople.lastname = peopleViewModel.lastname;
                    newPeople.email = peopleViewModel.email;
                    newPeople.pass = peopleViewModel.pass;
                    newPeople.idRol = peopleViewModel.idRol;
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