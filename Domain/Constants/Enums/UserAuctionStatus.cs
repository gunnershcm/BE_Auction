using System.ComponentModel;

namespace Domain.Constants.Enums;

public enum UserAuctionStatus
{
    [Description("ComingUp")]
    ComingUp,
    [Description("InProgress")]
    InProgress,
    [Description("Finished")]
    Finished,
}