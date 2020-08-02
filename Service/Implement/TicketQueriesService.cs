using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service
{
    public class TicketQueriesService : ITicketQueries
    {
        private dbSIACEntities _database;
        private List<TicketPrintableModel> printableTickets = new List<TicketPrintableModel>();
        private readonly TicketCommandsService _ticketCommandsService = new TicketCommandsService();

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
                        idLocal = tick.idLocal,
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

            _database.Dispose();
            return printableTickets;
        }

        public TicketViewModel GeTicketViewModel(int ticketIdLocal)
        {
            _database = new dbSIACEntities();
            var ticketViewModel = new TicketViewModel();
            using (_database)
            {
                var ticket = _database.ticket.FirstOrDefault(currentTicket => currentTicket.idLocal == ticketIdLocal);
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
            }

            _database.Dispose();
            return ticketViewModel;
        }

        public TicketPrintableModel GeTicketPrintableViewModel(int ticketIdLocal)
        {
            _database = new dbSIACEntities();
            TicketPrintableModel currentTickets;

            using (_database)
            {
                currentTickets = (from tick in _database.ticket
                    join creator in _database.people on tick.idCreatorPeople equals creator.id
                    join clientAddress in _database.people on tick.idClient equals clientAddress.id
                    join tecnician in _database.people on tick.idAssignedTechnician equals tecnician.id
                    where tick.idLocal == ticketIdLocal
                    select new TicketPrintableModel
                    {
                        idLocal = tick.idLocal,
                        address = clientAddress.address,
                        assignedTechnician = tecnician.name,
                        assignedTechnicianLastname = tecnician.lastname,
                        client = clientAddress.name,
                        clientLastname = clientAddress.lastname,
                        CreationDateTime = tick.creationDate,
                        creatorPeople = creator.name,
                        creatorPeopleLastname = creator.lastname,
                        description = tick.description,
                        status = tick.idStatus.ToString(),
                        email = clientAddress.email
                    }).FirstOrDefault();
            }

            _database.Dispose();
            return currentTickets;
        }

        public List<TicketHistoryViewModel> GeTicketHistoryViewModel(int ticketIdLocal)
        {
            _database = new dbSIACEntities();
            using (_database)
            {
                var ticketId = _ticketCommandsService.SearchTicketId(ticketIdLocal.ToString());
                var ticketHistoryList = (from tick in _database.ticketHistory
                    join creator in _database.people on tick.idPeople equals creator.id
                    where tick.idTicket == ticketId
                    select new TicketHistoryViewModel()
                    {
                        idTicket = tick.id,
                        date = tick.date,
                        idPeople = tick.idPeople,
                        idStatus = tick.idStatus,
                        note = tick.note,
                        peopleName = creator.name,
                        peopleLastName = creator.lastname
                    }).ToList();
                /* foreach (var currentTicket in ticketHistoryList)
                 {
                     currentTicket.SetFullName();
                 }*/

                return ticketHistoryList;
            }
        }
    }
}