//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPassBook.DAL.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class StageInRole
    {
        public int Id { get; set; }
        public Nullable<int> StageId { get; set; }
        public Nullable<int> RoleId { get; set; }
    
        public virtual RoleMaster RoleMaster { get; set; }
        public virtual WorkflowStage WorkflowStage { get; set; }
    }
}
