using System.ComponentModel;

namespace Domain.Constants.Enums;

public enum TransactionStatus
{
    [Description("Unpaid")]
    Unpaid,
    [Description("Paid")]
    Paid,  
}