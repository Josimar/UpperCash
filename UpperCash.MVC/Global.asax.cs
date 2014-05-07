using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;
using UpperCash.MVC.Controllers;

namespace UpperCash.MVC {
	public class MvcApplication : System.Web.HttpApplication {
		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		protected void Application_PostAuthenticateRequest(Object sender, EventArgs e) {
			HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authCookie != null) {

				FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

				var serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
				var newUser = new CustomPrincipal(authTicket.Name);
				newUser.UserId = serializeModel.UserId;
				newUser.FirstName = serializeModel.FirstName;
				newUser.LastName = serializeModel.LastName;
				newUser.Roles = serializeModel.Roles;

				HttpContext.Current.User = newUser;
			}

		}
	}
}
