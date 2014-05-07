using System;

namespace UpperCash.Entities {
	[Serializable]
	public class UsuarioCategoria {
		/// <summary>
		/// IDUsuario chave da tabela UsuarioCategoria com referência na tabela usuario
		/// </summary>
		public int IdUsuario { get; set; }

		/// <summary>
		/// IDCategoria chave da tabela UsuarioCategoria com referência na tabela categoria
		/// </summary>
		public int IdCategoria { get; set; }

		public UsuarioCategoria(){}

		public UsuarioCategoria(int idusuario, int idcategoria){
			IdUsuario = idusuario;
			IdCategoria = idcategoria;
		}
	}
}
