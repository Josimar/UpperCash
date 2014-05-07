using System;
using System.Collections.Generic;

namespace UpperCash.MVC.Models
{
    [Serializable]
    public class CategoriaModel
    {
        //lista
        public IList<Entities.Categoria> ListaCategoria { get; set; }

        //modelo
        public int id { get; set; }
        public int idpai { get; set; }
        public string nome { get; set; }

    }
}