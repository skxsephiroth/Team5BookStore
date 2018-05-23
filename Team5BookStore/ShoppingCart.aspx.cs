﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Team5BookStore.Models;

namespace Team5BookStore
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        int maxValue;
        string username = "LeTawn55";

        void Page_PreInit(Object sender, EventArgs e)
        {
            //MasterPage.Picker(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Label1.Text = getStockLevel("9780060555665").ToString();
            BindGrid();

            //RangeValidator1.MaximumValue = Convert.ToString(maxValue);
            TotalAmountLabel.Text = string.Format("{0:C}", TotalPrice(CartItemModel.GetCartItems(username)));
         }

        private void BindGrid()
        {
            List<CartItem> cartItemList = CartItemModel.GetCartItems(username);
            GridView1.DataSource = cartItemList;
            GridView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Label1.Text = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]).ToString();
            int cartItemId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            CartItemModel.RemoveFromCart(cartItemId);
            BindGrid();
            TotalAmountLabel.Text = string.Format("{0:C}", TotalPrice(CartItemModel.GetCartItems(username)));

        }

        protected decimal? TotalPrice(List<CartItem> cartItemList)
        {
            decimal? sum = 0;
            foreach(CartItem ci in cartItemList)
            {
                sum += ci.TotalPrice; 
            }
            return sum;
        }

        protected void CheckOutButton_Click(object sender, EventArgs e)
        {
            //CartModel.CheckOutCart(username);
            //Response.Redirect("~/Receipt.aspx");
        }

        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox textBox = row.FindControl("TextBox1") as TextBox;
                    int newQuantity = Convert.ToInt32(textBox.Text);
                    int cartItemId = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Values[0]);
                    CartItemModel.UpdateCartItemQuantity(cartItemId, newQuantity);
                }
            }
            BindGrid();
            TotalAmountLabel.Text = string.Format("{0:C}", TotalPrice(CartItemModel.GetCartItems(username)));

        }

        protected int getStockLevel(string ISBN)
        {
            using (BookStoreEntities m = new BookStoreEntities())
            {
                Book b = m.Books.Where(x => x.ISBN == ISBN).First();
                return b.StockLevel;
            }

        }

        //protected string quantityValidator(string ISBN)
        //{
            
        //    if (quantity <= 0)
        //    {
        //        return "Quantity cannot be less than zero";
        //    }
        //    else if(quantity>= getStockLevel(ISBN))
        //    {
        //        return "Maximum Quantity at " + getStockLevel(ISBN).ToString();
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        protected string displayPrice(decimal Price, decimal FinalPrice)
        {
            return (Price != FinalPrice) ? string.Format("{0:C}", FinalPrice) : "";
        }

        protected bool isStrikethrough(decimal Price, decimal FinalPrice)
        {
            return (Price == FinalPrice) ? false : true;
        }
    }
}