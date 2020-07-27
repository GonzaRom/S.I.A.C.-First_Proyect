using S.I.A.C.Models;
using System.Collections.Generic;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service
{
    interface ITicketQueries
    {
        List<TicketPrintableModel> GetTicketsList();
        TicketViewModel GeTicketViewModel(int? ticketId);
        TicketPrintableModel GeTicketPrintableViewModel(int ticketId);
        List<TicketHistoryViewModel> GeTicketHistoryViewModel(int? ticketId);
    }
}