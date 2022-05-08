using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoolSideMenu.Filter;
using System.Data;
using System.Data.SqlClient;
using PoolSideMenu.Models;

namespace PoolSideMenu.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        #region MainCategory
        public ActionResult MainCategory(string PK_MainCategoryID)
        {
            List<Admin> list = new List<Admin>();
            Admin model = new Admin();
            DataSet ds = model.GettingMainCategory();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_MainCategoryID = r["PK_MainCategoryID"].ToString();
                    obj.MainCategoryName = r["MainCategoryName"].ToString();
                    obj.MenuName = r["MenuName"].ToString();
                    list.Add(obj);
                }

                model.lstMainCategory = list;
            }

            #region Menu
            Admin obj1 = new Admin();
            int count = 0;
            List<SelectListItem> ddlMenu = new List<SelectListItem>();
            DataSet ds1 = obj1.GettingMenu();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMenu.Add(new SelectListItem { Text = "Select Menu", Value = "0" });
                    }
                    ddlMenu.Add(new SelectListItem { Text = r["MenuName"].ToString(), Value = r["PK_MenuID"].ToString() });
                    count = count + 1;
                }
            }

            ViewBag.ddlMenu = ddlMenu;

            #endregion

            return View(model);
        }

        public ActionResult GetMainCategory(string PK_MenuID)
        {
            List<Admin> list = new List<Admin>();
            Admin model = new Admin();
            model.PK_MenuID = PK_MenuID;
            #region Get
            List<SelectListItem> ddlMainCategory = new List<SelectListItem>();
            DataSet ds = model.GettingMainCategory();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    ddlMainCategory.Add(new SelectListItem { Text = r["MainCategoryName"].ToString(), Value = r["PK_MainCategoryID"].ToString() });

                }
            }

            model.ddlMainCategory = ddlMainCategory;
            #endregion
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [OnAction(ButtonName = "saveBtn")]
        [ActionName("MainCategory")]
        public ActionResult SaveMainCategory(Admin model)
        {

            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.SaveMainCategory();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["MainCategory"] = "Main Category saved successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["MainCategory"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["MainCategory"] = ex.Message;
            }
            return RedirectToAction("MainCategory");
        }

        public ActionResult DeleteMainCategory(string id)
        {
            try
            {
                Admin obj = new Admin();
                obj.PK_MainCategoryID = id;
                obj.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = obj.DeleteMainCategory();

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["MainCategory"] = "Main Category deleted successfully";
                    }
                    else
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["MainCategory"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["MainCategory"] = ex.Message;
            }
            return RedirectToAction("MainCategory");
        }

        #endregion

        #region CategoryItems
        public ActionResult CategoryItems(string CategoryID)
        {
            Admin model = new Admin();
            if (CategoryID != null)
            {
                model.PK_CategoryItemsID = CategoryID;
                List<Admin> lst = new List<Admin>();

                DataSet ds = model.GettingCategoryItems();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    //  model.DiscountID = DiscountID;

                    model.PK_CategoryItemsID = ds.Tables[0].Rows[0]["PK_CategoryItemsID"].ToString();
                    model.FK_MainCategoryID = ds.Tables[0].Rows[0]["FK_MainCategoryID"].ToString();
                    model.PK_MainCategoryID = ds.Tables[0].Rows[0]["FK_MainCategoryID"].ToString();
                    model.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                    model.Price = ds.Tables[0].Rows[0]["Price"].ToString();


                    model.lstCategoryItems = lst;

                }
            }

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
            #region Menu
            Admin obj1 = new Admin();
            int count = 0;
            List<SelectListItem> ddlMenu = new List<SelectListItem>();
            DataSet ds1 = obj1.GettingMenu();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMenu.Add(new SelectListItem { Text = "Select Menu", Value = "0" });
                    }
                    ddlMenu.Add(new SelectListItem { Text = r["MenuName"].ToString(), Value = r["PK_MenuID"].ToString() });
                    count = count + 1;
                }
            }

            ViewBag.ddlMenu = ddlMenu;

            #endregion

            return View(model);

        }

        public ActionResult CategoryItemsList(Admin model)
        {
            List<Admin> lst = new List<Admin>();

            // model.SiteID = model.SiteID == "0" ? null : model.SiteID;

            DataSet ds = model.GettingCategoryItems();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_CategoryItemsID = r["PK_CategoryItemsID"].ToString();
                    obj.FK_MainCategoryID = r["FK_MainCategoryID"].ToString();
                    obj.MainCategoryName = r["MainCategoryName"].ToString();
                    obj.ItemName = r["ItemName"].ToString();
                    obj.Price = r["Price"].ToString();


                    lst.Add(obj);
                }
                model.lstCategoryItems = lst;
            }

            return View(model);
        }


        [HttpPost]
        [ActionName("CategoryItems")]
        [OnAction(ButtonName = "btnSave")]
        public ActionResult SaveCategoryItems(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();

                DataSet ds = model.SaveCategoryItems();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["Category"] = " Category added successfully !";
                    }
                    else
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["Category"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["Category"] = ex.Message;
            }
            return RedirectToAction("CategoryItems");
        }
        [HttpPost]
        [ActionName("CategoryItems")]
        [OnAction(ButtonName = "btnUpdate")]
        public ActionResult UpdateCategoryItems(Admin obj)
        {

            try
            {

                obj.AddedBy = Session["Pk_AdminId"].ToString();
                //obj.dtProductQuantity = dtst;

                DataSet ds = obj.UpdateCategoryItems();

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["Category"] = " Category Updated successfully !";
                    }
                    else
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["Category"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["Category"] = ex.Message;
            }
            return RedirectToAction("CategoryItems");
        }



        public ActionResult DeleteCategoryItems(string id)
        {
            try
            {
                Admin obj = new Admin();
                obj.PK_CategoryItemsID = id;
                obj.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = obj.DeleteCategoryItems();

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["Category"] = "Category deleted successfully";
                    }
                    else
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["Category"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["Category"] = ex.Message;
            }
            return RedirectToAction("CategoryItemsList");
        }
        #endregion




        #region RoomDinningRoomDinningMainCategory
        public ActionResult RoomDinningMainCategory(string PK_MainCategoryID)
        {
            List<Admin> list = new List<Admin>();
            Admin model = new Admin();
            DataSet ds = model.GettingRoomDinningMainCategory();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_MainCategoryID = r["PK_MainCategoryID"].ToString();
                    obj.MainCategoryName = r["MainCategoryName"].ToString();
                    list.Add(obj);
                }

                model.lstRoomDinningMainCategory = list;
            }

            return View(model);
        }

        [HttpPost]
        [OnAction(ButtonName = "saveBtn")]
        [ActionName("RoomDinningMainCategory")]
        public ActionResult SaveRoomDinningMainCategory(Admin model)
        {

            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.SaveRoomDinningMainCategory();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["RoomDinningMainCategory"] = "Main Category saved successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["RoomDinningMainCategory"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["RoomDinningMainCategory"] = ex.Message;
            }
            return RedirectToAction("RoomDinningMainCategory");
        }

        public ActionResult DeleteRoomDinningMainCategory(string id)
        {
            try
            {
                Admin obj = new Admin();
                obj.PK_MainCategoryID = id;
                obj.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = obj.DeleteRoomDinningMainCategory();

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["RoomDinningMainCategory"] = "Main Category deleted successfully";
                    }
                    else
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["RoomDinningMainCategory"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["RoomDinningMainCategory"] = ex.Message;
            }
            return RedirectToAction("RoomDinningMainCategory");
        }

        #endregion



        #region RoomDinningCategoryItems
        public ActionResult RoomDinningCategoryItems(string CategoryID)
        {
            Admin model = new Admin();
            if (CategoryID != null)
            {
                model.PK_CategoryItemsID = CategoryID;
                List<Admin> lst = new List<Admin>();

                DataSet ds = model.GettingRoomDinningCategoryItems();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    //  model.DiscountID = DiscountID;

                    model.PK_CategoryItemsID = ds.Tables[0].Rows[0]["PK_CategoryItemsID"].ToString();
                    model.FK_MainCategoryID = ds.Tables[0].Rows[0]["FK_MainCategoryID"].ToString();
                    model.PK_MainCategoryID = ds.Tables[0].Rows[0]["FK_MainCategoryID"].ToString();
                    model.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                    model.Price = ds.Tables[0].Rows[0]["Price"].ToString();


                    model.lstRoomDinningCategoryItems = lst;

                }
            }

            #region RoomDinningMainCategory
            Admin obj1 = new Admin();
            int count = 0;
            List<SelectListItem> ddlMainCategory = new List<SelectListItem>();
            DataSet ds1 = obj1.GettingRoomDinningMainCategory();
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


            return View(model);

        }

        public ActionResult RoomDinningCategoryItemsList(Admin model)
        {
            List<Admin> lst = new List<Admin>();

            // model.SiteID = model.SiteID == "0" ? null : model.SiteID;

            DataSet ds = model.GettingRoomDinningCategoryItems();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_CategoryItemsID = r["PK_CategoryItemsID"].ToString();
                    obj.FK_MainCategoryID = r["FK_MainCategoryID"].ToString();
                    obj.MainCategoryName = r["MainCategoryName"].ToString();
                    obj.ItemName = r["ItemName"].ToString();
                    obj.Price = r["Price"].ToString();


                    lst.Add(obj);
                }
                model.lstRoomDinningCategoryItems = lst;
            }

            return View(model);
        }


        [HttpPost]
        [ActionName("RoomDinningCategoryItems")]
        [OnAction(ButtonName = "btnSave")]
        public ActionResult SaveRoomDinningCategoryItems(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();

                DataSet ds = model.SaveRoomDinningCategoryItems();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["RoomDinningCategory"] = " Category added successfully !";
                    }
                    else
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["RoomDinningCategory"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["RoomDinningCategory"] = ex.Message;
            }
            return RedirectToAction("RoomDinningCategoryItems");
        }
        [HttpPost]
        [ActionName("RoomDinningCategoryItems")]
        [OnAction(ButtonName = "btnUpdate")]
        public ActionResult UpdateRoomDinningCategoryItems(Admin obj)
        {

            try
            {

                obj.AddedBy = Session["Pk_AdminId"].ToString();
                //obj.dtProductQuantity = dtst;

                DataSet ds = obj.UpdateRoomDinningCategoryItems();

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["RoomDinningCategory"] = " Category Updated successfully !";
                    }
                    else
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["RoomDinningCategory"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["RoomDinningCategory"] = ex.Message;
            }
            return RedirectToAction("RoomDinningCategoryItems");
        }



        public ActionResult DeleteRoomDinningCategoryItems(string id)
        {
            try
            {
                Admin obj = new Admin();
                obj.PK_CategoryItemsID = id;
                obj.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = obj.DeleteRoomDinningCategoryItems();

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["RoomDinningCategory"] = "Category deleted successfully";
                    }
                    else
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["RoomDinningCategory"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["RoomDinningCategory"] = ex.Message;
            }
            return RedirectToAction("RoomDinningCategoryItemsList");
        }
        #endregion


        #region Menu
        public ActionResult Menu(string PK_MenuID)
        {
            List<Admin> list = new List<Admin>();
            Admin model = new Admin();
            DataSet ds = model.GettingMenu();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_MenuID = r["PK_MenuID"].ToString();
                    obj.MenuName = r["MenuName"].ToString();
                    list.Add(obj);
                }

                model.lstMenu = list;
            }

            return View(model);
        }

        [HttpPost]
        [OnAction(ButtonName = "saveBtn")]
        [ActionName("Menu")]
        public ActionResult SaveMenu(Admin model)
        {

            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.SaveMenu();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["Menu"] = "Main Category saved successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["Menu"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["Menu"] = ex.Message;
            }
            return RedirectToAction("Menu");
        }

        public ActionResult DeleteMenu(string id)
        {
            try
            {
                Admin obj = new Admin();
                obj.PK_MenuID = id;
                obj.AddedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = obj.DeleteMenu();

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["Menu"] = "Main Category deleted successfully";
                    }
                    else
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["Menu"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["Menu"] = ex.Message;
            }
            return RedirectToAction("Menu");
        }

        #endregion
    }
}