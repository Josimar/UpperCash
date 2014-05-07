using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class UsuarioPagamento {
		private readonly string _connectionString = string.Empty;

		public UsuarioPagamento(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.UsuarioPagamento> ListaUsuarioPagamento() {
			return ListaUsuarioPagamento(null);
		}

		public IList<Entities.UsuarioPagamento> ListaUsuarioPagamento(Entities.UsuarioPagamento usuarioPagamento) {
			var lstUsuarioPagamento = new List<Entities.UsuarioPagamento>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT idusuario, idpagamento ");
			sqlString.AppendLine("  FROM UsuarioPagamento ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (usuarioPagamento != null) {
				if (usuarioPagamento.IdUsuario > 0) {
					sqlString.AppendFormat(" AND idusuario = '{0}' ", usuarioPagamento.IdUsuario);
				}
				if (usuarioPagamento.IdPagamento > 0) {
					sqlString.AppendFormat(" AND idpagamento = '{0}' ", usuarioPagamento.IdPagamento);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxIdUsuario = reader.GetOrdinal("idusuario");
			int idxidpagamento = reader.GetOrdinal("idpagamento");

			while (reader.Read()) {
				var _UsuarioPagamento = new Entities.UsuarioPagamento {
					IdUsuario = Utilities.GetIntNullCheck(reader, idxIdUsuario),
					IdPagamento = Utilities.GetIntNullCheck(reader, idxidpagamento)
				};

				lstUsuarioPagamento.Add(_UsuarioPagamento);
			}

			reader.Close();
			conn.FechaConexao();

			return lstUsuarioPagamento;
		}

		public int SalvaUsuarioPagamento(Entities.UsuarioPagamento usuarioPagamento) {
			int salvou = 0;

			if (usuarioPagamento != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				sqlString.AppendLine("INSERT INTO UsuarioPagamento(idusuario, idpagamento)");
				sqlString.AppendLine(String.Format("VALUES({0}, {1})", usuarioPagamento.IdUsuario, usuarioPagamento.IdPagamento));

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = usuarioPagamento.IdUsuario;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiUsuarioPagamento(Entities.UsuarioPagamento usuarioPagamento) {
			bool salvou = false;

			if (usuarioPagamento != null && usuarioPagamento.IdUsuario > 0 && usuarioPagamento.IdPagamento > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM UsuarioPagamento ");
				sqlString.AppendLine(String.Format("WHERE idusuario = {0}", usuarioPagamento.IdUsuario));
				sqlString.AppendLine(String.Format("  AND idpagamento = {0}", usuarioPagamento.IdPagamento));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

	}
}