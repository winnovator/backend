using System.Diagnostics.CodeAnalysis;

namespace WInnovator.Helper
{
    [ExcludeFromCodeCoverage]
    public static class DefaultUsersAndRoles
    {
        public static readonly string defaultMailPartOfAppUserAccounts = "@winnovator.noreply.windesheim.nl";

        public static string[] getRoles()
        {
            return new string[] {
                "Administrator",
                "Facilitator",
                "Gebruiker",
                "FrontendApp"
            };
        }

        public static UserData[] getDefaultUsers()
        {
            return new UserData[]
            {
                new UserData() { email="winnovator@windesheim.nl", password="Welkom@01", defaultRoles=new string[] { "Administrator", "Facilitator", "Gebruiker" } },
                new UserData() { email="maarten.groensmit@windesheim.nl", password="Welkom@01", defaultRoles=new string[] { "Administrator", "Facilitator", "Gebruiker" } },
                new UserData() { email="marc.hoeve@windesheim.nl", password="Welkom@01", defaultRoles=new string[] { "Administrator", "Facilitator", "Gebruiker" } },
                new UserData() { email="niek.pruntel@windesheim.nl", password="Welkom@01", defaultRoles=new string[] { "Administrator", "Facilitator", "Gebruiker" } },
                new UserData() { email="gert.stoevelaar@windesheim.nl", password="Welkom@01", defaultRoles=new string[] { "Administrator", "Facilitator", "Gebruiker" } },
            };
        }
    }

    [ExcludeFromCodeCoverage]
    public class UserData
    {
        public string email { get; set; }
        public string password { get; set; }
        public string[] defaultRoles { get; set; }
    }

}
