using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class Controle {
		private readonly string _connectionString = string.Empty;

		public Controle(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.Controle> ListaControle() {
			return ListaControle(null);
		}

		public Entities.Controle LoadByPk(int kID) {
			var controle = new Entities.Controle { Id = kID };

			var controles = ListaControle(controle);

			if (controles.Count == 0) return null;

			return ListaControle(controle)[0];
		}

		public IList<Entities.Controle> ListaControle(Entities.Controle controle) {
			var lstControle = new List<Entities.Controle>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT id, titulo, descricao, observacao, valor, data, dataagendada, datapagamento, idcategoria, idtipo, idpagamento, idusuario ");
			sqlString.AppendLine("  FROM Controle ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (controle != null) {
				if (!string.IsNullOrEmpty(controle.Titulo)) {
					sqlString.AppendFormat(" AND titulo = '{0}' ", controle.Titulo);
				}
				if (!string.IsNullOrEmpty(controle.Descricao)) {
					sqlString.AppendFormat(" AND descricao = '{0}' ", controle.Descricao);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxId = reader.GetOrdinal("id");
			int idxTitulo = reader.GetOrdinal("titulo");
			int idxDescricao = reader.GetOrdinal("descricao");
			int idxObservacao = reader.GetOrdinal("observacao");
			int idxValor = reader.GetOrdinal("valor");
			int idxData = reader.GetOrdinal("data");
			int idxDataAgendada = reader.GetOrdinal("dataagendada");
			int idxDataPagamento = reader.GetOrdinal("datapagamento");
			int idxIdCategoria = reader.GetOrdinal("idcategoria");
			int idxIdTipo = reader.GetOrdinal("idtipo");
			int idxIdPagamento = reader.GetOrdinal("idpagamento");
			int idxIdUsuario = reader.GetOrdinal("idusuario");

			while (reader.Read()) {
				var _Controle = new Entities.Controle {
					Id = Utilities.GetIntNullCheck(reader, idxId),
					Titulo = Utilities.GetStringNullCheck(reader, idxTitulo),
					Descricao = Utilities.GetStringNullCheck(reader, idxDescricao),
					Observacao = Utilities.GetStringNullCheck(reader, idxObservacao),
					Valor = Utilities.GetDoubleNullCheck(reader, idxValor),
					Data = Utilities.GetDateNullCheck(reader, idxData),
					Dataagendada = Utilities.GetDateNullCheck(reader, idxDataAgendada),
					Datapagamento = Utilities.GetDateNullCheck(reader, idxDataPagamento),
					Idcategoria = Utilities.GetIntNullCheck(reader, idxIdCategoria),
					Idtipo = Utilities.GetIntNullCheck(reader, idxIdTipo),
					Idpagamento = Utilities.GetIntNullCheck(reader, idxIdPagamento),
					Idusuario = Utilities.GetIntNullCheck(reader, idxIdUsuario)
				};

				lstControle.Add(_Controle);
			}

			reader.Close();
			conn.FechaConexao();

			return lstControle;
		}

		public int SalvaControle(Entities.Controle controle) {
			int salvou = 0;

			if (controle != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				if (controle.Id > 0) {
					sqlString.AppendLine("UPDATE Controle SET ");
					sqlString.AppendLine(String.Format(" titulo = '{0}',", Utilities.campoString(controle.Titulo)));
					sqlString.AppendLine(String.Format(" descricao = '{0}'", controle.Descricao));
					sqlString.AppendLine(String.Format(" observacao = '{0}'", controle.Observacao));
					sqlString.AppendLine(String.Format(" valor = '{0}'", controle.Valor));
					sqlString.AppendLine(String.Format(" data = '{0}'", controle.Data));
					sqlString.AppendLine(String.Format(" dataagendada = '{0}'", controle.Dataagendada));
					sqlString.AppendLine(String.Format(" datapagamento = '{0}'", controle.Datapagamento));
					sqlString.AppendLine(String.Format(" idcategoria = '{0}'", controle.Idcategoria));
					sqlString.AppendLine(String.Format(" idtipo = '{0}'", controle.Idtipo));
					sqlString.AppendLine(String.Format(" idpagamento = '{0}'", controle.Idpagamento));
					sqlString.AppendLine(String.Format(" idusuario = '{0}'", controle.Idusuario));
					sqlString.AppendLine(String.Format(" WHERE id = {0}", controle.Id));
				} else {
					controle.Id = conn.getLast("Controle");

					sqlString.AppendLine("INSERT INTO Controle(titulo, descricao, observacao, valor, data, dataagendada, datapagamento, idcategoria, idtipo, idpagamento, idusuario)");
					sqlString.AppendLine(String.Format("VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')", 
						Utilities.campoString(controle.Titulo), Utilities.campoString(controle.Descricao), Utilities.campoString(controle.Observacao), controle.Valor, controle.Data,
						  controle.Dataagendada, controle.Datapagamento, controle.Idcategoria, controle.Idtipo, controle.Idpagamento, controle.Idusuario));
				}

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = controle.Id;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public int SalvaControle(int id, string titulo, string descricao, string observacao, double valor, string data, string dataagendada, string datapagamento, 
			                       int idcategoria, int idtipo, int idpagamento, int idusuario) {
			return SalvaControle(new Entities.Controle(id, titulo, descricao, observacao, valor, data, dataagendada, datapagamento, idcategoria, idtipo, idpagamento, idusuario));
		}

		public bool ExcluiControle(Entities.Controle controle) {
			bool salvou = false;

			if (controle != null && controle.Id > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM Controle ");
				sqlString.AppendLine(String.Format("WHERE id = {0}", controle.Id));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiControle(int id) {
			return ExcluiControle(new Entities.Controle(id));
		}

	}
}
