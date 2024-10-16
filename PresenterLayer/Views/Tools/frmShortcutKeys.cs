using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirecLayer
{
    public partial class frmShortcutKeys : MetroFramework.Forms.MetroForm
    {

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {

            //if (keyData == (Keys.Alt | Keys.S))
            //{
            //    frmInventoryTransferRequest. .Focus();
            //}

            //else if (keyData == (Keys.Alt | Keys.B))     //Brands
            //{
            //    LoadSelectionOfBrands();
            //}

            //else if (keyData == (Keys.Alt | Keys.D))     //Departments
            //{
            //    LoadSelectionOfDepts();
            //}

            //else if (keyData == (Keys.Alt | Keys.P))     //Sub Departments
            //{
            //    LoadSelectionOfSubDepts();
            //}

            //else if (keyData == (Keys.Alt | Keys.C))     //Categories
            //{
            //    LoadSelectionOfCategories();
            //}

            //else if (keyData == (Keys.Alt | Keys.U))     //Sub Categories
            //{
            //    LoadSelectionOfSubCategories();
            //}

            //else if (keyData == (Keys.Alt | Keys.Z))     //Sizes
            //{
            //    LoadSelectionOfSizes();
            //}

            //else if (keyData == (Keys.Alt | Keys.I))     //Sub Sizes
            //{
            //    LoadSelectionOfSubSizes();
            //}

            //else if (keyData == (Keys.Alt | Keys.E))     //Colors
            //{
            //    LoadSelectionOfColors();
            //}

            //else if (keyData == (Keys.Alt | Keys.O))     //Sub Colors
            //{
            //    LoadSelectionOfSubColors();
            //}

            //else if (keyData == (Keys.Alt | Keys.Y))     //Styles
            //{
            //    LoadSelectionOfStyles();
            //}

            //else if (keyData == (Keys.Alt | Keys.D1))   //Focus on List of Items Table
            //{
            //    gvITR.Focus();
            //}

            //else if (keyData == (Keys.Alt | Keys.D2))   //Focus on List of Selected Items Table
            //{
            //    gvSelectedItem.Focus();
            //}

            //else if (keyData == Keys.Enter)
            //{
            //    if (gvITR.Focused == true)             //Transfer items from List to Selected Items Table
            //    {
            //        itric.GetSelectedItems(gvITR, gvSelectedItem);
            //        //gvSelectedItem.Sort(gvSelectedItem.Columns["SortCode"], ListSortDirection.Ascending);
            //    }
            //    else if (gvSelectedItem.Focused == true)        //Transfer items from Selected Items Table to the List
            //    {
            //        //itric.GetSelectedItems(gvSelectedItem, gvITR);
            //        GetBackSelectedItems(gvSelectedItem, gvITR);
            //    }
            //}

            //else if (keyData == (Keys.Alt | Keys.Q) && gvSelectedItem.Focused == true)
            //{
            //    if (gvSelectedItem.Rows.Count > 0)
            //    {
            //        int index = gvSelectedItem.CurrentRow.Index;
            //        gvSelectedItem.CurrentCell = gvSelectedItem[3, index];
            //        gvSelectedItem[3, index].Selected = true;
            //        gvSelectedItem.BeginEdit(true);
            //    }
            //    else
            //    {
            //        frmMain.NotiMsg("No items to set Quantity.", Color.Red);
            //    }
            //}

            //else if (keyData == (Keys.Alt | Keys.A))
            //{
            //    AddItems();
            //}

            //else if (keyData == Keys.Escape)
            //{
            //    Close();
            //}

            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
