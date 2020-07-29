using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service
{
    interface ITicketCommands
    {
        string CreateTicket(TicketViewModel baseTicket, people sessionUser);
        bool EditTicket(TicketViewModel baseTicket, string ticketId);
        bool UpdateTicket(TicketHistoryViewModel baseTicket, string ticketId);
        bool UpdateTicketStatus(TicketViewModel baseTicket, string ticketId);
    }
}
