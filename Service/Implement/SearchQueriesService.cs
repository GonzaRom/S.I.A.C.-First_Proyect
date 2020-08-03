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
                    join technician in _database.people on tick.idAssignedTechnician equals technician.id
                    select new TicketPrintableModel
                    {
                        idLocal = tick.idLocal,
                        address = clientAddress.address,
                        assignedTechnician = technician.name,
                        assignedTechnicianLastname = technician.lastname,
                        client = clientAddress.name,
                        clientLastname = clientAddress.lastname,
                        CreationDateTime = tick.creationDate,
                        creatorPeople = creator.name,
                        creatorPeopleLastname = creator.lastname,
                        description = tick.description,
                        email = clientAddress.email
                    }).OrderByDescending(x => x.CreationDateTime);
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

                    return foundTickets.ToList();
                }

                return null;
            }
        }

        public List<TicketPrintableModel> SearchTicketByClient(int idClient)
        {
            if (idClient <= 0) return null;

            _database = new dbSIACEntities();
            using (_database)
            {
                IQueryable<TicketPrintableModel> foundTickets =
                    (from ticket in _database.ticket
                        where ticket.idClient == idClient
                        join creator in _database.people on ticket.idCreatorPeople equals creator.id
                        join clientAddress in _database.people on ticket.idClient equals clientAddress.id
                        join tecnician in _database.people on ticket.idAssignedTechnician equals tecnician.id
                        select new TicketPrintableModel
                        {
                            idLocal = ticket.idLocal,
                            address = clientAddress.address,
                            assignedTechnician = tecnician.name,
                            assignedTechnicianLastname = tecnician.lastname,
                            client = clientAddress.name,
                            clientLastname = clientAddress.lastname,
                            CreationDateTime = ticket.creationDate,
                            creatorPeople = creator.name,
                            creatorPeopleLastname = creator.lastname,
                            description = ticket.description,
                            email = clientAddress.email
                        }).OrderByDescending(x => x.CreationDateTime);
                return foundTickets.ToList();
            }
        }

        public TicketPrintableModel SearchTicketByNumber(int localIdTicket)
        {
            if (localIdTicket <= 0) return null;

            _database = new dbSIACEntities();
            using (_database)
            {
                if (_database.ticket.FirstOrDefault(currentTicket => currentTicket.idLocal == localIdTicket) !=
                    null)
                {
                    var foundTickets = (from ticket in _database.ticket
                        where ticket.idLocal == localIdTicket
                        join creator in _database.people on ticket.idCreatorPeople equals creator.id
                        join clientAddress in _database.people on ticket.idClient equals clientAddress.id
                        join tecnician in _database.people on ticket.idAssignedTechnician equals tecnician.id
                        select new TicketPrintableModel
                        {
                            idLocal = ticket.idLocal,
                            address = clientAddress.address,
                            assignedTechnician = tecnician.name,
                            assignedTechnicianLastname = tecnician.lastname,
                            client = clientAddress.name,
                            clientLastname = clientAddress.lastname,
                            CreationDateTime = ticket.creationDate,
                            creatorPeople = creator.name,
                            creatorPeopleLastname = creator.lastname,
                            description = ticket.description,
                            email = clientAddress.email
                        }).FirstOrDefault();
                    return foundTickets;
                }
            }

            return null;
        }
    }
}