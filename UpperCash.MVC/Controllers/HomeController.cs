using System.Web.Mvc;

namespace UpperCash.MVC.Controllers {
	public class HomeController : Controller {
		
		public ActionResult Index() {
			return View();
		}

		public ActionResult Configuracao(){
			return View();
		}

		public ActionResult AccessDenied(){
			return View();
		}

	}
}