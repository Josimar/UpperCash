using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UpperCash.Data;
using UpperCash.Entities;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class Usuario {

		private readonly string _connectionString = string.Empty;

		public Usuario(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.UsuarioVO> ListaUsuario() {
			return ListaUsuario(null);
		}

		public Entities.UsuarioVO AutenticaUsuario(string login, string senha) {
			var usuarios = ListaUsuario(new Entities.Usuario(login, senha));
			Entities.UsuarioVO usuario = usuarios.FirstOrDefault();

			if (usuario != null) {
				usuario = usuarios[0];
			}

			return usuario;
		}

		public Entities.Usuario LoadByPk(int kID) {
			var usuario = new Entities.Usuario{ID = kID};

			var usuarios = ListaUsuario(usuario);

			if (usuarios.Count == 0) return null;

			return ListaUsuario(usuario)[0];
		}

		public IList<Entities.UsuarioVO> ListaUsuario(Entities.Usuario usuario) {
			var lstUsuario = new List<Entities.UsuarioVO>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT U.id, U.nome, U.email, U.login, U.senha, U.ativo, P.Nome as Perfil ");
			sqlString.AppendLine("  FROM Usuario U ");
			sqlString.AppendLine("     LEFT JOIN Perfil P ON U.idperfil = P.id ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (usuario != null){
				if (usuario.ID > 0) {
					sqlString.AppendFormat(" AND U.id = '{0}' ", usuario.ID);
				}
				if (!string.IsNullOrEmpty(usuario.Nome)) {
					sqlString.AppendFormat(" AND U.nome = '{0}' ", usuario.Nome);
				}
				if (!string.IsNullOrEmpty(usuario.Email)) {
					sqlString.AppendFormat(" AND U.email = '{0}' ", usuario.Email);
				}
				if (!string.IsNullOrEmpty(usuario.Login)){
					sqlString.AppendFormat(" AND U.login = '{0}' ", usuario.Login);
				}
				if (!string.IsNullOrEmpty(usuario.Senha)) {
					sqlString.AppendFormat(" AND U.senha = '{0}' ", usuario.Senha);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxId = reader.GetOrdinal("id");
			int idxNome = reader.GetOrdinal("nome");
			int idxEmail = reader.GetOrdinal("email");
			int idxLogin = reader.GetOrdinal("login");
			int idxSenha = reader.GetOrdinal("senha");
			int idxAtivo = reader.GetOrdinal("ativo");
			int idxPerfil = reader.GetOrdinal("perfil");

			while (reader.Read()) {
				var _Usuario = new Entities.UsuarioVO();
				_Usuario.ID = Utilities.GetIntNullCheck(reader, idxId);
				_Usuario.Nome = Utilities.GetStringNullCheck(reader, idxNome);
				_Usuario.Email = Utilities.GetStringNullCheck(reader, idxEmail);
				_Usuario.Login = Utilities.GetStringNullCheck(reader, idxLogin);
				_Usuario.Senha = Utilities.GetStringNullCheck(reader, idxSenha);
				_Usuario.Ativo = Utilities.GetBoolNullCheck(reader, idxAtivo);
				_Usuario.Perfil = new Entities.Perfil { Nome = Utilities.GetStringNullCheck(reader, idxPerfil) };

				lstUsuario.Add(_Usuario);
			}

			reader.Close();
			conn.FechaConexao();

			return lstUsuario;
		}

		public int SalvaUsuario(Entities.Usuario usuario) {
			int salvou = 0;

			if (usuario != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				if (usuario.ID > 0) {
					sqlString.AppendLine("UPDATE usuario SET ");
					sqlString.AppendLine(String.Format(" nome = '{0}',", Utilities.campoString(usuario.Nome)));
					sqlString.AppendLine(String.Format(" email = '{0}',", Utilities.campoString(usuario.Email)));
					sqlString.AppendLine(String.Format(" login = '{0}',", Utilities.campoString(usuario.Login)));
					sqlString.AppendLine(String.Format(" ativo = {0}, ", (usuario.Ativo ? 1 : 0)));
					sqlString.AppendLine(String.Format(" idperfil = {0} ", usuario.PerfilId));

					// Verificar se o usuário mudou a senha
					var usuarioBo = new Usuario(_connectionString);
					var listaUsuario = usuarioBo.ListaUsuario(new Entities.Usuario(usuario.ID));
					MD5 md5Hash = MD5.Create();
					foreach (Entities.UsuarioVO item in listaUsuario) {
						if (usuario.Senha != item.Senha) {
							sqlString.AppendLine(String.Format(" ,senha = '{0}' ", Utilities.GetMd5Hash(md5Hash, usuario.Senha.Replace("'", "''"))));
						}
					}

					sqlString.AppendLine(String.Format(" WHERE id = {0}", usuario.ID));
				} else {
					usuario.ID = conn.getLast("Usuario");
					MD5 md5Hash = MD5.Create();

					sqlString.AppendLine("INSERT INTO usuario(nome, email, login, senha, ativo, idperfil)");
					sqlString.AppendLine(String.Format("VALUES('{0}', '{1}', '{2}', '{3}', {4}, {5})", Utilities.campoString(usuario.Nome), Utilities.campoString(usuario.Email),
						Utilities.campoString(usuario.Login), Utilities.GetMd5Hash(md5Hash, Utilities.campoString(usuario.Senha)), (usuario.Ativo ? 1 : 0), usuario.PerfilId));
				}

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = usuario.ID;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public int SalvaUsuario(int id, string nome, string email, string login, string senha, bool ativo, int perfilid) {
			return SalvaUsuario(new Entities.Usuario(id, nome, email, login, senha, ativo, perfilid));
		}

		public bool ExcluiUsuario(Entities.Usuario usuario) {
			bool salvou = false;

			if (usuario != null && usuario.ID > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM usuario ");
				sqlString.AppendLine(String.Format("WHERE id = {0}", usuario.ID));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiUsuario(int id) {
			return ExcluiUsuario(new Entities.Usuario(id));
		}

	}
}
