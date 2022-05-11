using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BonfirePizza.Models
{
    public class Common
    {

        public string SMSTemplate { get; set; }
        public string MessageType { get; set; }
        public string AddedBy { get; set; }
    
        public string Pk_BranchID { get; set; }
        public string UpdatedBy { get; set; }
        public string ReferBy { get; set; }
        public string Result { get; set; }
        public string ErrorMessage { get; set; }
        public string DisplayName { get; set; }
        public string AddedOn { get; set; }
        public string EncrptNo { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
         public string DeletedBy { get; set; }
        public string PinCode { get;  set; }
        public string State { get; set; }
        public string City { get; set; }
        public string MonthName { get; set; }
        public string MonthId { get; set; }
        public string IsPaid { get; set; }
         public string IDLoginId { get; set; }
     
        public class SMSCredential
        {
            public static string UserName = "seemagrouop";
            public static string Password = "123123";
            public static string SenderId = "THENRJ";
            public static string tempid = "1607100000000133025";
            public static string clientMobileNo = "7055087866";


            public static string customertempid = "1607100000000133027";
        }


        public static List<SelectListItem> BindPreferableTime()
        {
            List<SelectListItem> PreferableTime = new List<SelectListItem>();
            PreferableTime.Add(new SelectListItem { Text = "Select Preferable Time", Value =  null });
            PreferableTime.Add(new SelectListItem { Text = "07 AM - 08 AM", Value = "07 AM - 08 AM" });
            PreferableTime.Add(new SelectListItem { Text = "08 AM - 09 AM", Value = "08 AM - 09 AM" });
            PreferableTime.Add(new SelectListItem { Text = "09 AM - 10 AM", Value = "09 AM - 10 AM" });
            PreferableTime.Add(new SelectListItem { Text = "10 AM - 11 AM", Value = "09 AM - 10 AM" });
            PreferableTime.Add(new SelectListItem { Text = "11 AM - 12 PM", Value = "11 AM - 12 PM" });
            PreferableTime.Add(new SelectListItem { Text = "12 PM - 01 PM", Value = "12 PM - 01 PM" });
            PreferableTime.Add(new SelectListItem { Text = "01 PM - 02 PM", Value = "01 PM - 02 PM" });
            PreferableTime.Add(new SelectListItem { Text = "02 PM - 03 PM", Value = "02 PM - 03 PM" });
            PreferableTime.Add(new SelectListItem { Text = "03 PM - 04 PM", Value = "03 PM - 04 PM" });
            PreferableTime.Add(new SelectListItem { Text = "04 PM - 05 PM", Value = "04 PM - 05 PM" });
            PreferableTime.Add(new SelectListItem { Text = "05 PM - 06 PM", Value = "05 PM - 06 PM" });
            PreferableTime.Add(new SelectListItem { Text = "09 PM - 10 PM", Value = "09 AM - 10 AM" });
            return PreferableTime;
        }
        public static string GenerateRandom()
        {
            Random r = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
            {
                s = string.Concat(s, r.Next(10).ToString());
            }
            return s;

        }

        public class PaymentGateWayDetails
        {
            public static string CreateOrder = "https://api.razorpay.com/v1/orders";
            public static string CapturePayment = "https://api.razorpay.com/v1/payments/";
            public static string FetchPaymentByOrderURL = "https://api.razorpay.com/v1/orders/";
            public static string KeyName = "rzp_test_ttV6lOBKMV4EU4";
            public static string SecretKey = "OHndiKT0TfN8dKT27eZADBdZ";

            // public static string KeyName = "rzp_test_VSnr1Dm89baTnm";
            // public static string SecretKey = "6G6WnMAj9UpxZ9mDZBPBof4A";
        }

    }
    
}