using System.Collections.Generic;
using System.Linq;
using S.I.A.C.Models;

namespace S.I.A.C.Service
{
    public class TicketService
    {
        private dbSIACEntities _database;

        private List<TicketPrintableModel> printableTickets = new List<TicketPrintableModel>();

        public List<TicketPrintableModel> GetTickets()
        {
            _database = new dbSIACEntities();

            using (_database)
            {
                //tickets=database.ticket.Where(active => active.idStatus == 1).ToList();  
                printableTickets = (from tick in _database.ticket
                    join creator in _database.people on tick.idCreatorPeople equals creator.id
                    join clientAddress in _database.people on tick.idClient equals clientAddress.id
                    join tecnician in _database.people on tick.idAssignedTechnician equals tecnician.id
                    select new TicketPrintableModel
                    {
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
            }
            return printableTickets;
        }
    }
}