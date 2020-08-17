using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service
{
    internal interface ITicketCommands
    {
        (bool result, int idLocal) CreateTicket(TicketViewModel baseTicket, people sessionUser);
        bool EditTicket(TicketViewModel baseTicket, string ticketId);
        bool UpdateTicket(TicketHistoryViewModel baseTicket, string ticketIdLocal);
        bool UpdateTicketStatus(int idStatus, int ticketId);
    }
}