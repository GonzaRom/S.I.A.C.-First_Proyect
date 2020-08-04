using S.I.A.C.Models;

namespace S.I.A.C.Service
{
    internal interface IPeopleCommands
    {
        bool CreatePeople(RegistrationViewModel registrationViewModel);
    }
}