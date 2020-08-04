using System.Collections.Generic;
using System.Linq;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;
using S.I.A.C.Service.Implement;

namespace S.I.A.C.Service
{
    public class TicketQueriesService : ITicketQueries
    {
        private readonly SearchQueriesService _searchQueriesService = new SearchQueriesService();
        private dbSIACEntities _database;
        private List<TicketPrintableModel> printableTickets = new List<TicketPrintableModel>();

        public List<TicketPrintableModel> GetActiveTicketsList(people currentPeople)
        {
            if (currentPeople.idRol == 3)
                return printableTickets = _searchQueriesService.SearchActiveTicketByClient(currentPeople.id);

            _database = new dbSIACEntities();
            using (_database)
            {
                printableTickets = (from ticket in _database.ticket
                    where ticket.idStatus != (int) EStatus.Finalizado
                    where ticket.idStatus != (int) EStatus.Cancelado
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
                    }).ToList();
            }

            return printableTickets;
        }

        public TicketViewModel GeTicketViewModel(int ticketIdLocal)
        {
            _database = new dbSIACEntities();
            var ticketViewModel = new TicketViewModel();
            using (_database)
            {
                var ticket = _database.ticket.FirstOrDefault(currentTicket => currentTicket.idLocal == ticketIdLocal);
                if (ticket == null) return null;

                ticketViewModel.idAssignedTechnician = ticket.idAssignedTechnician;
                ticketViewModel.creationDate = ticket.creationDate;
                ticketViewModel.estimatedFinishDate = ticket.estimatedFinishDate;
                ticketViewModel.idPriority = ticket.idPriority;
                ticketViewModel.idCategory = ticket.idCategory;
                ticketViewModel.idStatus = ticket.idStatus;
                ticketViewModel.creationDate = ticket.creationDate;
                ticketViewModel.description = ticket.description;
            }

            _database.Dispose();
            return ticketViewModel;
        }

        public List<TicketHistoryViewModel> GeTicketHistoryViewModel(int ticketIdLocal)
        {
            _database = new dbSIACEntities();
            using (_database)
            {
                var ticketId = _searchQueriesService.SearchTicketId(ticketIdLocal.ToString());
                var ticketHistoryList = (from ticket in _database.ticketHistory
                    join creator in _database.people on ticket.idPeople equals creator.id
                    where ticket.idTicket == ticketId
                    select new TicketHistoryViewModel
                    {
                        idTicket = ticket.id,
                        date = ticket.date,
                        idPeople = ticket.idPeople,
                        idStatus = ticket.idStatus,
                        note = ticket.note,
                        peopleName = creator.name,
                        peopleLastName = creator.lastname
                    }).ToList();

                return ticketHistoryList;
            }
        }
    }
}