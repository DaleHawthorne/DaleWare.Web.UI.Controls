using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace DaleWare.Web.UI.Controls
{
    /// <summary>
    /// This is a utility class of extension methods which can be applied to 
    /// a list box. It can be used outside the controls assembly for convenience
    /// simply by including the namespace.
    /// </summary>
    public static class ListControlUtility
    {
        /// <summary>
        /// This method returns a list of selected listitems from the listbox.
        /// </summary>
        /// <param name="lc">The list control.</param>
        /// <returns></returns>
        public static List<ListItem> SelectedItems(this ListControl lc)
        {
            List<ListItem> selectedItems =
                    lc.Items.Cast<ListItem>().Where(item => item.Selected == true).ToList();
            return selectedItems;
        }

        #region Sort Implementation
        /// <summary>
        /// This method sorts the items in a listbox.
        /// </summary>
        /// <param name="items">The list control.</param>
        /// <param name="Descending">Whether to sort the list box descending. </param>
        public static void SortListItems(this ListItemCollection items, bool Descending)
        {
            System.Collections.Generic.List<ListItem> list = new System.Collections.Generic.List<ListItem>();
            foreach (ListItem i in items)
            {
                list.Add(i);
            }

            if (Descending)
            {
                IEnumerable<ListItem> itemEnum =
                    from item in list
                    orderby item.Text descending
                    select item;
                items.Clear();
                items.AddRange(itemEnum.ToArray());
                // anonymous delegate list.Sort(delegate(ListItem x, ListItem y) { return y.Text.CompareTo(x.Text); });
            }
            else
            {
                IEnumerable<ListItem> itemEnum =
                    from item in list
                    orderby item.Text ascending
                    select item;
                items.Clear();
                items.AddRange(itemEnum.ToArray());

                //list.Sort(delegate(ListItem x, ListItem y) { return x.Text.CompareTo(y.Text); });
            }
        }
        #endregion
    }
}
