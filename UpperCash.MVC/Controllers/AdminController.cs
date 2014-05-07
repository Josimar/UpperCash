using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpperCash.Utility;

namespace UpperCash.MVC.Controllers {
	[CustomAuthorize(Roles = "Admin")]
	public class AdminController : Controller {

		public ActionResult Index() {
			return View();
		}
	}
}