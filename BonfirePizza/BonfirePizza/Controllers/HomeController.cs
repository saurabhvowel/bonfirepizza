using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonfirePizza.Models;
using BonfirePizza.Filter;
using System.Data;
using System.Net.Mail;

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


            string clientMob = "8896223445" + ',' + "7897294077";
            BLSMS.SendSMS2(Common.SMSCredential.UserName, Common.SMSCredential.Password, Common.SMSCredential.SenderId, clientMob, "You got orders from Room No: " + obj.RoomNo + ". Mobile No is: " + obj.MobileNo + ". -THE NEERAJ FOREST RESORT HOLISTIC HEALTH SPA", Common.SMSCredential.tempid, "1");



            SmtpClient mailServer = new SmtpClient("smtp.gmail.com", 587);
            mailServer.EnableSsl = true;
            mailServer.UseDefaultCredentials = true;
            mailServer.Credentials = new System.Net.NetworkCredential("voweldigitaladworld@gmail.com", "mudmnvgqtyvffkmq");

            using (var mess1 = new MailMessage("voweldigitaladworld@gmail.com", "voweldigitaladworld@gmail.com")
            {
                Subject = "The Bonefire Pizza Order ",
                Body = "Name: "+ obj.Name + " , Mobile: "+ obj.MobileNo + " , Email: " + obj.Email + " , Address: "+ obj.Address +  " , & His/Her orders are : " + obj.ContainOrders 
            })
            {
                mess1.IsBodyHtml = true;
                mailServer.Send(mess1);
            }

            string mobile2 = obj.MobileNo;
            BLSMS.SendSMS(Common.SMSCredential.UserName, Common.SMSCredential.Password, Common.SMSCredential.SenderId, mobile2, "The Neeraj River Forest Resort has received your orders. As soon as you got your Orders. Have a nice day. - www.theneerajspa.com", Common.SMSCredential.customertempid, "1");




            TempData["SendSMS"] = "Msg Sent";
            return RedirectToAction("Index");
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


            string clientMob = "8077320232" + ',' + "7055087866" + ',' + "7906558228" + ',' + "8630943854";
            BLSMS.SendSMS2(Common.SMSCredential.UserName, Common.SMSCredential.Password, Common.SMSCredential.SenderId, clientMob, "You got orders from Room No: " + obj.RoomNo + ". Mobile No is: " + obj.MobileNo + ". -THE NEERAJ FOREST RESORT HOLISTIC HEALTH SPA", Common.SMSCredential.tempid, "1");



            SmtpClient mailServer = new SmtpClient("smtp.gmail.com", 587);
            mailServer.EnableSsl = true;
            mailServer.UseDefaultCredentials = true;
            mailServer.Credentials = new System.Net.NetworkCredential("theneerajforest@gmail.com", "Neerajforest@123");

            using (var mess1 = new MailMessage("theneerajforest@gmail.com", "order@theneerajforestresort.com")
            {
                Subject = "Order from Room No: " + obj.RoomNo,
                Body = "His/Her orders are : " + obj.ContainOrders + ". And His/Her preferred time is: " + obj.PreferableTime
            })
            {
                mess1.IsBodyHtml = true;
                mailServer.Send(mess1);
            }

            string mobile2 = obj.MobileNo;
            //    mobile2 = mobile + ',' + "9910098768";

            BLSMS.SendSMS(Common.SMSCredential.UserName, Common.SMSCredential.Password, Common.SMSCredential.SenderId, mobile2, "The Neeraj River Forest Resort has received your orders. As soon as you got your Orders. Have a nice day. - www.theneerajspa.com", Common.SMSCredential.customertempid, "1");




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