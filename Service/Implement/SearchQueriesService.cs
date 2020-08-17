using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.Enum;

namespace S.I.A.C.Service.Implement
{
    public class SearchQueriesService : ISearchQueries
    {
        private dbSIACEntities _database;

        /// <summary>
        ///     Use idRol and userId to reduce the number of records in the database.
        ///     Then, search within that list, comparing idLocal and possible hits even if it is not a complete word.
        /// </summary>
        /// <param name="stringToSearch"></param>
        /// <param name="idRol"></param>
        /// <param name="userId"></param>
        /// <returns>List&lt;TicketPrintableModel&gt;</returns>
        public List<TicketPrintableModel> SearchTicket(string stringToSearch, int idRol, int userId)
        {
            if (stringToSearch.IsNullOrWhiteSpace()) return null;
            var toSearch = stringToSearch.Trim().ToLower();

            var smallFilterTickets = _filterTicketsByRolAndId(idRol, userId);
            var filteredTickets = _IQuerablePrintableModels(smallFilterTickets);

            _database = new dbSIACEntities();
            using (_database)
            {
                if (int.TryParse(toSearch, out var toSearchInt))
                {
                    var foundedTicketbyId =
                        filteredTickets.Where(currentTicket => currentTicket.idLocal.Equals(toSearchInt));
                    var founderdList = foundedTicketbyId.ToList();
                    if (founderdList.Any()) return founderdList;
                }

                filteredTickets = filteredTickets.Where(currentTicket =>
                    currentTicket.assignedTechnician.ToLower().Contains(toSearch) ||
                    currentTicket.assignedTechnicianLastname.ToLower().Contains(toSearch) ||
                    currentTicket.client.ToLower().Contains(toSearch) ||
                    currentTicket.clientLastname.ToLower().Contains(toSearch) ||
                    currentTicket.address.ToLower().Contains(toSearch) ||
                    currentTicket.description.ToLower().Contains(toSearch));
                return filteredTickets.ToList();
            }
        }

        /// <summary>
        ///     Find all active tickets of a client.
        /// </summary>
        /// <param name="idPeople"></param>
        /// <param name="idRol"></param>
        /// <returns>List&lt;TicketPrintableModel&gt;</returns>
        public List<TicketPrintableModel> SearchActiveTicketByClient(int idPeople, int idRol)
        {
            if (idPeople <= 0) return null;
            var filteredTickets = _filterTicketsByRolAndId(idRol, idPeople);
            _database = new dbSIACEntities();
            using (_database)
            {
                var foundTickets = (from foundTicket in filteredTickets
                    where foundTicket.idStatus != (int) EStatus.Finalizado
                    where foundTicket.idStatus != (int) EStatus.Cancelado
                    join creator in _database.people on foundTicket.idCreatorPeople equals creator.id
                    join clientAddress in _database.people on foundTicket.idClient equals clientAddress.id
                    join tecnician in _database.people on foundTicket.idAssignedTechnician equals tecnician.id
                    select new TicketPrintableModel
                    {
                        idLocal = foundTicket.idLocal,
                        address = clientAddress.address,
                        assignedTechnician = tecnician.name,
                        assignedTechnicianLastname = tecnician.lastname,
                        client = clientAddress.name,
                        clientLastname = clientAddress.lastname,
                        CreationDateTime = foundTicket.creationDate,
                        creatorPeople = creator.name,
                        creatorPeopleLastname = creator.lastname,
                        description = foundTicket.description,
                        email = clientAddress.email,
                        status = foundTicket.idStatus
                    }).OrderByDescending(x => x.CreationDateTime);

                return foundTickets.ToList();
            }
        }

