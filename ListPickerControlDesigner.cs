using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace DaleWare.Web.UI.Controls
{
    /// <summary>
    /// Summary description for ListPickerControlDesigner
    /// </summary>
    public class ListPickerControlDesigner : ControlDesigner
    {

        public override string GetDesignTimeHtml()
        {

            StringBuilder sb = new StringBuilder();
            ListPickerControl lpc = Component as ListPickerControl;
            sb.Append("<table");
            if(lpc.Width !=null)
            {
                sb.Append(" width=\"");
                sb.Append(ControlDesignerUtility.UnitString(lpc.Width));
                sb.Append("\"");
            }
            sb.Append("><tr><td style=\"width:40%;text-align:left\">\r\n");
            if (string.IsNullOrEmpty(lpc.AvailableChoicesLabelText) == false)
            {
                sb.Append(lpc.AvailableChoicesLabelText);
                sb.Append("<br />");
            }
            sb.Append("<select style=\"width:");
            sb.Append(lpc.ListBoxStyle.Width.ToString());
            if (lpc.NumberOfListBoxRows > 0)
            {
                sb.AppendFormat("\" size=\"{0}", lpc.NumberOfListBoxRows);
            }

            sb.Append("\">\r\n");
            if (lpc.AvailableChoices == null || lpc.AvailableChoices.Count == 0)
            {

                sb.Append("<li>&nbsp;</li>");

            }

            else
            {
                                
                foreach (ListItem item in lpc.AvailableChoices)
                {
                    sb.Append("<li");
                    if (item.Value != null)
                    {
                        sb.Append(" value=\"");
                        sb.Append(item.Value);
                        sb.Append("\"");
                    }
                    sb.Append(">");
                    sb.Append(item.Text);
                    sb.Append("</li>\r\n");
                }

            }
            sb.Append("</select>\r\n</td>");
            sb.Append("<td style=\"width:20%;text-align:center\">");
            sb.Append("<input type=\"button\" value=\">>\" style=\"width:3em\"><br /><input type=\"button\" value=\">&nbsp;\" style=\"width:3em\"><br /><input type=\"button\" value=\"<&nbsp;\" style=\"width:3em\"><br /><input type=\"button\" value=\"<<\" style=\"width:3em\"><br />");
            sb.Append("</td>\r\n<td style=\"width:40%;text-align:left\">\r\n");
            if (string.IsNullOrEmpty(lpc.SelectedChoicesLabelText) == false)
            {
                sb.Append(lpc.SelectedChoicesLabelText);
                sb.Append("<br />");
            }
            sb.Append("<select style=\"width:");
            sb.Append(lpc.ListBoxStyle.Width.ToString().Trim());
            
            if (lpc.NumberOfListBoxRows > 0)
            {
                sb.AppendFormat("\" size=\"{0}", lpc.NumberOfListBoxRows);
            }

            sb.Append("\">\r\n");
            if (lpc.SelectedItems == null || lpc.SelectedItems.Count == 0)
            {
                sb.Append("<li>&nbsp;</li>\r\n");
            }

            else
            {

                foreach (ListItem item in lpc.SelectedItems)
                {
                    sb.Append("<li");
                    if (item.Value != null)
                    {
                        sb.Append(" value=\"");
                        sb.Append(item.Value);
                        sb.Append("\"");
                    }
                    sb.Append(">");
                    sb.Append(item.Text);
                    sb.Append("</li>\r\n");
                }              
            }
            sb.Append("</select>\r\n</td></tr></table>");
           System.IO.File.WriteAllText("C:\\Users\\Dale\\Documents\\Output.txt",sb.ToString());
            return sb.ToString();
        }
    }
}