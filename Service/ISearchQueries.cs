using System.Collections.Generic;
using S.I.A.C.Models;

namespace S.I.A.C.Service
{
    interface ISearchQueries
    {
        List<TicketPrintableModel> SearchTicket(string stringToSearch);
        List<TicketPrintableModel> SearchTicketByClient(int idClient);
        TicketPrintableModel SearchTicketByNumber(int localIdTicket);
    }
}
