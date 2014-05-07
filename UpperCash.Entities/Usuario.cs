using System;
using UpperCash.Utility;
using System.ComponentModel.DataAnnotations;

namespace UpperCash.Entities {
	[Serializable]
	public class Usuario {
		/// <summary>
		/// ID do usuário 
		/// </summary>
		[PrimaryKey, NotNull]
		public int ID { get; set; }
		/// <summary>
		/// Nome do usuário
		/// </summary>
		[MaxStringLength(128)]
		public string Nome { get; set; }
		/// <summary>
		/// E-mail do usuário
		/// </summary>
	  [MaxStringLength(128)]
		public string Email { get; set; }
		/// <summary>
		/// Login do usuário
		/// </summary>
		[MaxStringLength(32)]
		public string Login { get; set; }
		/// <summary>
		/// Senha do usuário
		/// </summary>
		[MaxStringLength(128), DataType(DataType.Password)]
		public string Senha { get; set; }
		/// <summary>
		/// Indica se o usuário está ativo ou não
		/// </summary>
		public bool Ativo { get; set; }
		/// <summary>
		/// ID do perfil
		/// </summary>
		public int PerfilId { get; set; }

		public Usuario() {

		}

		public Usuario(int id) {
			ID = id;
		}

		public Usuario(string login) {
			Login = login;
		}

		public Usuario(string login, string senha) {
			Login = login;
			Senha = senha;
		}

		public Usuario(int id, string nome, string email, string login, string senha, bool ativo, int perfilid) {
			ID = id;
			Nome = nome;
			Email = email;
			Login = login;
			Senha = senha;
			Ativo = ativo;
			PerfilId = perfilid;
		}
	}

	[Serializable]
	public class UsuarioVO : Usuario{
		/// <summary>
		/// Propriedade <code>Perfil</code> para mostrar qual o perfil do usuário
		/// </summary>
		public virtual Perfil Perfil { get; set; }
	}
}
