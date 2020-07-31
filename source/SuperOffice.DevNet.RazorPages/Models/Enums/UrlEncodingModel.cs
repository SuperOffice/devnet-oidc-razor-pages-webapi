using System.Runtime.Serialization;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models.Enums
{
    public enum UrlEncodingModel
    {
        [EnumMember(Value = @"Unknown")]
        Unknown = 0,

        [EnumMember(Value = @"None")]
        None = 1,

        [EnumMember(Value = @"ANSI")]
        ANSI = 2,

        [EnumMember(Value = @"Unicode")]
        Unicode = 3,

    }
}