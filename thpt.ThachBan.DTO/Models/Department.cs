﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace thpt.ThachBan.DTO.Models
{
    public partial class Department
    {
        public Department()
        {
            DepartmentManager = new HashSet<DepartmentManager>();
            Subject = new HashSet<Subject>();
        }

        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual ICollection<DepartmentManager> DepartmentManager { get; set; }
        public virtual ICollection<Subject> Subject { get; set; }
    }
}