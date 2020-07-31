using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class ListItemModel : ListBase
    {
        public string Type { get; set; }
        public int ColorBlock { get; set; }
        public string IconHint { get; set; }
        public bool Selected { get; set; }
        public DateTime LastChanged { get; set; }
        public IList<ListItemModel> ChildItems { get; set; }
        public string ExtraInfo { get; set; }
        public string StyleHint { get; set; }
        public bool Hidden { get; set; }
        public string FullName { get; set; }
    }
}
