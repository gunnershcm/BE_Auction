using System.ComponentModel;

namespace Domain.Constants.Enums;

public enum AuctionStatus
{
    [Description("ComingUp")]
    ComingUp,
    [Description("InProgress")]
    InProgress,
    [Description("Finished")]
    Finished,
}