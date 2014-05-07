using System;

namespace UpperCash.Entities {
	[Serializable]
	public class UsuarioPagamento {
		/// <summary>
		/// IDUsuario chave da tabela UsuarioPagamento com referência na tabela usuario
		/// </summary>
		public int IdUsuario { get; set; }

		/// <summary>
		/// IDPagamento chave da tabela UsuarioPagamento com referência na tabela pagamento
		/// </summary>
		public int IdPagamento { get; set; }

		public UsuarioPagamento(){}

		public UsuarioPagamento(int idusuario, int idPagamento){
			IdUsuario = idusuario;
			IdPagamento = idPagamento;
		}
	}
}
