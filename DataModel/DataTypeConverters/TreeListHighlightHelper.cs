using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DevExpress.Web.ASPxTreeList;

namespace MvcBaseApp.Models
{
    public class TreeListHighlightHelper
    {
        public static string GetCellText(TreeListDataCellTemplateContainer container, string searchText)
        {
            string cell_text = container.Text;
            if (searchText.Length > cell_text.Length)
                return cell_text;
            if (searchText != "")
            {
                string cell_text_lower = cell_text.ToLower();
                string serchText_lower = searchText.ToLower();

                int start_pos = cell_text_lower.IndexOf(serchText_lower);
                if (start_pos >= 0)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(cell_text.Substring(0, start_pos));
                    builder.Append(string.Format("<span class='highlight'>{0}</span>",
                        cell_text.Substring(start_pos, searchText.Length)));
                    builder.Append(cell_text.Substring(start_pos + searchText.Length));
                    cell_text = builder.ToString();
                }
            }

            return cell_text;
        }

        public static void CheckNode(TreeListNode node, string searchText)
        {
            object node_value = node.GetValue("Name");
            if (node_value == null)
                return;
            if (node_value.ToString().ToLower().IndexOf(searchText.ToLower()) >= 0)
            {
                node.MakeVisible();
                node.Expanded = true;

            }

        }


    }
}