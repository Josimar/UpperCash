using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpperCash.MVC.Models;

namespace UpperCash.MVC.Controllers
{
    public class CategoriaController : Controller
    {
        private string _connString = ConfigurationManager.ConnectionStrings["SQLConnection"].ToString();

        public ActionResult Index()
        {
			return View();
		}
        public ActionResult Novo()
        {
            CategoriaModel model = new CategoriaModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Novo(Entities.Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var categoriaBo = new Business.Categoria(_connString);
                try
                {
                    categoriaBo.SalvaCategoria(categoria);
                    return View("Index");
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return View(categoria);
        }
        public ActionResult Editar(int ID)
        {
            CategoriaModel model = new CategoriaModel();
            Business.Categoria categoriaBo = new Business.Categoria(_connString);
            Entities.Categoria categoria = categoriaBo.LoadByPk(ID);

            //Fazer uma coisa mais bonita aqui..
            model.id = categoria.Id;
            model.idpai = categoria.IdPai;
            model.nome = categoria.Nome;

            return View(model);
        }
        [HttpPost]
        public ActionResult Editar(Entities.Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var categoriaBo = new Business.Categoria(_connString);
                categoriaBo.SalvaCategoria(categoria);
                return View(categoria);
            }
            return View(categoria);
        }

        public ActionResult Excluir(int ID)
        {
            if (ModelState.IsValid)
            {
                var categoriaBo = new Business.Categoria(_connString);
                Entities.Categoria paraExcluir = categoriaBo.LoadByPk(ID);
                categoriaBo.ExcluiCategoria(paraExcluir);
            }
            return View("Index");
        }

        public ActionResult ListaCateg()
        {
            var categoriaBo = new Business.Categoria(_connString);
            var lista = categoriaBo.ListaCategoria();
            CategoriaModel model = new CategoriaModel { ListaCategoria = lista };
            return View(model);
        }
	}
}