        /// <summary>
        ///     Find the ticket that has the same idLocal assigned to it. Does not respect ticket ownership or technician
        ///     assignment.
        /// </summary>
        /// <param name="ticketIdLocal"></param>
        /// <returns>TicketPrintableModel</returns>
        public TicketPrintableModel SearchTicketByNumber(int ticketIdLocal)
        {
            if (ticketIdLocal <= 0) return null;

            _database = new dbSIACEntities();
            using (_database)
            {
                if (_database.ticket.FirstOrDefault(currentTicket => currentTicket.idLocal == ticketIdLocal) !=
                    null)
                {
                    var foundTickets = (from ticket in _database.ticket
                        where ticket.idLocal == ticketIdLocal
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
                            email = clientAddress.email,
                            status = ticket.idStatus
                        }).FirstOrDefault();
                    return foundTickets;
                }
            }

            return null;
        }

        /// <summary>
        ///     Find the ticket that has the same idLocal assigned to it.
        ///     Respecting that the user who performs the search is the creator, the client or the assigned technician.
        /// </summary>
        /// <param name="ticketIdLocal"></param>
        /// <param name="idRol"></param>
        /// <param name="userId"></param>
        /// <returns name="TicketPrintableModel"></returns>
        public TicketPrintableModel SearchTicketByIdAndUser(int ticketIdLocal, int idRol, int userId)
        {
            if (ticketIdLocal <= 0) return null;
            var smallFilterTickets = _filterTicketsByRolAndId(idRol, userId);
            _database = new dbSIACEntities();
            using (_database)
            {
                if (smallFilterTickets.FirstOrDefault(currentTicket => currentTicket.idLocal == ticketIdLocal) !=
                    null)
                {
                    var foundTickets = (from ticket in smallFilterTickets
                        where ticket.idLocal == ticketIdLocal
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
                            email = clientAddress.email,
                            status = ticket.idStatus
                        }).FirstOrDefault();
                    return foundTickets;
                }
            }

            return null;
        }

        /// <summary>
        ///     Shortcut to get the real id of a ticket according to its localID.
        /// </summary>
        /// <param name="ticketIdLocal"></param>
        /// <returns></returns>
        public int SearchTicketId(string ticketIdLocal)
        {
            if (!int.TryParse(ticketIdLocal, out var localTicketId)) return 0;
            _database = new dbSIACEntities();
            using (_database)
            {
                var entityTicketId = _database.ticket.FirstOrDefault(current => current.idLocal == localTicketId);
                return entityTicketId?.id != null ? entityTicketId.id : 0; //demasiado comprimido?
            }
        }

        /// <summary>
        ///     Search email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string SearchEmailPeople(int id)
        {
            if (id <= 0) return null;
            _database = new dbSIACEntities();
            using (_database)
            {
                var email = _database.people.Find(id)?.email;
                return email;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="idRol"></param>
        /// <param name="userId"></param>
        /// <returns>IQueryable&lt;ticket&gt;</returns>
        private IEnumerable<ticket> _filterTicketsByRolAndId(int idRol, int userId)
        {
            _database = new dbSIACEntities();

            IQueryable<ticket> smallFilterTickets = null;
            switch (idRol)
            {
                case (int) ERols.Technician:
                {
                    smallFilterTickets = _database.ticket.Where(ticket => ticket.idAssignedTechnician == userId);
                    break;
                }
                case (int) ERols.Client:
                {
                    smallFilterTickets = _database.ticket.Where(ticket => ticket.idClient == userId);
                    break;
                }
                case (int) ERols.TechnicianLead:
                {
                    smallFilterTickets = _database.ticket;
                    break;
                }
            }

            return smallFilterTickets.AsEnumerable();
        }

        /// <summary>
        /// </summary>
        /// <param name="IquerableTickets"></param>
        /// <returns></returns>
        private IEnumerable<TicketPrintableModel> _IQuerablePrintableModels(IEnumerable<ticket> IquerableTickets)
        {
            _database = new dbSIACEntities();

            var iqTicketPrintableModel = (from tick in IquerableTickets
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
                    email = clientAddress.email,
                    status = tick.idStatus
                }).OrderByDescending(x => x.CreationDateTime);
            return iqTicketPrintableModel;
        }
    }
}