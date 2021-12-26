using Newtonsoft.Json;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class Associate
    {
        [JsonProperty("AssociateId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public int AssociateId { get; set; }
        
        [JsonProperty("Name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public string Name { get; set; }
        
        [JsonProperty("PersonId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int PersonId { get; set; }
        
        [JsonProperty("Rank", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public int Rank { get; set; }
        
        [JsonProperty("Tooltip", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public string Tooltip { get; set; }
        
        [JsonProperty("Type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public string Type { get; set; }
        
        [JsonProperty("GroupIdx", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public int GroupIdx { get; set; }
        
        [JsonProperty("FullName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public string FullName { get; set; }
        
        [JsonProperty("FormalName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public string FormalName { get; set; }
        
        [JsonProperty("Deleted", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public bool Deleted { get; set; }
        
        [JsonProperty("EjUserId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public int EjUserId { get; set; }
        
        [JsonProperty("UserName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)] 
        public string UserName { get; set; }
    }

}
