using System;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using UpperCash.Entities;
using UpperCash.MVC.Models;

namespace UpperCash.MVC.Controllers {
	public class UsuarioController : BaseController {
		private string _connString = ConfigurationManager.ConnectionStrings["SQLConnection"].ToString();

		public ActionResult Index() {
			return View();
		}

		[HttpPost]
		public ActionResult Login(LoginModel model, string returnUrl = ""){
			if (ModelState.IsValid){
				var usuarioBo = new Business.Usuario(_connString);

				var user = usuarioBo.AutenticaUsuario(model.Username, model.Password);
				if (user != null){
					var perfil = new[] { user.Perfil.Nome };

					var serializeModel = new CustomPrincipalSerializeModel();
					serializeModel.UserId = user.ID;
					serializeModel.FirstName = user.Nome;
					serializeModel.LastName = user.Email;
					serializeModel.Roles = perfil;

					string userData = JsonConvert.SerializeObject(serializeModel);
					var authTicket = new FormsAuthenticationTicket(1, user.Login, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);

					string encTicket = FormsAuthentication.Encrypt(authTicket);

					var ucCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
					Response.Cookies.Add(ucCookie);

					if (perfil.Contains("Admin")){
						return RedirectToAction("Index", "Admin");
					}
					if (perfil.Contains("User")){
						return RedirectToAction("Index", "Usuario");
					}
					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError("Error", "Usuário ou senha incorreto.");
			}
			return View(model);
		}

		[AllowAnonymous]
		public ActionResult LogOut() {
			FormsAuthentication.SignOut();
			return RedirectToAction("Login", "Usuario", null);
		}

		public ActionResult Login(){
			return View();
		}

		public ActionResult Lista(){
			var usuarioBo = new Business.Usuario(_connString);

			var lista = usuarioBo.ListaUsuario();

			var model = new UsuarioModel{ListaUsuario = lista};

			return View(model);
		}

		public ActionResult Novo(){
			ListaCombo(new UsuarioVO());

			return View();
		}

		[HttpPost]
		public ActionResult Novo(Usuario usuario) {
			if (ModelState.IsValid) {
				var usuarioBo = new Business.Usuario(_connString);
				usuarioBo.SalvaUsuario(usuario);

				return RedirectToAction("Lista");
			}
			return View(usuario);
		}

		public ActionResult Editar(int id) {
			var usuarioBo = new Business.Usuario(_connString);

			var lista = usuarioBo.ListaUsuario(new Usuario(id));

			var usuario = lista[0];

			ListaCombo(usuario);

			return View(usuario);
		}

		private void ListaCombo(UsuarioVO usuario){
			var perfilBo = new Business.Perfil(_connString);
			var lstPerfil = perfilBo.ListaPerfil();

			ViewBag.PerfilId = new SelectList(lstPerfil, "Id", "Nome", usuario.PerfilId);
		}

		[HttpPost]
		public ActionResult Editar(Usuario usuario) {
			if (ModelState.IsValid) {
				var usuarioBo = new Business.Usuario(_connString);
				usuarioBo.SalvaUsuario(usuario);

				return RedirectToAction("Lista");
			}
			return View(usuario);
		}
	}
}