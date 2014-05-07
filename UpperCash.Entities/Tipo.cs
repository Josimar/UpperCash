using System;
using UpperCash.Utility;

namespace UpperCash.Entities {
	[Serializable]
	public class Tipo {
		/// <summary>
		/// ID chave da tabela Tipo
		/// </summary>
		[PrimaryKey, NotNull]
		public int Id { get; set; }

		/// <summary>
		/// Nome do Tipo
		/// </summary>
		[NotNull, MaxStringLength(128)]
		public string Nome { get; set; }
		
		/// <summary>
		/// Indica se o valor é positivo ou negativo (receita, despesa) para cálculos automâticos
		/// </summary>
		public bool Positivo { get; set; }

		public Tipo(){}

		public Tipo(int id){
			Id = id;
		}

		public Tipo(int id, string nome, bool positivo){
			Id = id;
			Nome = nome;
			Positivo = positivo;
		}
	}
}
