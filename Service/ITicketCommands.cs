using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;

namespace S.I.A.C.Service
{
    interface ITicketCommands
    {
        string CreateTicket(TicketViewModel baseTicket, people sessionUser);

    }
}
