using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service
{
    interface ITicketCommands
    {
        (bool result,int idLocal) CreateTicket(TicketViewModel baseTicket, people sessionUser);
        int SearchTicketId(string ticketId);
        bool EditTicket(TicketViewModel baseTicket, string ticketId);
        bool UpdateTicket(TicketHistoryViewModel baseTicket, string ticketId);
        bool UpdateTicketStatus(TicketViewModel baseTicket, string ticketId);
    }
}
