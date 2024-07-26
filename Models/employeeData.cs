using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OneMoreShot.Models
{
    public class employeeData
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Email { get; set; }
        public Int64 PhNo { get; set; }
        public string EmpBDate { get; set; }
        [DisplayName("Upload File")]
        public string img_name { get; set; }
        public HttpPostedFileBase img_file { get; set; }
    }
}