using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BonfirePizza.Models
{
    public class CreateOrder
    {
        public string amount { get; set; }
        public string Pk_UserId { get; set; }
        public string Type { get; set; }
        public string TransactionType { get; set; }
        public string OrderId { get; set; }

        public DataSet SaveOrderDetails()
        {
            SqlParameter[] para = {

                                      new SqlParameter("@amount", amount),
                                      new SqlParameter("@Pk_UserId", Pk_UserId),
                                      new SqlParameter("@Type", Type),
                                      new SqlParameter("@TransactionType", TransactionType),
                                      new SqlParameter("@OrderId", OrderId),

                                  };
            DataSet ds = Connection.ExecuteQuery("SaveOrderDetails", para);
            return ds;
        }


    }
    public class CreateOrderResponse
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class Home
    {
        public string TotalAmount { get; set; }
        public string OrderNo { get; set; }
        

        public string PaymentMode { get; set; }
        public string PaymentID { get; set; }
        public string FinalAmount { get; set; }
        public string PK_MenuID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public string alreadycontain { get; set; }
        public string PreferableTime { get; set; }
        public string FK_MainCategoryID { get; set; }
        public string ContainOrders { get; set; }
        public string PK_CategoryItemsID { get; set; }
        public string ItemName { get; set; }
        public string Price { get; set; }
        public string PK_MainCategoryID { get; set; }
        public string MainCategoryName { get; set; }
        public List<Home> lstMainCategory { get; set; }
        public List<Home> lstCategoryItems { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string LoginId { get; set; }
        public string UserType { get; set; }
        public string Pk_AdminId { get; set; }
        public string Null { get; set; }
        public string RoomNo { get; set; }
        public string Orders { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public string Url { get; set; }
        public List<Home> lstMenu { get; set; }
        public List<Home> lstsubmenu { get; set; }

        public static Home GetMenus(string Pk_AdminId, string UserType)
        {
            Home model = new Home();
            List<Home> lstmenu = new List<Home>();
            List<Home> lstsubmenu = new List<Home>();

            model.Pk_AdminId = Pk_AdminId;
            model.UserType = UserType;
            DataSet dsHeader = model.loadHeaderMenu();
            if (dsHeader != null && dsHeader.Tables.Count > 0)
            {
                if (dsHeader.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dsHeader.Tables[0].Rows)
                    {
                        Home obj = new Home();

                        obj.MenuId = r["PK_FormTypeId"].ToString();
                        obj.MenuName = r["FormType"].ToString();
                        obj.Url = r["Url"].ToString();
                        lstmenu.Add(obj);
                    }

                    model.lstMenu = lstmenu;
                    foreach (DataRow r in dsHeader.Tables[1].Rows)
                    {
                        Home obj = new Home();
                        obj.Url = r["Url"].ToString();
                        obj.MenuId = r["FK_FormTypeId"].ToString();
                        obj.SubMenuId = r["PK_FormId"].ToString();
                        obj.SubMenuName = r["FormName"].ToString();
                        lstsubmenu.Add(obj);
                    }

                    model.lstsubmenu = lstsubmenu;

                }


            }
            return model;

        }

        public DataSet loadHeaderMenu()
        {
            SqlParameter[] para = {
                                new SqlParameter("@PK_AdminId", Pk_AdminId),
                                 new SqlParameter("@UserType", UserType)
            };

            DataSet ds = Connection.ExecuteQuery("GetMenuUserWise", para);
            return ds;
        }

        public DataSet Login()
        {
            SqlParameter[] para ={new SqlParameter ("@LoginId",LoginId),
                                new SqlParameter("@Password",Password)};
            DataSet ds = Connection.ExecuteQuery("ValidateAdminLogin", para);
            return ds;
        }

        public DataSet GettingMainCategory()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@PK_MainCategoryID", PK_MainCategoryID),
                                     new SqlParameter("@MainCategoryName", MainCategoryName),

                                      };
            DataSet ds = Connection.ExecuteQuery("MainCategoryList");
            return ds;
        }

        public DataSet GettingCategoryItems()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@PK_CategoryItemsID", PK_CategoryItemsID),
                                     new SqlParameter("@FK_MainCategoryID", FK_MainCategoryID),
                                        new SqlParameter("@ItemName", ItemName),
                                     new SqlParameter("@Price", Price),

                                      };
            DataSet ds = Connection.ExecuteQuery("CategoryItemsList", para);
            return ds;
        }


        #region RoomDinningMainCategory
        public DataSet GettingRoomDinningMainCategory()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@PK_MainCategoryID", PK_MainCategoryID),
                                     new SqlParameter("@MainCategoryName", MainCategoryName),

                                      };
            DataSet ds = Connection.ExecuteQuery("RoomDinningMainCategoryList");
            return ds;
        }


        public DataSet GettingRoomDinningCategoryItems()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@PK_CategoryItemsID", PK_CategoryItemsID),
                                     new SqlParameter("@FK_MainCategoryID", FK_MainCategoryID),
                                        new SqlParameter("@ItemName", ItemName),
                                     new SqlParameter("@Price", Price),

                                      };
            DataSet ds = Connection.ExecuteQuery("RoomDinningCategoryItemsList", para);
            return ds;
        }



        #endregion

        public DataSet GettingMenu()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@PK_MenuID", PK_MenuID),
                                     new SqlParameter("@MenuName", MenuName),

                                      };
            DataSet ds = Connection.ExecuteQuery("MenuList");
            return ds;
        }

        public DataSet PlaceCustomerOrderOnline()
        {
            SqlParameter[] para = {
                 new SqlParameter("@Mobile", MobileNo),
             new SqlParameter("@PaymentMode", PaymentMode),
            new SqlParameter("@transactionNo", PaymentID),
            new SqlParameter("@FinalAmount", FinalAmount)
            };

            DataSet ds = Connection.ExecuteQuery("PlaceCustomerOrderMobile", para);
            return ds;
        }
    }
}