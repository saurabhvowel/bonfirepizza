using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonfirePizza.Models;
using BonfirePizza.Filter;
using System.Data;
using System.Net.Mail;
using System.Net;

using Razorpay.Api;
using static BonfirePizza.Models.Common;

namespace BonfirePizza.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult AdminLogin()
        {
            Session.Abandon();
            return View();
        }

        public ActionResult LoginAction(Home obj)
        {
            string FormName = "";
            string Controller = "";
            try
            {
                Home Modal = new Home();
                DataSet ds = obj.Login();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    //{
                    //    TempData["Login"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    //}

                    if (ds.Tables[0].Rows[0]["UserType"].ToString() != "Vendor")
                    {
                        //Session["LoginId"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                        Session["Pk_AdminId"] = ds.Tables[0].Rows[0]["PK_UserID"].ToString();
                        Session["UserType"] = ds.Tables[0].Rows[0]["UserType"].ToString();
                        Session["Name"] = ds.Tables[0].Rows[0]["Fullname"].ToString();
                        Session["ProfilePic"] = ds.Tables[0].Rows[0]["ProfilePic"].ToString();

                        FormName = "DashBoard";
                        Controller = "Admin";

                    }

                    else
                    {
                        TempData["Login"] = "Incorrect Login ID Or Password";
                        FormName = "Login";
                        Controller = "Home";
                    }
                }
                else
                {
                    TempData["Login"] = "Incorrect Login ID Or Password";
                    FormName = "Login";
                    Controller = "Home";
                }
            }
            catch (Exception ex)
            {
                TempData["Login"] = "Incorrect Login ID Or Password";
                FormName = "Login";
                Controller = "Home";
            }

            return RedirectToAction(FormName, Controller);
        }

        public ActionResult Index(Home obj)
        {
            if (TempData["SendSMS"] != null)
            {

                obj.Null = "1";
            }

            List<Home> list = new List<Home>();
            DataSet ds = obj.GettingMenu();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Home obj1 = new Home();
                    obj1.PK_MenuID = r["PK_MenuID"].ToString();
                    obj1.MenuName = r["MenuName"].ToString();
                    list.Add(obj1);
                }

                obj.lstMenu = list;
            }

            List<SelectListItem> ddlPreferableTime = Common.BindPreferableTime();
            ViewBag.ddlPreferableTime = ddlPreferableTime;

            //#region MainCategory
            //int count = 0;
            //List<SelectListItem> ddlMainCategory = new List<SelectListItem>();
            //DataSet ds1 = obj.GettingMainCategory();
            //if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow r in ds1.Tables[0].Rows)
            //    {
            //        if (count == 0)
            //        {
            //            ddlMainCategory.Add(new SelectListItem { Text = "Select Main Category", Value = "0" });
            //        }
            //        ddlMainCategory.Add(new SelectListItem { Text = r["MainCategoryName"].ToString(), Value = r["PK_MainCategoryID"].ToString() });
            //        count = count + 1;
            //    }
            //}

            //ViewBag.ddlMainCategory = ddlMainCategory;

            //#endregion


            #region MainCategory
            List<SelectListItem> ddlMainCategory = new List<SelectListItem>();
            //DataSet ds1 = obj1.GettingMainCategory();
            //if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow r in ds1.Tables[0].Rows)
            //    {
            //        if (count == 0)
            //        {
            //            ddlMainCategory.Add(new SelectListItem { Text = "Select Main Category", Value = "0" });
            //        }
            //        ddlMainCategory.Add(new SelectListItem { Text = r["MainCategoryName"].ToString(), Value = r["PK_MainCategoryID"].ToString() });
            //        count = count + 1;
            //    }
            //}

            ViewBag.ddlMainCategory = ddlMainCategory;

            #endregion


            return View(obj);
        }

        [HttpPost]
        [ActionName("Index")]
        [OnAction(ButtonName = "SendSMS")]
        public ActionResult SendSMS(Home obj)
        {

            Session["Email"] = obj.Email;
            Session["PaidPrice"] = obj.PaidPrice;
            Session["Address"] = obj.Address;
            Session["ContainOrders"] = obj.ContainOrders;
            Session["Name"] = obj.Name;
            
            CreateOrder obj1 = new CreateOrder();
            Session["sub_total"] = obj.PaidPrice;
            Session["Mobile"] = obj.MobileNo;
            ViewBag.CartTotal = Math.Round(Convert.ToDecimal(obj.PaidPrice) * 100);
            CreateOrderResponse obj2 = new CreateOrderResponse();
            string random = Common.GenerateRandom();
            try
            {

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", ViewBag.CartTotal); // amount in the smallest currency unit
                options.Add("receipt", random);
                options.Add("currency", "INR");
                options.Add("payment_capture", "1");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                RazorpayClient client = new RazorpayClient(PaymentGateWayDetails.KeyName, PaymentGateWayDetails.SecretKey);
                Razorpay.Api.Order order = client.Order.Create(options);
                obj2.OrderId = order["id"].ToString();
                obj2.Status = "0";
                obj1.OrderId = order["id"].ToString();
                obj1.Pk_UserId = Session["Mobile"].ToString();
                obj1.TransactionType = "Wallet Web";
                obj1.Type = "Card";
                obj1.amount = obj.PaidPrice;
                DataSet ds = obj1.SaveOrderDetails();

                Session["OrderId"] = order["id"].ToString();
                Session["Amount"] = ViewBag.CartTotal;  

            }
            catch (Exception ex)
            {
                obj2.Status = "1";
                obj2.ErrorMessage = ex.Message;
            }


            return RedirectToAction("Payment");
        }

        public virtual PartialViewResult MenuHotel()
        {
            Home Menu = null;

            if (Session["_MenuHotel"] != null)
            {
                Menu = (Home)Session["_MenuHotel"];
            }
            else
            {

                Menu = Home.GetMenus(Session["Pk_AdminId"].ToString(), Session["UserType"].ToString()); // pass employee id here
                Session["_MenuHotel"] = Menu;
            }
            return PartialView("_MenuHotel", Menu);
        }

        #region razorpay

        public ActionResult CreateOrder(string Amount,string Mobile)
        {
            CreateOrder obj = new CreateOrder();
            Session["sub_total"] = Amount;
            Session["Mobile"] = Mobile;
            ViewBag.CartTotal = Math.Round(Convert.ToDecimal(Amount) * 100);
            CreateOrderResponse obj1 = new CreateOrderResponse();
            string random = Common.GenerateRandom();
            try
            {

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", ViewBag.CartTotal); // amount in the smallest currency unit
                options.Add("receipt", random);
                options.Add("currency", "INR");
                options.Add("payment_capture", "1");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                RazorpayClient client = new RazorpayClient(PaymentGateWayDetails.KeyName, PaymentGateWayDetails.SecretKey);
                Razorpay.Api.Order order = client.Order.Create(options);
                obj1.OrderId = order["id"].ToString();
                obj1.Status = "0";
                obj.OrderId = order["id"].ToString();
                obj.Pk_UserId = Session["Mobile"].ToString() ;
                obj.TransactionType = "Wallet Web";
                obj.Type = "Card";
                obj.amount = Amount;
                DataSet ds = obj.SaveOrderDetails();

                Session["OrderId"] = order["id"].ToString();
                Session["Amount"] = ViewBag.CartTotal;

            }
            catch (Exception ex)
            {
                obj1.Status = "1";
                obj1.ErrorMessage = ex.Message;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Payment()
        {
            Home obj = new Home();

            obj.TotalAmount = Session["Amount"].ToString();
            obj.OrderNo = Session["OrderId"].ToString();

            Session["Mobile"] = Session["Mobile"].ToString();
            //obj.EmailId = Session["EmailId"].ToString();
            return View(obj);
        }

        public ActionResult PaymentSuccess(string paymentid)
        {
            string paymentid1 = paymentid;
            ViewBag.d = Session["Amount"].ToString();
            #region Start PlaceOrder Process


            Home model = new Home();
            decimal d = (Decimal)Session["Amount"];
            decimal sde = d / 100;
            string str = sde.ToString("0.00").Replace(".00", String.Empty);
            model.PaymentMode = "Online";
            model.PaymentID = paymentid1;
            model.FinalAmount = str;
            try
            {
                model.MobileNo = Session["Mobile"].ToString();

                DataSet dsCustomerOrder = model.PlaceCustomerOrderOnline();
                if (dsCustomerOrder != null && dsCustomerOrder.Tables.Count > 0)
                {
                    if (dsCustomerOrder.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        try
                        {

                            model.OrderNo = dsCustomerOrder.Tables[0].Rows[0]["OrderNo"].ToString();
                            SmtpClient mailServer = new SmtpClient("smtp.mail.yahoo.com", 587);
                            mailServer.EnableSsl = true;
                            mailServer.UseDefaultCredentials = true;
                            mailServer.Credentials = new System.Net.NetworkCredential("bonfirepizza@yahoo.com", "urqkfycdpdnhjcuq");

                            using (var mess1 = new MailMessage("bonfirepizza@yahoo.com", "priyajoshi14468@gmail.com")
                            {
                                Subject = "Order from " + model.MobileNo + " - Bonfire Pizza",
                                Body = "Hi," + "<br>" + "<br>" + "You got order from" + "<br>" + "Name: " + Session["Name"].ToString() + " , Mobile: " + model.MobileNo + " , Email: " + Session["Email"].ToString() + "<br>" + "Address: " + Session["Address"].ToString() + "<br>" + "His/Her orders are : " + Session["ContainOrders"].ToString() + "<br>" + "Total amount : ₹ " + Session["PaidPrice"].ToString()
                            })


                            {
                                mess1.IsBodyHtml = true;
                                mailServer.Send(mess1);
                            }
                            using (var mess2 = new MailMessage("bonfirepizza@yahoo.com", Session["Email"].ToString())
                            {
                                Subject = "Order Confirmation - Bonfire Pizza",
                                Body = "Hi " + Session["Name"].ToString() + "," + "<br>" + "<br>" + "We have received your orders. You will get your food shortly." + "<br>" + "Your orders are : " + Session["ContainOrders"].ToString() + "<br>" + "Total amount : ₹ " + Session["PaidPrice"].ToString() + "<br>" + "Delivery Address: " + Session["Address"].ToString() + "<br>" + " Mobile : " + model.MobileNo
                            })


                            {
                                mess2.IsBodyHtml = true;
                                mailServer.Send(mess2);
                            }
                            string mobile2 = model.MobileNo;
                            string clientMob = "9650672515" + ',' + "9548448084" + ',' + "8896223445";
                            BLSMS.SendSMSClient(clientMob, " got orders from " + Session["Name"].ToString() + ", His/her mobile no. is: " + mobile2 + " %26 the orders are: " + Session["ContainOrders"].ToString() + "-Bonfire Pizza");

                            BLSMS.SendSMSNew(mobile2, " customer, We have received your orders. You will get your food shortly. Have a nice day. -Bonfire Pizza Rishikesh");



                            TempData["SendSMS"] = "Msg Sent";
                        }
                        catch (Exception ex1)
                        { }
                    }
                    else if (dsCustomerOrder.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["CustomerOrder"] = dsCustomerOrder.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                TempData["CustomerOrder"] = ex.Message;
            }
            #endregion end PlaceOrder Process
            return View(model);
        }
        #endregion

        public JsonResult CategoryItems(string PK_MainCategoryID)
        {
            Home model = new Home();
            if (PK_MainCategoryID != null)
            {
                model.FK_MainCategoryID = PK_MainCategoryID;
                List<Home> lst = new List<Home>();

                DataSet ds = model.GettingCategoryItems();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Home obj = new Home();
                        obj.PK_CategoryItemsID = r["PK_CategoryItemsID"].ToString();
                        obj.FK_MainCategoryID = r["FK_MainCategoryID"].ToString();
                        obj.MainCategoryName = r["MainCategoryName"].ToString();
                        obj.ItemName = r["ItemName"].ToString();
                        obj.Price = r["Price"].ToString();


                        lst.Add(obj);
                    }
                    model.lstCategoryItems = lst;
                }
            }




            return Json(model, JsonRequestBehavior.AllowGet);

        }



        public ActionResult Index2(Home obj)
        {
            if (TempData["SendSMS"] != null)
            {

                obj.Null = "1";
            }

            List<SelectListItem> ddlPreferableTime = Common.BindPreferableTime();
            ViewBag.ddlPreferableTime = ddlPreferableTime;

            #region MainCategory
            int count = 0;
            List<SelectListItem> ddlMainCategory = new List<SelectListItem>();
            DataSet ds1 = obj.GettingRoomDinningMainCategory();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMainCategory.Add(new SelectListItem { Text = "Select Main Category", Value = "0" });
                    }
                    ddlMainCategory.Add(new SelectListItem { Text = r["MainCategoryName"].ToString(), Value = r["PK_MainCategoryID"].ToString() });
                    count = count + 1;
                }
            }

            ViewBag.ddlMainCategory = ddlMainCategory;

            #endregion



            return View(obj);
        }

        [HttpPost]
        [ActionName("Index2")]
        [OnAction(ButtonName = "SendSMS")]
        public ActionResult SendSMS2(Home obj)
        {


            string clientMob = "";
           // BLSMS.SendSMS2(Common.SMSCredential.UserName, Common.SMSCredential.Password, Common.SMSCredential.SenderId, clientMob, "You got orders from Room No: " + obj.RoomNo + ". Mobile No is: " + obj.MobileNo + ". -THE NEERAJ FOREST RESORT HOLISTIC HEALTH SPA", Common.SMSCredential.tempid, "1");


/*
            SmtpClient mailServer = new SmtpClient("smtp.gmail.com", 587);
            mailServer.EnableSsl = true;
            mailServer.UseDefaultCredentials = true;
            mailServer.Credentials = new System.Net.NetworkCredential("theneerajforest@gmail.com", "Neerajforest@123");

            using (var mess1 = new MailMessage("theneerajforest@gmail.com", "order@theneerajforestresort.com")
            {
                Subject = "Order for The Bonefire Pizza" ,
                Body = "His/Her orders are : " + obj.ContainOrders + ". And His/Her preferred time is: " + obj.PreferableTime
            })
            {
                mess1.IsBodyHtml = true;
                mailServer.Send(mess1);
            }
*/
            string mobile2 = obj.MobileNo;
            //    mobile2 = mobile + ',' + "9910098768";

            BLSMS.SendSMSNew(mobile2, " customer, We have received your orders. You will get your food shortly. Have a nice day. -Bonfire Pizza Rishikesh");




            TempData["SendSMS"] = "Msg Sent";
            return RedirectToAction("Index2");
        }

        public JsonResult RoomDinningCategoryItems(string PK_MainCategoryID)
        {
            Home model = new Home();
            if (PK_MainCategoryID != null)
            {
                model.FK_MainCategoryID = PK_MainCategoryID;
                List<Home> lst = new List<Home>();

                DataSet ds = model.GettingRoomDinningCategoryItems();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Home obj = new Home();
                        obj.PK_CategoryItemsID = r["PK_CategoryItemsID"].ToString();
                        obj.FK_MainCategoryID = r["FK_MainCategoryID"].ToString();
                        obj.MainCategoryName = r["MainCategoryName"].ToString();
                        obj.ItemName = r["ItemName"].ToString();
                        obj.Price = r["Price"].ToString();


                        lst.Add(obj);
                    }
                    model.lstCategoryItems = lst;
                }
            }




            return Json(model, JsonRequestBehavior.AllowGet);

        }

    }
}