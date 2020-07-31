using Newtonsoft.Json;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class WebPanelEntityModel
    {
        /// <summary>The identity of the web panel</summary>
        [Key]
        [JsonProperty("WebPanelId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int WebPanelId { get; set; }

        /// <summary>True if the web panel is marked as deleted</summary>
        [JsonProperty("Deleted", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Deleted { get; set; } = false;

        /// <summary>The icon of the webpanel</summary>
        [JsonProperty("Icon", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Icon { get; set; }

        /// <summary>Used to distinquish application specific web panels from all web panels.</summary>
        [JsonIgnore]
        public bool IsAppWebPanel { get; set; }

        /// <summary>The name of the web panel</summary>
        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>Is the webpanel visible when user is on central database</summary>
        [JsonProperty("OnCentral", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool OnCentral { get; set; } = true;

        /// <summary>Is the webpanel visible when user is on a satellite</summary>
        [JsonProperty("OnSatellite", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool OnSatellite { get; set; } = true;

        /// <summary>Is the webpanel visible when user is on travel</summary>
        [JsonProperty("OnTravel", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool OnTravel { get; set; } = false;

        /// <summary>Is the webpanel visible when user is on web client</summary>
        [JsonProperty("OnSalesMarketingWeb", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool OnSalesMarketingWeb { get; set; } = true;

        /// <summary>Is the webpanel visible when user is on pocket client</summary>
        [JsonProperty("OnSalesMarketingPocket", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool OnSalesMarketingPocket { get; set; } = false;

        /// <summary>String key that can be used to uniquely retrieve the panel; particularly useful for partners and others who do not wish to store database ID's</summary>
        [JsonProperty("ProgId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ProgId { get; set; } = string.Empty;

        /// <summary>The rank of the web panel</summary>
        [JsonProperty("Rank", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Rank { get; set; }

        /// <summary>Does the webpanel have an address bar</summary>
        [JsonProperty("ShowInAddressBar", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool ShowAddressBar { get; set; } = false;

        /// <summary>Does the webpanel have a menu bar</summary>
        [JsonProperty("ShowInMenuBar", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool ShowMenuBar { get; set; } = false;

        /// <summary>Does the webpanel have a status bar</summary>
        [JsonProperty("ShowInStatusBar", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool ShowStatusBar { get; set; } = false;

        /// <summary>Does the webpanel have a toolbar</summary>
        [JsonProperty("ShowInToolBar", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool ShowToolBar { get; set; } = false;

        /// <summary>The tooltip of the web panel</summary>
        [JsonProperty("Tooltip", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Tooltip { get; set; } = string.Empty;

        /// <summary>The url</summary>
        [JsonProperty("Url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; } = string.Empty;

        /// <summary>The encoding of the URL</summary>
        [JsonProperty("UrlEncoding", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public UrlEncodingModel UrlEncoding { get; set; } = UrlEncodingModel.None;


        /// <summary>The webpanel is visible in</summary>
        [JsonProperty("VisibleIn", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public VisibleInModel VisibleIn { get; set; }

        /// <summary>The window which the URL address is to open in (webpanel only)</summary>
        [JsonProperty("WindowName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string WindowName { get; set; }
    }
}