using System.ComponentModel;

namespace Domain.Constants.Enums
{
    public enum TranferFormStatus
    {
        [Description("Draft")]
        Draft,
        [Description("Requesting")]
        Requesting,
        [Description("Rejected")]
        Rejected,
        [Description("Approved")]
        Approved,
        [Description("Payment Complete")]
        PaymentComplete,
        [Description("Done")]
        Done,
    }
}
