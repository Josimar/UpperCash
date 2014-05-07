using System;
using UpperCash.Utility;

namespace UpperCash.Entities {
	[Serializable]
	public class Controle {
		/// <summary>
		/// ID chave da tabela Tipo
		/// </summary>
		[PrimaryKey, NotNull]
		public int Id { get; set; }

		/// <summary>
		/// Título do registro de controle
		/// </summary>
		public string Titulo { get; set; }

		/// <summary>
		/// Descrição do controle
		/// </summary>
		public string Descricao { get; set; }

		/// <summary>
		/// Observação para controle
		/// </summary>
		public string Observacao { get; set; }

		/// <summary>
		/// Valor 
		/// </summary>
		public double Valor { get; set; }

		/// <summary>
		/// Data que a transação ocorreu
		/// </summary>
		public string Data { get; set; }

		/// <summary>
		/// Data de agendamento de pagamento
		/// </summary>
		public string Dataagendada { get; set; }

		/// <summary>
		/// Data de pagamento
		/// </summary>
		public string Datapagamento { get; set; }

		/// <summary>
		/// ID da categoria
		/// </summary>
		public int Idcategoria { get; set; }

		/// <summary>
		/// ID do tipo
		/// </summary>
		public int Idtipo { get; set; }

		/// <summary>
		/// ID do pagamento
		/// </summary>
		public int Idpagamento { get; set; }

		/// <summary>
		/// ID do usuário
		/// </summary>
		public int Idusuario { get; set; }

		public Controle(){}

		public Controle(int id){
			Id = id;
		}

		public Controle(int id, string titulo, string descricao, string observacao, double valor, string data, string dataagendada, string datapagamento, 
			              int idcategoria, int idtipo, int idpagamento, int idusuario){
			Id = id;
			Titulo = titulo;
			Descricao = descricao;
			Observacao = observacao;
			Valor = valor;
			Data = data;
			Dataagendada = dataagendada;
			Datapagamento = datapagamento;
			Idcategoria = idcategoria;
			Idtipo = idtipo;
			Idpagamento = idpagamento;
			Idusuario = idusuario;
		}
	}
}
