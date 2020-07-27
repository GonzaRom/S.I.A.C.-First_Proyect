using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using System;
using System.Linq;

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
            _database =new dbSIACEntities();

            using (_database)
            {
                var result = _database.ticket.SingleOrDefault(b => b.id == id);
                if (result == null)
                {
                    _database.Dispose();
                    return false;
                }
                else{
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

        public bool UpdateTicketStatus(TicketViewModel baseTicket, string ticketId)
        {
            throw new NotImplementedException();
        }
    }
}