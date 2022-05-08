using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PoolSideMenu.Models
{
    static public class BLSMS
    {
        public static void SendSMS2(string User, string password, string senderid, string Mobile_Number, string Message,string tempid,string messageType)
        {
            try
            {
                string SMSAPI = ConfigurationSettings.AppSettings["SMSAPI"].ToString();
                SMSAPI = SMSAPI.Replace("[AND]", "&");
                SMSAPI = SMSAPI.Replace("[receiver]", Mobile_Number);
                SMSAPI = SMSAPI.Replace("[sms]", Message);
                SMSAPI = SMSAPI.Replace("[tempid]", tempid);
                SMSAPI = SMSAPI.Replace("[messageType]", messageType);


                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(SMSAPI, false));
                HttpWebResponse httpResponse = (HttpWebResponse)(httpReq.GetResponse());
            }
            catch (Exception ex)
            {
            }
        }


        public static void SendSMS(string User, string password, string senderid, string Mobile_Number, string Message, string tempid, string messageType)
        {
            try
            {
                string SMSAPI = ConfigurationSettings.AppSettings["SMSAPI"].ToString();
                SMSAPI = SMSAPI.Replace("[AND]", "&");
                SMSAPI = SMSAPI.Replace("[receiver]", Mobile_Number);
                SMSAPI = SMSAPI.Replace("[sms]", Message);
                SMSAPI = SMSAPI.Replace("[tempid]", tempid);
                SMSAPI = SMSAPI.Replace("[messageType]", messageType);


                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(SMSAPI, false));
                HttpWebResponse httpResponse = (HttpWebResponse)(httpReq.GetResponse());
            }
            catch (Exception ex)
            {
            }
        }

    }
}