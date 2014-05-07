using System;
using System.Collections.Generic;

namespace UpperCash.MVC.Models {
	[Serializable]
	public class UsuarioModel {

		public IList<Entities.UsuarioVO> ListaUsuario { get; set; }

	}
}