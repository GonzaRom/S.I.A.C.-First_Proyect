using Microsoft.Ajax.Utilities;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using System.Collections.Generic;
using System.Linq;

namespace S.I.A.C.Service.Implement
{
    public class SearchQueriesService : ISearchQueries
    {
        private dbSIACEntities _database;

        public List<TicketPrintableModel> SearchTicket(string stringToSearch)
        {
            _database = new dbSIACEntities();
            using (_database)
            {
                IQueryable<TicketPrintableModel> foundTickets = (from tick in _database.ticket
                    join creator in _database.people on tick.idCreatorPeople equals creator.id
                    join clientAddress in _database.people on tick.idClient equals clientAddress.id
                    join tecnician in _database.people on tick.idAssignedTechnician equals tecnician.id
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
                    });
                if (!stringToSearch.IsNullOrWhiteSpace())
                {
                    var toSearch = stringToSearch.Trim().ToLower();
                    if (toSearch != null)
                    {
                        foundTickets = foundTickets.Where(currentTicket =>
                            (currentTicket.assignedTechnician.ToLower().Contains(toSearch)) ||
                            (currentTicket.assignedTechnicianLastname.ToLower().Contains(toSearch)) ||
                            (currentTicket.client.ToLower().Contains(toSearch)) ||
                            (currentTicket.clientLastname.ToLower().Contains(toSearch)) ||
                            (currentTicket.address.ToLower().Contains(toSearch)) ||
                            (currentTicket.description.ToLower().Contains(toSearch)));
                    }
                }

                return foundTickets.ToList();
            }
        }
    }
}