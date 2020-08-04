using System.Collections.Generic;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service
{
    internal interface ITicketQueries
    {
        List<TicketPrintableModel> GetActiveTicketsList(people currentPeople);
        TicketViewModel GeTicketViewModel(int ticketIdLocal);
        List<TicketHistoryViewModel> GeTicketHistoryViewModel(int ticketIdLocal);
    }
}