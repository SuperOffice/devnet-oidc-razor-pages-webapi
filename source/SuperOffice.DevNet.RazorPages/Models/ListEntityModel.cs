using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class ListEntityModel : ListBase
    {
        public bool IsCustomList { get; set; }
        public bool IsMDOList { get; set; }
        public bool UseGroupsAndHeadings { get; set; }
        public string ListType { get; set; }
        public bool InUseByUserDefinedFields { get; set; }
    }
}
