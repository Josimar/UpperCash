using System;
using UpperCash.Utility;

namespace UpperCash.Entities {
	[Serializable]
	public class UsuarioTipo {
		/// <summary>
		/// IDUsuario chave da tabela UsuarioTipo com referência na tabela usuario
		/// </summary>
		[PrimaryKey, NotNull]
		public int IdUsuario { get; set; }

		/// <summary>
		/// IDTipo chave da tabela UsuarioTipo com referência na tabela tipo
		/// </summary>
		[PrimaryKey, NotNull]
		public int IdTipo { get; set; }

		public UsuarioTipo(){}

		public UsuarioTipo(int idusuario, int idtipo){
			IdUsuario = idusuario;
			IdTipo = idtipo;
		}
	}
}
