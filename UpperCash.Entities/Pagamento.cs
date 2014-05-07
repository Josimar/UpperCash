using System;
using UpperCash.Utility;

namespace UpperCash.Entities {
	[Serializable]
	public class Pagamento {
		/// <summary>
		/// ID chave da tabela Tipo
		/// </summary>
		[PrimaryKey, NotNull]
		public int Id { get; set; }

		/// <summary>
		/// Nome do pagamento
		/// </summary>
		[NotNull, MaxStringLength(128)]
		public string Nome { get; set; }

		/// <summary>
		/// Limite de valor da forma de pagamento
		/// </summary>
		public double Limite { get; set; }

		/// <summary>
		/// Data de vencimento
		/// </summary>
		public string Vencimento { get; set; }

		public Pagamento() { }

		public Pagamento(int id) {
			Id = id;
		}

		public Pagamento(int id, string nome, double limite, string vencimento){
			Id = id;
			Nome = nome;
			Limite = limite;
			Vencimento = vencimento;
		}

	}
}
