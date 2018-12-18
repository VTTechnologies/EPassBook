using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPassBook.Models
{
    public class WorkFlowStagesViewModel
    {
        [Key]
        public int StageId { get; set; }
        public string Stage { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual ICollection<StageinRoleViewModel> StageInRoles { get; set; }
    }

}