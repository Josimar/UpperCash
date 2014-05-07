using System;
using UpperCash.Utility;

namespace UpperCash.Entities {
	[Serializable]
	public class Conta {
		/// <summary>
		/// ID chave da tabela Conta
		/// </summary>
		[PrimaryKey, NotNull]
		public int Id { get; set; }

		/// <summary>
		/// Nome do Tipo
		/// </summary>
		[NotNull, MaxStringLength(128)]
		public string Nome { get; set; }

		/// <summary>
		/// Valor disponível na conta
		/// </summary>
		public double ValorDisponivel { get; set; }

		public Conta(){}

		public Conta(int id){
			Id = id;
		}

		public Conta(int id, double valordisponivel, string nome){
			Id = id;
			Nome = nome;
			ValorDisponivel = valordisponivel;
		}
	}
}
