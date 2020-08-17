using System.Collections.Generic;
using S.I.A.C.Models;

namespace S.I.A.C.Service
{
    internal interface ISearchQueries
    {
        List<TicketPrintableModel> SearchTicket(string stringToSearch, int idRol, int userId);
        List<TicketPrintableModel> SearchActiveTicketByClient(int idClient, int idRol);
        TicketPrintableModel SearchTicketByNumber(int ticketIdLocal);
        int SearchTicketId(string ticketIdLocal);
        TicketPrintableModel SearchTicketByIdAndUser(int ticketIdLocal, int idRol, int userId);
        string SearchEmailPeople(int id);
    }
}