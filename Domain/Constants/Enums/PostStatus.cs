using System.ComponentModel;

namespace Domain.Constants.Enums;

public enum PostStatus
{
    [Description("Draft")]
    Draft,
    [Description("Requesting")]
    Requesting,
    [Description("Rejected")]
    Rejected,
    [Description("Approved")]
    Approved,
    [Description("Completed")]
    Completed,
}