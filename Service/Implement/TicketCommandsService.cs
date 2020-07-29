using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using System;
using System.Linq;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service
{
    public class TicketCommandsService : ITicketCommands
    {
        private dbSIACEntities _database;

        public string CreateTicket(TicketViewModel baseTicket, people sessionUser)
        {
            _database = new dbSIACEntities();

            var ticket = new ticket
            {
                idStatus = baseTicket.idStatus,
                idCreatorPeople = sessionUser.id,
                creationDate = baseTicket.creationDate,
                estimatedFinishDate = baseTicket.estimatedFinishDate,
                idPriority = baseTicket.idPriority,
                idAssignedTechnician = baseTicket.idAssignedTechnician,
                idCategory = baseTicket.idCategory,
                description = baseTicket.description
            };
            try
            {
                using (_database)
                {
                    _database.ticket.Add(ticket);
                    _database.SaveChanges();
                }

                return "Ticket creado exitosamente";
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool EditTicket(TicketViewModel baseTicket, string ticketId)
        {
            var id = int.Parse(ticketId);
            _database = new dbSIACEntities();

            using (_database)
            {
                var result = _database.ticket.SingleOrDefault(b => b.id == id);
                if (result == null)
                {
                    _database.Dispose();
                    return false;
                }
                else
                {
                    result.idCategory = baseTicket.idCategory;
                    result.idAssignedTechnician = baseTicket.idAssignedTechnician;
                    result.idPriority = baseTicket.idPriority;
                    result.idStatus = baseTicket.idStatus;
                    result.description = baseTicket.description;
                    _database.SaveChanges();
                }
            }

            _database.Dispose();
            return true;
        }

        public bool UpdateTicket(TicketHistoryViewModel baseTicket, string ticketId)
        {
            _database = new dbSIACEntities();
            ticketHistory updateTicket = new ticketHistory
            {
                date = DateTime.Now,
                idPeople = baseTicket.idPeople,
                idStatus = baseTicket.idStatus,
                idTicket = Int32.Parse(ticketId),
                note = baseTicket.note
            };
            try
            {
                using (_database)
                {
                    _database.ticketHistory.Add(updateTicket);
                    _database.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool UpdateTicketStatus(TicketViewModel baseTicket, string ticketId)
        {
            throw new NotImplementedException();
        }
    }
}