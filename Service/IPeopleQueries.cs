using System.Collections.Generic;
using S.I.A.C.Models;
using S.I.A.C.Models.DomainModels;

namespace S.I.A.C.Service
{
    internal interface IPeopleQueries
    {
        people SearchPeople(string email, string password);
        PeopleViewModel GetDetailsPeople(int peopleId);
        List<PeopleViewModel> GetListClients();
        List<TechniciansViewModel> GetListTechnicians();
        List<AdminsViewModel> GetListAdmins();
    }
}