using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class UsuarioTipo {
		private readonly string _connectionString = string.Empty;

		public UsuarioTipo(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.UsuarioTipo> ListaUsuarioTipo() {
			return ListaUsuarioTipo(null);
		}

		public IList<Entities.UsuarioTipo> ListaUsuarioTipo(Entities.UsuarioTipo usuarioTipo) {
			var lstUsuarioTipo = new List<Entities.UsuarioTipo>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT idusuario, idtipo ");
			sqlString.AppendLine("  FROM UsuarioTipo ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (usuarioTipo != null) {
				if (usuarioTipo.IdUsuario > 0) {
					sqlString.AppendFormat(" AND idusuario = '{0}' ", usuarioTipo.IdUsuario);
				}
				if (usuarioTipo.IdTipo > 0) {
					sqlString.AppendFormat(" AND idtipo = '{0}' ", usuarioTipo.IdTipo);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxIdUsuario = reader.GetOrdinal("idusuario");
			int idxidtipo = reader.GetOrdinal("idtipo");

			while (reader.Read()) {
				var _UsuarioTipo = new Entities.UsuarioTipo {
					IdUsuario = Utilities.GetIntNullCheck(reader, idxIdUsuario),
					IdTipo = Utilities.GetIntNullCheck(reader, idxidtipo)
				};

				lstUsuarioTipo.Add(_UsuarioTipo);
			}

			reader.Close();
			conn.FechaConexao();

			return lstUsuarioTipo;
		}

		public int SalvaUsuarioTipo(Entities.UsuarioTipo usuarioTipo) {
			int salvou = 0;

			if (usuarioTipo != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				sqlString.AppendLine("INSERT INTO UsuarioTipo(idusuario, idtipo)");
				sqlString.AppendLine(String.Format("VALUES({0}, {1})", usuarioTipo.IdUsuario, usuarioTipo.IdTipo));

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = usuarioTipo.IdUsuario;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiUsuarioTipo(Entities.UsuarioTipo usuarioTipo) {
			bool salvou = false;

			if (usuarioTipo != null && usuarioTipo.IdUsuario > 0 && usuarioTipo.IdTipo > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM UsuarioTipo ");
				sqlString.AppendLine(String.Format("WHERE idusuario = {0}", usuarioTipo.IdUsuario));
				sqlString.AppendLine(String.Format("  AND idtipo = {0}", usuarioTipo.IdTipo));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

	}
}