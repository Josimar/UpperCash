using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class UsuarioCategoria {
		private readonly string _connectionString = string.Empty;

		public UsuarioCategoria(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.UsuarioCategoria> ListaUsuarioCategoria() {
			return ListaUsuarioCategoria(null);
		}
		
		public IList<Entities.UsuarioCategoria> ListaUsuarioCategoria(Entities.UsuarioCategoria usuarioCategoria) {
			var lstUsuarioCategoria = new List<Entities.UsuarioCategoria>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT idusuario, idcategoria ");
			sqlString.AppendLine("  FROM UsuarioCategoria ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (usuarioCategoria != null) {
				if (usuarioCategoria.IdUsuario > 0) {
					sqlString.AppendFormat(" AND idusuario = '{0}' ", usuarioCategoria.IdUsuario);
				}
				if (usuarioCategoria.IdCategoria > 0) {
					sqlString.AppendFormat(" AND idcategoria = '{0}' ", usuarioCategoria.IdCategoria);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxIdUsuario = reader.GetOrdinal("idusuario");
			int idxIdCategoria = reader.GetOrdinal("idcategoria");

			while (reader.Read()) {
				var _UsuarioCategoria = new Entities.UsuarioCategoria {
					IdUsuario = Utilities.GetIntNullCheck(reader, idxIdUsuario),
					IdCategoria = Utilities.GetIntNullCheck(reader, idxIdCategoria)
				};

				lstUsuarioCategoria.Add(_UsuarioCategoria);
			}

			reader.Close();
			conn.FechaConexao();

			return lstUsuarioCategoria;
		}

		public int SalvaUsuarioCategoria(Entities.UsuarioCategoria usuarioCategoria) {
			int salvou = 0;

			if (usuarioCategoria != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				sqlString.AppendLine("INSERT INTO UsuarioCategoria(idusuario, idcategoria)");
				sqlString.AppendLine(String.Format("VALUES({0}, {1})", usuarioCategoria.IdUsuario, usuarioCategoria.IdCategoria));

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = usuarioCategoria.IdUsuario;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiUsuarioCategoria(Entities.UsuarioCategoria usuarioCategoria) {
			bool salvou = false;

			if (usuarioCategoria != null && usuarioCategoria.IdUsuario > 0 && usuarioCategoria.IdCategoria > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM UsuarioCategoria ");
				sqlString.AppendLine(String.Format("WHERE idusuario = {0}", usuarioCategoria.IdUsuario));
				sqlString.AppendLine(String.Format("  AND idcategoria = {0}", usuarioCategoria.IdCategoria));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}
		
	}
}