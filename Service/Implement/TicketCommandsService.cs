using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Antlr.Runtime;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.MailerModels;
using S.I.A.C.Models.ViewModels;
using S.I.A.C.Service.Implement;
using Xamarin.Forms;

namespace S.I.A.C.Service
{
    public class TicketCommandsService : ITicketCommands
    {
        private readonly SearchQueriesService _searchQueriesService = new SearchQueriesService();
        private dbSIACEntities _database;

        /// <summary>
        ///     Create a new ticket, if a client is the creator idClient is taken from the sessionUser info. Othewise from the
        ///     ViewModel.
        /// </summary>
        /// <param name="baseTicket"></param>
        /// <param name="sessionUser"></param>
        /// <returns>True ticket created. False something went wrong</returns>
        public (bool result, int idLocal) CreateTicket(TicketViewModel baseTicket, people sessionUser)
        {
            var random = new Random();
            baseTicket.idLocal = random.Next(1000, 99999); //Algun dia llegara a mas de esa cantidad de tickets?

            _database = new dbSIACEntities();
            if (baseTicket.idClient == 0) baseTicket.idClient = sessionUser.id;

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
            }
            catch (Exception)
            {
                return (false, 0);
            }
        }


        public bool EditTicket(TicketViewModel baseTicket, string ticketId)
        {
            if (!int.TryParse(ticketId, out var currentTicketId)) return false;

            _database = new dbSIACEntities();
            using (_database)
            {
                var result = _database.ticket.FirstOrDefault(b => b.idLocal == currentTicketId);
                if (result == null)
                {
                    _database.Dispose();
                    return false;
                }

                result.idCategory = baseTicket.idCategory;
                result.idAssignedTechnician = baseTicket.idAssignedTechnician;
                result.idPriority = baseTicket.idPriority;
                result.idStatus = baseTicket.idStatus;
                result.description = baseTicket.description;
                _database.SaveChanges();
            }

            return true;
        }

        public bool UpdateTicket(TicketHistoryViewModel baseTicket, string ticketIdLocal)
        {
            var entityTicketId = _searchQueriesService.SearchTicketId(ticketIdLocal);
            if (!UpdateTicketStatus(baseTicket.idStatus, entityTicketId)) return false;

            _database = new dbSIACEntities();
            var updateTicket = new ticketHistory
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
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool UpdateTicketStatus(int idStatus, int ticketId)
        {
            if (ticketId <= 0) return false;

            _database = new dbSIACEntities();
            using (_database)
            {
                var ticketDefault = _database.ticket.FirstOrDefault(ticket => ticket.id == ticketId);
                if (ticketDefault == null) return false;

                ticketDefault.idStatus = idStatus;
                ticketDefault.editionDate = DateTime.Now;
                try
                {
                    _database.ticket.AddOrUpdate(ticketDefault);
                    _database.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }
    }
}