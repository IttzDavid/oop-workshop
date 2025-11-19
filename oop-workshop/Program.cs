using System;
using System.Collections.Generic;

namespace OopWorkshop
{
    internal static class Program
    {

    }
}

public enum Role
{
    Guest,
    User,
    Admin
}

public class User
{
    Guid id;
    string Name;
    int Age;
    string SocialSecurityNumber;
    DateTime CreatedAt;

    public Role IdentifyRole()
    {
        if (string.IsNullOrWhiteSpace(SocialSecurityNumber))
            return Role.Guest;

        if (Age >= 18 && string.Equals(Name, "Admin", StringComparison.OrdinalIgnoreCase))
            return Role.Admin;

        return Role.User;
    }

    // Returns a list of available actions based on the user's role.
    public List<string> ViewAvailableActions()
    {
        switch (IdentifyRole())
        {
            case Role.Admin:
                return new List<string> { "CreateUser", "DeleteUser", "ViewReports", "EditSettings" };
            case Role.User:
                return new List<string> { "ViewProfile", "EditProfile", "SubmitReport" };
            case Role.Guest:
            default:
                return new List<string> { "Register", "ViewPublicContent" };
        }
    }
}