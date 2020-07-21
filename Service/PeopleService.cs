using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S.I.A.C.Models;

namespace S.I.A.C.Service
{
    public class PeopleService
    {
        private readonly dbSIACEntities database;

        public PeopleService()
        {
            database=new dbSIACEntities();
        }

        /// <summary>
        /// Get a user by email and password in the database.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>object person or null depending if email and password match with a active User</returns>
        public people SearchUser(string email, string password)
        {
            people activePerson = null;
            try
            {
                using (database)
                {
                    var objPeople =
                        database.people.FirstOrDefault(e => e.email == email.Trim() && e.pass == password.Trim());

                    activePerson = objPeople;
                }
            }
            catch (Exception ex)
            {
                return activePerson;
            }
            return activePerson;
        }
    }
}