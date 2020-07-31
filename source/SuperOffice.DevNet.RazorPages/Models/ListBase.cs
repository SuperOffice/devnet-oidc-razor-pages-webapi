using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class ListBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tooltip { get; set; }
        public bool Deleted { get; set; }
        public int Rank { get; set; }
    }
}
