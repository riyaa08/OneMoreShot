using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace OneMoreShot.Models
{
    public class email
    {
        static void Main(string[] args)
        {   
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["birthdayEntities"].ConnectionString);
            conn.Open();
            var date1 = DateTime.Today.ToString();
            string qry = "select * from employeeData where EmpBDate=@date1";
            SqlCommand cmd = new SqlCommand(qry,conn);
            SqlDataReader dr = cmd.ExecuteReader();
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
                
                //var bdate = emp.EmpBDate;
                if (date1==emp.EmpBDate)
                {
                    string fromMail = "ishu.252502@gmail.com";
                    string fromPassword = "ibkg cjbs dpzr sfjz";
                         
                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(fromMail);
                    msg.Subject = "Birthday Wishes...";
                    msg.To.Add(new MailAddress(emp.Email));
                    msg.Body = "<html><body background='https://img.freepik.com/free-vector/flat-golden-circle-balloons-birthday-background_52683-34659.jpg?t=st=1721368644~exp=1721372244~hmac=6c9d09b06ba6db26f868b7e8ee646796bd28da7743d262b6af7f398c4aedff33&w=996'>Happy Birthday<br><img src='https://as1.ftcdn.net/v2/jpg/03/25/69/56/1000_F_325695691_662cr0MVqqJ6NA1dzgmtlMtHCoT6aguX.jpg'></body></html>";
                    msg.IsBodyHtml = true;

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromMail, fromPassword),
                        EnableSsl = true,
                    };
                    smtpClient.Send(msg);
                }
            }  
        }
    }
}