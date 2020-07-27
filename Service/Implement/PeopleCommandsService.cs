using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using System;
using System.Linq;

namespace S.I.A.C.Service.Implement
{
    public class PeopleCommandsService : IPeopleCommands
    {
        private dbSIACEntities _database;

        /// <summary>
        /// Include in the view model only the properties you want to update. After the MVC model linker is finished,
        /// copy the properties of the view model into the entity instance and use the tool as an automapper.
        /// </summary>
        /// <param name="registrationViewModel"></param>
        /// <returns>True all good, False something goes wrong </returns>
        public bool CreatePeople(RegistrationViewModel registrationViewModel)
        {
            _database = new dbSIACEntities();
            var peopleCount = _database.people.Count();

            using (_database)
            {
                var newPeople = new people
                {
                    creationDate = DateTime.Now,
                    address = registrationViewModel.address,
                    dni = registrationViewModel.dni,
                    name = registrationViewModel.name,
                    lastname = registrationViewModel.lastname,
                    email = registrationViewModel.email,
                    pass = registrationViewModel.pass,
                    idRol = registrationViewModel.idRol
                };
                _database.people.Add(newPeople);
                _database.SaveChanges();
            }

            _database.Dispose();


            if (peopleCount == _database.people.Count())
            {
                return false;
            }
            else return true;
        }
    }
}