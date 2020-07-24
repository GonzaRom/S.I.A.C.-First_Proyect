using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using System.Collections.Generic;
using System.Linq;

namespace S.I.A.C.Service
{
    public class TicketQueriesService : ITicketQueries
    {
        private dbSIACEntities _database;

        private List<TicketPrintableModel> printableTickets = new List<TicketPrintableModel>();

        public List<TicketPrintableModel> GetTicketsList()
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
            }

            return printableTickets;
        }

        public TicketViewModel GeTicketViewModel(int? ticketId)
        {
            _database = new dbSIACEntities();
            var ticketViewModel = new TicketViewModel();
            using (_database)
            {
                var ticket = _database.ticket.Find(ticketId);
                if (ticket == null)
                {
                    return null;
                }

                ticketViewModel.idAssignedTechnician = ticket.idAssignedTechnician;
                ticketViewModel.creationDate = ticket.creationDate;
                ticketViewModel.estimatedFinishDate = ticket.estimatedFinishDate;
                ticketViewModel.idPriority = ticket.idPriority;
                ticketViewModel.idCategory = ticket.idCategory;
                ticketViewModel.idStatus = ticket.idStatus;
                ticketViewModel.creationDate = ticket.creationDate;
                ticketViewModel.description = ticket.description;

                _database.Dispose();
            }

            return ticketViewModel;
        }
    }
}