using System;
using System.Web;
using System.Web.Security;

namespace UpperCash.Net {
	public partial class Logoff : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			var sessionManager = new UCSessionManager(HttpContext.Current.Session);
			sessionManager.RemoveSessionData(UCSessionManager.SessionManagerData.VISITOR);

			UCCookies.EatCookie(HttpContext.Current.Request, HttpContext.Current.Response); // Limpa os cookies
			FormsAuthentication.SignOut();  //  Efetua o logout, desconectando o usuário.

			Response.Redirect("Login.aspx");
		}
	}
}