using S.I.A.C.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using S.I.A.C.Models.DomainModels;

namespace S.I.A.C.Service.Implement
{
    public class SearchQueriesService : ISearchQueries
    {
        private dbSIACEntities _database;

        public List<TicketPrintableModel> SearchTicket(string stringToSearch)
        {
            if (stringToSearch.IsNullOrWhiteSpace())
            {
                return null;
            }

            _database = new dbSIACEntities();
            var toSearch = stringToSearch.Trim();

            List<TicketPrintableModel> foundTickets = new List<TicketPrintableModel>();

            using (_database)
            {
                foundTickets = (from tick in _database.ticket
                    join creator in _database.people on tick.idCreatorPeople equals creator.id
                    join clientAddress in _database.people on tick.idClient equals clientAddress.id
                    join tecnician in _database.people on tick.idAssignedTechnician equals tecnician.id
                    where (creator.name.Equals(toSearch, StringComparison.InvariantCultureIgnoreCase) ||
                           clientAddress.address.Equals(toSearch, StringComparison.InvariantCultureIgnoreCase) ||
                           tecnician.name.Equals(toSearch, StringComparison.InvariantCultureIgnoreCase) ||
                           tecnician.lastname.Equals(toSearch, StringComparison.InvariantCultureIgnoreCase) ||
                           creator.lastname.Equals(toSearch, StringComparison.InvariantCultureIgnoreCase))
                    select new TicketPrintableModel
                    {
                        idTicket = tick.id,
                        address = clientAddress.address,
                        assignedTechnician = tecnician.name,
                        assignedTechnicianLastname = tecnician.lastname,
                        client = clientAddress.name,
                        clientLastname = clientAddress.lastname,
                        CreationDateTime = tick.creationDate,
                        creatorPeople = creator.name,
                        creatorPeopleLastname = creator.lastname,
                        description = tick.description,
                        email = clientAddress.email
                    }).ToList();
                return foundTickets;
            }
        }
    }
}