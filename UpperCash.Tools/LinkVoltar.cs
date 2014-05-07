using System;
using System.Web.Mvc;

namespace UpperCash.Tools
{
    public static class LinkVoltarExtensions
    {
        public static MvcHtmlString LinkVoltar(this HtmlHelper html, string idLink, string textoLink = "Voltar")
        {
            string strLink = String.Format("<a id=\"{0}\" class=\"ui-btn ui-icon-back ui-btn-icon-left ui-shadow ui-corner-all\" href=\"javascript:history.go(-1);\">{1}</a>", idLink, textoLink);
			return new MvcHtmlString(strLink);
		}
	}
}
