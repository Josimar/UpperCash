using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace UpperCash.Tools
{
    public static class ListaDinamicaExtension
    {
        public static MvcHtmlString ListaDinamica(this HtmlHelper html, string id, IList lista)
        {
            string strLink = String.Format("<ul data-role=\"listview\" data-filter=\"true\" data-filter-placeholder=\"Buscar...\" data-inset=\"true\">");

            PropertyInfo[] properties = lista.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.Name == "Categoria")
                {
                    foreach (Entities.Categoria categoria in lista)
                    {
                        strLink += ("<li><a href=\"../Categoria/Editar/"+ categoria.Id.ToString() +"\">" + categoria.Nome.ToString() + "</a></li>");
                    }
                }
            }
            strLink += ("</ul>");
            return new MvcHtmlString(strLink);
        }
    }
}