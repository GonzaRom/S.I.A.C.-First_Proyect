using System.Text;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;
using S.I.A.C.Models.MailerModels;
using S.I.A.C.Models.ViewModels;

namespace S.I.A.C.Service.Implement
{
    public class MailerService : IMailer
    {
        private readonly PeopleQueriesService _peopleQueriesService = new PeopleQueriesService();
        private readonly SearchQueriesService _searchQueriesService = new SearchQueriesService();
        private readonly TicketQueriesService _ticketQueriesService = new TicketQueriesService();

        public void NotifyNewTicket(people ticketCreator, string description)
        {
            var adminsList = _peopleQueriesService.GetListAdmins();
            var fullNameAndId = GetNameAndIdString(ticketCreator.id, ticketCreator.name, ticketCreator.lastname);

            foreach (var admin in adminsList)
            {
                var email = new NewTicketEmail
                {
                    to = admin.emailAdmin,
                    userName = fullNameAndId,
                    comment = description
                };

                email.Send();
            }
        }

        public void NotifyUpdatedTicket(TicketHistoryViewModel updatedTicket, string ticketIdLocal)
        {
            if (int.TryParse(ticketIdLocal, out var ticketId))
            {
                var emailStrings = new string[2];
                var peopleData = new string[2];
                var ticketView = _ticketQueriesService.GeTicketViewModel(ticketId);
                var originalTicket = _searchQueriesService.SearchTicketByNumber(ticketId);

                if (ticketView.idAssignedTechnician != null)
                {
                    emailStrings[0] = _searchQueriesService.SearchEmailPeople((int) ticketView.idAssignedTechnician);
                    peopleData[0] = GetNameAndIdString((int) ticketView.idAssignedTechnician,
                        originalTicket.assignedTechnician,
                        originalTicket.assignedTechnicianLastname);
                }

                emailStrings[1] = _searchQueriesService.SearchEmailPeople(ticketView.idClient);
                peopleData[1] = GetNameAndIdString(ticketView.idClient, originalTicket.client,
                    originalTicket.clientLastname);


                var email = new UpdateTicketEmail
                {
                    to = emailStrings[1],
                    cc = emailStrings[0],
                    technicianName = peopleData[0],
                    userName = peopleData[1],
                    baseComment = originalTicket.description,
                    updateComment = updatedTicket.note,
                    idTicket = originalTicket.idLocal
                };

                email.Send();
            }
        }

        public void NotifyDeletedTicket(TicketPrintableModel deletedTicket, int ticketId, people currentPeople)
        {
            var emailStrings = new string[2];
            var peopleData = new string[2];
            var ticketView = _ticketQueriesService.GeTicketViewModel(ticketId);

            emailStrings[0] = currentPeople.email;
            peopleData[0] = GetNameAndIdString(currentPeople.id,
                currentPeople.name,
                currentPeople.lastname);

            emailStrings[1] = _searchQueriesService.SearchEmailPeople(ticketView.idClient);
            peopleData[1] = GetNameAndIdString(ticketView.idClient, deletedTicket.client,
                deletedTicket.clientLastname);


            var email = new DeletedTicketEmail
            {
                to = emailStrings[1],
                cc = emailStrings[0],
                currentUser = peopleData[0],
                clientName = peopleData[1],
                baseComment = ticketView.description,
                idTicket = deletedTicket.idLocal
            };

            email.Send();
        }

        private static string GetNameAndIdString(int id, string name, string lastname)
        {
            var nameAndIdString = new StringBuilder();
            if (name != null && lastname != null)
            {
                nameAndIdString.Append(id);
                nameAndIdString.Append(" - ");
                nameAndIdString.Append(name);
                nameAndIdString.Append(", ");
                nameAndIdString.Append(lastname);
            }

            return nameAndIdString.ToString();
        }
    }
}