using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoolSideMenu.Models
{
    public class Admin
    {
        public List<Admin> lstMenu { get; set; }
        public List<SelectListItem> ddlMainCategory { get; set; }

        public string PK_MenuID { get; set; }
        public string MenuName { get; set; }
        public string FK_MainCategoryID { get; set; }
        public string PK_CategoryItemsID { get; set; }
        public string ItemName { get; set; }
        public string Price { get; set; }
        public string PK_MainCategoryID { get; set; }
        public string MainCategoryName { get; set; }
        public List<Admin> lstMainCategory { get; set; }
        public List<Admin> lstCategoryItems { get; set; }


        #region MainCategory
        public DataSet GettingMainCategory()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@PK_MainCategoryID", PK_MainCategoryID),
                                     new SqlParameter("@MainCategoryName", MainCategoryName),
                                     new SqlParameter("@PK_MenuID", PK_MenuID),

                                      };
            DataSet ds = Connection.ExecuteQuery("MainCategoryList",para);
            return ds;
        }


        public DataSet SaveMainCategory()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@MainCategoryName", MainCategoryName),
                                      new SqlParameter("@PK_MenuID", PK_MenuID),
                                  new SqlParameter("@AddedBy", AddedBy)};
            DataSet ds = Connection.ExecuteQuery("AddMainCategory", para);
            return ds;
        }

        public DataSet DeleteMainCategory()
        {
            SqlParameter[] para = {
                                       new SqlParameter("@PK_MainCategoryID", PK_MainCategoryID),
                                  new SqlParameter("@AddedBy", AddedBy),


                                  };
            DataSet ds = Connection.ExecuteQuery("DeleteMainCategory", para);
            return ds;
        }

        #endregion

        #region CategoryItems
        public DataSet GettingCategoryItems()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@PK_CategoryItemsID", PK_CategoryItemsID),
                                     new SqlParameter("@FK_MainCategoryID", FK_MainCategoryID),
                                        new SqlParameter("@ItemName", ItemName),
                                     new SqlParameter("@Price", Price),

                                      };
            DataSet ds = Connection.ExecuteQuery("CategoryItemsList",para);
            return ds;
        }

        public DataSet SaveCategoryItems()
        {
            SqlParameter[] para = { new SqlParameter("@ItemName", ItemName),
                                      new SqlParameter("@AddedBy", AddedBy),
                                      new SqlParameter("@FK_MenuID", PK_MenuID),
                                       new SqlParameter("@Price", Price),
                                       new SqlParameter("@FK_MainCategoryID", PK_MainCategoryID)

                                  };
            DataSet ds = Connection.ExecuteQuery("AddCategoryItems", para);
            return ds;
        }


        public DataSet UpdateCategoryItems()
        {
            SqlParameter[] para = { new SqlParameter("@PK_CategoryItemsID", PK_CategoryItemsID),
                                      new SqlParameter("@ItemName", ItemName),
                                      new SqlParameter("@FK_MenuID", PK_MenuID),
                                         new SqlParameter("@FK_MainCategoryID", PK_MainCategoryID),
                                  new SqlParameter("@UpdatedBy", AddedBy),
                                   new SqlParameter("@Price", Price)
            };
            DataSet ds = Connection.ExecuteQuery("UpdateCategoryItems", para);
            return ds;
        }


        public DataSet DeleteCategoryItems()
        {
            SqlParameter[] para = {
                                       new SqlParameter("@PK_CategoryItemsID", PK_CategoryItemsID),
                                  new SqlParameter("@AddedBy", AddedBy),


                                  };
            DataSet ds = Connection.ExecuteQuery("DeleteCategoryItems", para);
            return ds;
        }

        #endregion





        public string AddedBy { get; set; }
        public string OfferID { get; set; }

        public string OfferName { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string OfferStatus { get; set; }
        public List<Admin> List { get; set; }


        public List<Admin> lstRoomDinningMainCategory { get; set; }
        public List<Admin> lstRoomDinningCategoryItems { get; set; }

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


        public DataSet SaveRoomDinningMainCategory()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@MainCategoryName", MainCategoryName),
                                  new SqlParameter("@AddedBy", AddedBy)};
            DataSet ds = Connection.ExecuteQuery("AddRoomDinningMainCategory", para);
            return ds;
        }

        public DataSet DeleteRoomDinningMainCategory()
        {
            SqlParameter[] para = {
                                       new SqlParameter("@PK_MainCategoryID", PK_MainCategoryID),
                                  new SqlParameter("@AddedBy", AddedBy),


                                  };
            DataSet ds = Connection.ExecuteQuery("DeleteRoomDinningMainCategory", para);
            return ds;
        }

        #endregion



        #region RoomDinningCategoryItems
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

        public DataSet SaveRoomDinningCategoryItems()
        {
            SqlParameter[] para = { new SqlParameter("@ItemName", ItemName),
                                      new SqlParameter("@AddedBy", AddedBy),
                                       new SqlParameter("@Price", Price),
                                       new SqlParameter("@FK_MainCategoryID", PK_MainCategoryID)

                                  };
            DataSet ds = Connection.ExecuteQuery("AddRoomDinningCategoryItems", para);
            return ds;
        }


        public DataSet UpdateRoomDinningCategoryItems()
        {
            SqlParameter[] para = { new SqlParameter("@PK_CategoryItemsID", PK_CategoryItemsID),
                                      new SqlParameter("@ItemName", ItemName),
                                         new SqlParameter("@FK_MainCategoryID", PK_MainCategoryID),
                                  new SqlParameter("@UpdatedBy", AddedBy),
                                   new SqlParameter("@Price", Price)
            };
            DataSet ds = Connection.ExecuteQuery("UpdateRoomDinningCategoryItems", para);
            return ds;
        }


        public DataSet DeleteRoomDinningCategoryItems()
        {
            SqlParameter[] para = {
                                       new SqlParameter("@PK_CategoryItemsID", PK_CategoryItemsID),
                                  new SqlParameter("@AddedBy", AddedBy),


                                  };
            DataSet ds = Connection.ExecuteQuery("DeleteRoomDinningCategoryItems", para);
            return ds;
        }

        #endregion

        #region Menu
        public DataSet GettingMenu()
        {
            SqlParameter[] para = {
                                    new SqlParameter("@PK_MenuID", PK_MenuID),
                                     new SqlParameter("@MenuName", MenuName),

                                      };
            DataSet ds = Connection.ExecuteQuery("MenuList");
            return ds;
        }


        public DataSet SaveMenu()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@MenuName", MenuName),
                                  new SqlParameter("@AddedBy", AddedBy)};
            DataSet ds = Connection.ExecuteQuery("AddMenu", para);
            return ds;
        }

        public DataSet DeleteMenu()
        {
            SqlParameter[] para = {
                                       new SqlParameter("@PK_MenuID", PK_MenuID),
                                  new SqlParameter("@AddedBy", AddedBy),


                                  };
            DataSet ds = Connection.ExecuteQuery("DeleteMenu", para);
            return ds;
        }

        #endregion
    }
}