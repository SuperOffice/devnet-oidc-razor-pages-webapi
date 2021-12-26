using System.Runtime.Serialization;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models.Enums
{
    /// <summary>
    /// Name of plugin that handles this webhook. 'webhook' for webhooks, or 'crmscript' for crmscript.
    /// </summary>
    public enum WebhookType
    {
        [EnumMember(Value = @"webhook")]
        Webhook = 0,

        [EnumMember(Value = @"crmscript")]
        CrmScript = 1,
    }
}
