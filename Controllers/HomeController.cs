using OneMoreShot.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace OneMoreShot.Controllers
{
    public class HomeController : Controller
    {
        List<employeeData> empList = new List<employeeData>();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["birthdayEntities"].ConnectionString);

        // GET: Home
        public ActionResult Index()
        {
            conn.Open();
            string qry = "select * from employeeData";
            SqlCommand cmd = new SqlCommand(qry, conn);
            SqlDataReader dr=cmd.ExecuteReader();
            while (dr.Read())
            {
                employeeData emp = new employeeData
                {
                    EmpId = Convert.ToInt32(dr["EmpId"]),
                    EmpName = dr["EmpName"].ToString(),
                    Email = dr["Email"].ToString(),
                    PhNo = Convert.ToInt64(dr["PhNo"]),
                    EmpBDate = dr["EmpBDate"].ToString(),
                    img_name = dr["img_name"].ToString()
                };
                empList.Add(emp);
            }
            
            conn.Close();
            return View(empList);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(employeeData emp)
        {
            //C:\Users\ISHITA\source\repos\OneMoreShot\1\
            string fileName = Path.GetFileNameWithoutExtension(emp.img_file.FileName);
            string extention = Path.GetExtension(emp.img_file.FileName);
            fileName=fileName+DateTime.Now.ToString("yyyymmddss")+extention;
            emp.img_name= "~/1/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/1/"),fileName);
            emp.img_file.SaveAs(fileName);

            conn.Open();
            SqlCommand cmd = new SqlCommand("emp_Insert",conn);
            cmd.CommandType=CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@EmpId", emp.EmpId));
            cmd.Parameters.Add(new SqlParameter("@EmpName", emp.EmpName));
            cmd.Parameters.Add(new SqlParameter("@Email", emp.Email));
            cmd.Parameters.Add(new SqlParameter("@PhNo", emp.PhNo));
            cmd.Parameters.Add(new SqlParameter("@EmpBDate", emp.EmpBDate));
            cmd.Parameters.Add(new SqlParameter("@img_name", emp.img_name));

            cmd.ExecuteNonQuery();

            List<employeeData> empList = new List<employeeData>();
            
            string qry = "select * from employeeData";
            cmd = new SqlCommand(qry, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                employeeData emp1 = new employeeData
                {
                    EmpId = Convert.ToInt32(dr["EmpId"]),
                    EmpName = dr["EmpName"].ToString(),
                    Email = dr["Email"].ToString(),
                    PhNo = Convert.ToInt64(dr["PhNo"]),
                    EmpBDate = dr["EmpBDate"].ToString(),
                    img_name = dr["img_name"].ToString(),
                    img_file=emp.img_file
                };
                empList.Add(emp1);
            }

            conn.Close();
            

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int EmpId)
        {
            conn.Open();

            SqlCommand cmd=new SqlCommand("emp_Delete", conn);
            cmd.CommandType=CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@EmpId", EmpId));
            cmd.ExecuteNonQuery();
            conn.Close();

            var del_emp = empList.FirstOrDefault(e => e.EmpId == EmpId);
            if (del_emp != null)
            {
                empList.Remove(del_emp);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Details(int EmpId)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("emp_Details", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@EmpId", EmpId));
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            
            employeeData emp1 = new employeeData
            {
                EmpId = Convert.ToInt32(dr["EmpId"]),
                EmpName = dr["EmpName"].ToString(),
                Email = dr["Email"].ToString(),
                PhNo = Convert.ToInt64(dr["PhNo"]),
                EmpBDate = dr["EmpBDate"].ToString(),
                img_name = dr["img_name"].ToString()
            };
            dr.Close();
            conn.Close();
            return View(emp1);
            
        }
        public ActionResult Edit(int EmpId)
        {
            conn.Open();
            string qry = "select * from employeeData where EmpId=@EmpId";
            SqlCommand cmd = new SqlCommand(qry, conn);
            cmd.Parameters.Add(new SqlParameter("@EmpId", EmpId));
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
                employeeData emp1 = new employeeData
                {
                    EmpId = Convert.ToInt32(dr["EmpId"]),
                    EmpName = dr["EmpName"].ToString(),
                    Email = dr["Email"].ToString(),
                    PhNo = Convert.ToInt64(dr["PhNo"]),
                    EmpBDate = dr["EmpBDate"].ToString(),
                    img_name = dr["img_name"].ToString()
                };
                

            conn.Close();
            return View(emp1);
        }
        [HttpPost]
        public ActionResult Edit(employeeData emp)
        {
            conn.Open();

            //C:\Users\ISHITA\source\repos\OneMoreShot\1\
            
            string fileName = Path.GetFileNameWithoutExtension(emp.img_file.FileName);
            string extention = Path.GetExtension(emp.img_file.FileName);
            fileName = fileName + DateTime.Now.ToString("yyyymmddss") + extention;
            emp.img_name = "~/1/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/1/"), fileName);
            emp.img_file.SaveAs(fileName);
            

            SqlCommand cmd = new SqlCommand("emp_Update", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@EmpId", emp.EmpId));
            cmd.Parameters.Add(new SqlParameter("@EmpName", emp.EmpName));
            cmd.Parameters.Add(new SqlParameter("@Email", emp.Email));
            cmd.Parameters.Add(new SqlParameter("@PhNo", emp.PhNo));
            cmd.Parameters.Add(new SqlParameter("@EmpBDate", emp.EmpBDate));
            cmd.Parameters.Add(new SqlParameter("@img_name", emp.img_name));
            cmd.ExecuteNonQuery();

            var show_emp = empList.FirstOrDefault(e => e.EmpId == emp.EmpId);
            if (show_emp != null)
            {
                empList.FirstOrDefault(e => e.EmpId == emp.EmpId).EmpName=emp.EmpName;
                empList.FirstOrDefault(e => e.EmpId == emp.EmpId).Email=emp.Email;
                empList.FirstOrDefault(e => e.EmpId == emp.EmpId).PhNo = emp.PhNo;
                empList.FirstOrDefault(e => e.EmpId == emp.EmpId).EmpBDate = emp.EmpBDate;
                empList.FirstOrDefault(e => e.EmpId == emp.EmpId).img_name = emp.img_name;
            }
            
            return RedirectToAction("Index");

        }
    }
}