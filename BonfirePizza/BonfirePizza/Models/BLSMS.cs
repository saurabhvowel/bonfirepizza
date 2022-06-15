using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace BonfirePizza.Models
{
    static public class BLSMS
    {
        public static void SendSMS2(string User, string password, string senderid, string Mobile_Number, string Message,string tempid,string messageType)
        {
           /* string strUrl = "http://otpmsg.in/sms-panel/api/http/index.php?username=agesart&password=121212&sender=SSGOTP&tempid=1207161458731628344&receiver=" + mobile + "&route=TA&msgtype=1&sms=" + msg;

            string strurl1 = "http://otpmsg.in/sms-panel/api/http/index.php?username=bonfire&apikey=03954-EA7A6&apirequest=Text&sender=BNFPZA&mobile="+Mobile_Number+"&message=Dear"+ Message+ "&route=APISMS&TemplateID=1607100000000215151";
            WebRequest request = HttpWebRequest.Create(strUrl);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = (Stream)response.GetResponseStream();
            StreamReader readStream = new StreamReader(s);
            string dataString = readStream.ReadToEnd();
            response.Close();
            s.Close();
            readStream.Close();
            return dataString;*/
        }

        public static string SendSMSNew(string mobile, string msg)
        {
            string strurl1 = "http://otpmsg.in/sms-panel/api/http/index.php?username=bonfire&apikey=03954-EA7A6&apirequest=Text&sender=BNFPZA&mobile=" + mobile + "&message=Dear" + msg+ "&route=APISMS&TemplateID=1607100000000215151";
            WebRequest request = HttpWebRequest.Create(strurl1);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = (Stream)response.GetResponseStream();
            StreamReader readStream = new StreamReader(s);
            string dataString = readStream.ReadToEnd();
            response.Close();
            s.Close();
            readStream.Close();
            return dataString;

        }


        public static string SendSMSClient(string clientMob, string msg)
        {
            //string strUrl = "http://msgblast.in/index.php/smsapi/httpapi/?uname=kapil&password=123@123" + mobile + "&senderid=SSGOTP&message=" + msg + "";
            string strurl1 = "http://otpmsg.in/sms-panel/api/http/index.php?username=bonfire&apikey=03954-EA7A6&apirequest=Text&sender=BNFPZA&mobile=" + clientMob + "&message=Dear,You " + msg + "&route=APISMS&TemplateID=1607100000000215155";
            ////http://msgblast.in/index.php/smsapi/httpapi/?uname=agesart&password=121212&sender=SSGOTP&tempid=1207161458731628344&receiver=6391408888&route=TA&msgtype=1&sms=Hi, Your OTP for login is 6678
            WebRequest request = HttpWebRequest.Create(strurl1);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = (Stream)response.GetResponseStream();
            StreamReader readStream = new StreamReader(s);
            string dataString = readStream.ReadToEnd();
            response.Close();
            s.Close();
            readStream.Close();
            return dataString;

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