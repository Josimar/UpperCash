using System;
using System.ComponentModel.DataAnnotations;

namespace UpperCash.Entities {
	[Serializable]
	public class Perfil {

		public int Id { get; set; }
		[Required]
		public string Nome { get; set; }
		public string Descricao { get; set; }
		public int Flags { get; set; }


		public Perfil() {

		}

		public Perfil(int id) {
			Id = id;
		}

		public Perfil(int id, string nome, string descricao, int flags){
			Id = id;
			Nome = nome;
			Descricao = descricao;
			Flags = flags;
		}
	}
}
