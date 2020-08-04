using System.Collections.Generic;
using S.I.A.C.Models;

namespace S.I.A.C.Service
{
    internal interface ISearchQueries
    {
        List<TicketPrintableModel> SearchTicket(string stringToSearch);
        List<TicketPrintableModel> SearchActiveTicketByClient(int idClient);
        TicketPrintableModel SearchTicketByNumber(int ticketIdLocal);
        int SearchTicketId(string ticketIdLocal);
    }
}