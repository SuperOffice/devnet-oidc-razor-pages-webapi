using System.Runtime.Serialization;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models.Enums
{
    /// <summary>
    /// Webhook status - should we post events to the URL?, 1=Active, 2=Stopped or 3=TooManyErrors
    /// </summary>
    public enum WebhookState
    {
        [EnumMember(Value = @"Unknown")]
        Unknown = 0,

        [EnumMember(Value = @"Active")]
        Active = 1,

        [EnumMember(Value = @"Stopped")]
        Stopped = 2,

        [EnumMember(Value = @"TooManyErrors")]
        TooManyErrors = 3,
    }
}
