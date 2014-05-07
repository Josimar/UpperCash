using System;
using UpperCash.Utility;

namespace UpperCash.Entities {
	[Serializable]
	public class Categoria {
		/// <summary>
		/// ID chave da tabela Categoria
		/// </summary>
		[PrimaryKey, NotNull]
		public int Id { get; set; }
		
		/// <summary>
		/// ID que mostra quem é o pai da categoria
		/// </summary>
		public int? IdPai { get; set; }

		/// <summary>
		/// Nome da categoria
		/// </summary>
		[NotNull,MaxStringLength(128)]
		public string Nome { get; set; }

		public Categoria(){}

		public Categoria(int id){
			Id = id;
		}

		public Categoria(int id, int idpai, string nome){
			Id = id;
			IdPai = idpai;
			Nome = nome;
		}
	}
}
