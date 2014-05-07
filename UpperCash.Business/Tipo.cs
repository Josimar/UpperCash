using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class Tipo {
		private readonly string _connectionString = string.Empty;

		public Tipo(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.Tipo> ListaTipo() {
			return ListaTipo(null);
		}

		public Entities.Tipo LoadByPk(int kID) {
			var tipo = new Entities.Tipo { Id = kID };

			var tipos = ListaTipo(tipo);

			if (tipos.Count == 0) return null;

			return ListaTipo(tipo)[0];
		}

		public IList<Entities.Tipo> ListaTipo(Entities.Tipo tipo) {
			var lstTipo = new List<Entities.Tipo>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT id, nome, positivo ");
			sqlString.AppendLine("  FROM Tipo ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (tipo != null) {
				if (!string.IsNullOrEmpty(tipo.Nome)) {
					sqlString.AppendFormat(" AND nome = '{0}' ", tipo.Nome);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxId = reader.GetOrdinal("id");
			int idxNome = reader.GetOrdinal("nome");
			int idxPositivo = reader.GetOrdinal("positivo");

			while (reader.Read()) {
				var _Tipo = new Entities.Tipo {
					Id = Utilities.GetIntNullCheck(reader, idxId),
					Nome = Utilities.GetStringNullCheck(reader, idxNome),
					Positivo = Utilities.GetBoolNullCheck(reader, idxPositivo)
				};

				lstTipo.Add(_Tipo);
			}

			reader.Close();
			conn.FechaConexao();

			return lstTipo;
		}

		public int SalvaTipo(Entities.Tipo tipo) {
			int salvou = 0;

			if (tipo != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				if (tipo.Id > 0) {
					sqlString.AppendLine("UPDATE Tipo SET ");
					sqlString.AppendLine(String.Format(" nome = '{0}',", Utilities.campoString(tipo.Nome)));
					sqlString.AppendLine(String.Format(" positivo = '{0}'", tipo.Positivo));
					sqlString.AppendLine(String.Format(" WHERE id = {0}", tipo.Id));
				} else {
					tipo.Id = conn.getLast("Tipo");

					sqlString.AppendLine("INSERT INTO Tipo(nome, positivo)");
					sqlString.AppendLine(String.Format("VALUES('{0}', '{1}')", Utilities.campoString(tipo.Nome), tipo.Positivo));
				}

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = tipo.Id;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public int SalvaTipo(int id, string nome, bool positivo) {
			return SalvaTipo(new Entities.Tipo(id, nome, positivo));
		}

		public bool ExcluiTipo(Entities.Tipo tipo) {
			bool salvou = false;

			if (tipo != null && tipo.Id > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM Tipo ");
				sqlString.AppendLine(String.Format("WHERE id = {0}", tipo.Id));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiTipo(int id) {
			return ExcluiTipo(new Entities.Tipo(id));
		}

	}
}