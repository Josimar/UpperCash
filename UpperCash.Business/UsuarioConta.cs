using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class UsuarioConta {
		private readonly string _connectionString = string.Empty;

		public UsuarioConta(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.UsuarioConta> ListaUsuarioConta() {
			return ListaUsuarioConta(null);
		}

		public IList<Entities.UsuarioConta> ListaUsuarioConta(Entities.UsuarioConta usuarioConta) {
			var lstUsuarioConta = new List<Entities.UsuarioConta>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT idusuario, idtipo ");
			sqlString.AppendLine("  FROM UsuarioConta ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (usuarioConta != null) {
				if (usuarioConta.IdUsuario > 0) {
					sqlString.AppendFormat(" AND idusuario = '{0}' ", usuarioConta.IdUsuario);
				}
				if (usuarioConta.IdConta > 0) {
					sqlString.AppendFormat(" AND idConta = '{0}' ", usuarioConta.IdConta);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxIdUsuario = reader.GetOrdinal("idusuario");
			int idxIdConta = reader.GetOrdinal("idConta");

			while (reader.Read()) {
				var _UsuarioConta = new Entities.UsuarioConta {
					IdUsuario = Utilities.GetIntNullCheck(reader, idxIdUsuario),
					IdConta = Utilities.GetIntNullCheck(reader, idxIdConta)
				};

				lstUsuarioConta.Add(_UsuarioConta);
			}

			reader.Close();
			conn.FechaConexao();

			return lstUsuarioConta;
		}

		public int SalvaUsuarioConta(Entities.UsuarioConta usuarioConta) {
			int salvou = 0;

			if (usuarioConta != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				sqlString.AppendLine("INSERT INTO UsuarioConta(idusuario, idConta)");
				sqlString.AppendLine(String.Format("VALUES({0}, {1})", usuarioConta.IdUsuario, usuarioConta.IdConta));

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = usuarioConta.IdUsuario;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiUsuarioConta(Entities.UsuarioConta usuarioConta) {
			bool salvou = false;

			if (usuarioConta != null && usuarioConta.IdUsuario > 0 && usuarioConta.IdConta > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM UsuarioConta ");
				sqlString.AppendLine(String.Format("WHERE idusuario = {0}", usuarioConta.IdUsuario));
				sqlString.AppendLine(String.Format("  AND idConta = {0}", usuarioConta.IdConta));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

	}
}