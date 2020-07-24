using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using System;

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
    }
}