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

        /// <summary>
        /// Create a new ticket, if a client is the creator idClient is taken from the sessionUser info. Othewise from the ViewModel.
        /// </summary>
        /// <param name="baseTicket"></param>
        /// <param name="sessionUser"></param>
        /// <returns>True ticket created. False something went wrong</returns>
        public (bool result, int idLocal) CreateTicket(TicketViewModel baseTicket, people sessionUser)
        {
            var random = new Random();
            baseTicket.idLocal = random.Next(1000, 99999); //Algun dia llegara a mas de esa cantidad de tickets?

            _database = new dbSIACEntities();
            if (baseTicket.idClient == 0)
            {
                baseTicket.idClient = sessionUser.id;
            }

            var ticket = new ticket
            {
                idLocal = baseTicket.idLocal,
                idStatus = baseTicket.idStatus,
                idCreatorPeople = sessionUser.id,
                creationDate = baseTicket.creationDate,
                estimatedFinishDate = baseTicket.estimatedFinishDate,
                idPriority = baseTicket.idPriority,
                idCategory = baseTicket.idCategory,
                description = baseTicket.description,
                idClient = baseTicket.idClient,
                idAssignedTechnician = baseTicket.idAssignedTechnician
            };
            try
            {
                using (_database)
                {
                    _database.ticket.Add(ticket);
                    _database.SaveChanges();
                }
                return (true, baseTicket.idLocal);
                //TODO Hangfire { send email to admin/technical-supervisor}
            }
            catch (Exception ex)
            {
                return (false, 0);
            }
        }

        public int SearchTicketId(string ticketId)
        {
            _database = new dbSIACEntities();
            using (_database)
            {
                var localTicketId = Int32.Parse(ticketId);
                var entityTicketId = _database.ticket.FirstOrDefault(current => current.idLocal == localTicketId);
                return entityTicketId?.id ?? 0; //demasiado comprimido?
            }
        }

        public bool EditTicket(TicketViewModel baseTicket, string ticketId)
        {
            var id = int.Parse(ticketId);
            _database = new dbSIACEntities();

            using (_database)
            {
                var result = _database.ticket.SingleOrDefault(b => b.idLocal == id);
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
            var entityTicketId = SearchTicketId(ticketId);
            _database = new dbSIACEntities();
            ticketHistory updateTicket = new ticketHistory
            {
                date = DateTime.Now,
                idPeople = baseTicket.idPeople,
                idStatus = baseTicket.idStatus,
                idTicket = entityTicketId,
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