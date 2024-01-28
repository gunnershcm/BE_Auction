using System.ComponentModel;

namespace Domain.Constants.Enums;

public enum Role
{
    [Description("Admin")]
    Admin,
    [Description("Guest")]
    Guest,
    [Description("Staff")]
    Staff,
    [Description("Member")]
    Member,
}
