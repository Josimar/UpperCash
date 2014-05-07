using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UpperCash.MVC.Controllers {
	public class ControleController : Controller {
		public ActionResult Novo() {
			return View();
		}

		public ActionResult Sumario() {
			return View();
		}

		public ActionResult Relatorio() {
			return View();
		}
	}
}