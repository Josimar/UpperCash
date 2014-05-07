using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class Pagamento {
		private readonly string _connectionString = string.Empty;

		public Pagamento(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.Pagamento> ListaPagamento() {
			return ListaPagamento(null);
		}

		public Entities.Pagamento LoadByPk(int kID) {
			var pagamento = new Entities.Pagamento { Id = kID };

			var pagamentos = ListaPagamento(pagamento);

			if (pagamentos.Count == 0) return null;

			return ListaPagamento(pagamento)[0];
		}

		public IList<Entities.Pagamento> ListaPagamento(Entities.Pagamento pagamento) {
			var lstPagamento = new List<Entities.Pagamento>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT id, nome, limite, vencimento ");
			sqlString.AppendLine("  FROM Pagamento ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (pagamento != null) {
				if (!string.IsNullOrEmpty(pagamento.Nome)) {
					sqlString.AppendFormat(" AND nome = '{0}' ", pagamento.Nome);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxId = reader.GetOrdinal("id");
			int idxNome = reader.GetOrdinal("nome");
			int idxLimite = reader.GetOrdinal("limite");
			int idxVencimento = reader.GetOrdinal("vencimento");

			while (reader.Read()) {
				var _Pagamento = new Entities.Pagamento {
					Id = Utilities.GetIntNullCheck(reader, idxId),
					Nome = Utilities.GetStringNullCheck(reader, idxNome),
					Limite = Utilities.GetDoubleNullCheck(reader, idxLimite),
					Vencimento = Utilities.GetDateNullCheck(reader, idxVencimento)
				};

				lstPagamento.Add(_Pagamento);
			}

			reader.Close();
			conn.FechaConexao();

			return lstPagamento;
		}

		public int SalvaPagamento(Entities.Pagamento pagamento) {
			int salvou = 0;

			if (pagamento != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				if (pagamento.Id > 0) {
					sqlString.AppendLine("UPDATE Pagamento SET ");
					sqlString.AppendLine(String.Format(" nome = '{0}',", Utilities.campoString(pagamento.Nome)));
					sqlString.AppendLine(String.Format(" limite = '{0}',", pagamento.Limite));
					sqlString.AppendLine(String.Format(" vencimento = '{0}'", pagamento.Vencimento));
					sqlString.AppendLine(String.Format(" WHERE id = {0}", pagamento.Id));
				} else {
					pagamento.Id = conn.getLast("Pagamento");

					sqlString.AppendLine("INSERT INTO Pagamento(nome, limite, vencimento)");
					sqlString.AppendLine(String.Format("VALUES('{0}', '{1}', '{2}')", Utilities.campoString(pagamento.Nome), pagamento.Limite, pagamento.Vencimento));
				}

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = pagamento.Id;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public int SalvaPagamento(int id, string nome, double limite, string vencimento) {
			return SalvaPagamento(new Entities.Pagamento(id, nome, limite, vencimento));
		}

		public bool ExcluiPagamento(Entities.Pagamento Pagamento) {
			bool salvou = false;

			if (Pagamento != null && Pagamento.Id > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM Pagamento ");
				sqlString.AppendLine(String.Format("WHERE id = {0}", Pagamento.Id));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiPagamento(int id) {
			return ExcluiPagamento(new Entities.Pagamento(id));
		}

	}
}
