using Newtonsoft.Json;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class WebhookModel
    {
        [Key]
        [JsonProperty("WebhookId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int WebhookId { get; set; }

        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [NotMapped]
        [JsonProperty("Events", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Events { get; set; }

        [JsonProperty("TargetUrl", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TargetUrl { get; set; }

        [JsonProperty("Secret", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Secret { get; set; }

        [JsonProperty("State", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))] 
        public WebhookState State { get; set; }

        [JsonProperty("Type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))] 
        public WebhookType Type { get; set; }

        [NotMapped]
        [JsonProperty("Headers", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("Registered", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Registered { get; set; }

        private Associate _associate;

        [NotMapped]
        [JsonProperty("RegisteredAssociate", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Associate RegisteredAssociate {
            get { return _associate; }
            set { 
                if(value != null)
                {
                    _registeredByID = value.AssociateId;
                    _registeredByName = value.FullName;
                }
            }
        }

        [JsonProperty("Updated", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Updated { get; set; }

        [NotMapped]
        [JsonProperty("UpdatedAssociate", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Associate UpdatedAssociate { get; set; }


        private string _registeredByName;
        public string RegisteredByName { get { return _registeredByName; } }

        private int _registeredByID;
        public int RegisteredByID { get { return _registeredByID; } }

    }
}
