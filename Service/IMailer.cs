using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service
{
    internal interface IMailer
    {
        void NotifyNewTicket(people ticketCreator, string description);
        void NotifyUpdatedTicket(TicketHistoryViewModel updatedTicket, string ticketIdLocal);
        void NotifyDeletedTicket(TicketPrintableModel deletedTicket, int ticketId, people currentPeople);
    }
}