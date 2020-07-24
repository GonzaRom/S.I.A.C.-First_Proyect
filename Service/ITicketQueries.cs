using S.I.A.C.Models;
using System.Collections.Generic;

namespace S.I.A.C.Service
{
    interface ITicketQueries
    {
        List<TicketPrintableModel> GetTicketsList();
        TicketViewModel GeTicketViewModel(int? ticketId);
    }
}
