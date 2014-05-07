using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class Conta {
		private readonly string _connectionString = string.Empty;

		public Conta(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.Conta> ListaConta() {
			return ListaConta(null);
		}

		public Entities.Conta LoadByPk(int kID) {
			var conta = new Entities.Conta{Id = kID};

			var contas = ListaConta(conta);

			if (contas.Count == 0) return null;

			return ListaConta(conta)[0];
		}

		public IList<Entities.Conta> ListaConta(Entities.Conta conta) {
			var lstConta = new List<Entities.Conta>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT id, valordisponivel, nome ");
			sqlString.AppendLine("  FROM Conta ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (conta != null) {
				if (!string.IsNullOrEmpty(conta.Nome)) {
					sqlString.AppendFormat(" AND nome = '{0}' ", conta.Nome);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxId = reader.GetOrdinal("id");
			int idxValorDisponivel = reader.GetOrdinal("valordisponivel");
			int idxNome = reader.GetOrdinal("nome");

			while (reader.Read()) {
				var _Conta = new Entities.Conta{
					Id = Utilities.GetIntNullCheck(reader, idxId),
					Nome = Utilities.GetStringNullCheck(reader, idxNome),
					ValorDisponivel = Utilities.GetDoubleNullCheck(reader, idxValorDisponivel)
				};

				lstConta.Add(_Conta);
			}

			reader.Close();
			conn.FechaConexao();

			return lstConta;
		}

		public int SalvaConta(Entities.Conta conta) {
			int salvou = 0;

			if (conta != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				if (conta.Id > 0) {
					sqlString.AppendLine("UPDATE Conta SET ");
					sqlString.AppendLine(String.Format(" nome = '{0}',", Utilities.campoString(conta.Nome)));
					sqlString.AppendLine(String.Format(" valordisponivel = '{0}'", conta.ValorDisponivel));
					sqlString.AppendLine(String.Format(" WHERE id = {0}", conta.Id));
				} else {
					conta.Id = conn.getLast("Conta");

					sqlString.AppendLine("INSERT INTO Conta(valordisponivel, nome)");
					sqlString.AppendLine(String.Format("VALUES('{0}', '{1}')", conta.ValorDisponivel, Utilities.campoString(conta.Nome)));
				}

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = conta.Id;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public int SalvaConta(int id, double valordisponivel, string nome) {
			return SalvaConta(new Entities.Conta(id, valordisponivel, nome));
		}

		public bool ExcluiConta(Entities.Conta conta) {
			bool salvou = false;

			if (conta != null && conta.Id > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM Conta ");
				sqlString.AppendLine(String.Format("WHERE id = {0}", conta.Id));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiConta(int id) {
			return ExcluiConta(new Entities.Conta(id));
		}

	}
}
