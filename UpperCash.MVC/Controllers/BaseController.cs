using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UpperCash.MVC.Controllers {
	public class BaseController : Controller {
		protected virtual new CustomPrincipal User {
			get { return HttpContext.User as CustomPrincipal; }
		}
	}

	public class CustomPrincipal : IPrincipal {
		public IIdentity Identity { get; private set; }
		public bool IsInRole(string role) {
			if (Roles.Any(role.Contains)) {
				return true;
			}
			return false;
		}

		public CustomPrincipal(string username) {
			Identity = new GenericIdentity(username);
		}

		public int UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string[] Roles { get; set; }
	}

	public class CustomPrincipalSerializeModel {
		public int UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string[] Roles { get; set; }
	}

	public class CustomAuthorizeAttribute : AuthorizeAttribute {
		protected virtual CustomPrincipal CurrentUser {
			get { return HttpContext.Current.User as CustomPrincipal; }
		}

		public override void OnAuthorization(AuthorizationContext filterContext) {
			if (filterContext.HttpContext.Request.IsAuthenticated){
				var authorizedUsers = "2,3";
				var authorizedRoles = "Admin";

				Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
				Roles = String.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;

				if (!String.IsNullOrEmpty(Roles)){
					if (!CurrentUser.IsInRole(Roles)){
						filterContext.Result = new RedirectToRouteResult(new
							RouteValueDictionary(new{controller = "Home", action = "AccessDenied"}));

						// base.OnAuthorization(filterContext); //returns to login url
					}
				}

				if (!String.IsNullOrEmpty(Users)){
					if (!Users.Contains(CurrentUser.UserId.ToString())){
						filterContext.Result =
							new RedirectToRouteResult(new RouteValueDictionary(new{controller = "Home", action = "AccessDenied"}));

						// base.OnAuthorization(filterContext); //returns to login url
					}
				}
			}
			else{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
			}

		}
	}
}