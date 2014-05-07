using System;

namespace UpperCash.Entities {
	[Serializable]
	public class UsuarioConta {
		/// <summary>
		/// IDUsuario chave da tabela UsuarioConta com referência na tabela usuario
		/// </summary>
		public int IdUsuario { get; set; }

		/// <summary>
		/// IDConta chave da tabela UsuarioConta com referência na tabela conta
		/// </summary>
		public int IdConta { get; set; }
		
		public UsuarioConta(){}

		public UsuarioConta(int idusuario, int idconta){
			IdUsuario = idusuario;
			IdConta = idconta;
		}
	}
}
