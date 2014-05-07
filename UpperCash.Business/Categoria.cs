using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class Categoria {

		private readonly string _connectionString = string.Empty;

		public Categoria(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.Categoria> ListaCategoria() {
			return ListaCategoria(null);
		}

		public Entities.Categoria LoadByPk(int kID) {
			var categoria = new Entities.Categoria { Id = kID };
			return ListaCategoriaUnica(categoria)[0];
		}

		public IList<Entities.Categoria> ListaCategoria(Entities.Categoria categoria) {
			var lstCategoria = new List<Entities.Categoria>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT id, idpai, nome ");
			sqlString.AppendLine("  FROM Categoria ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (categoria != null) {
				if (!string.IsNullOrEmpty(categoria.Nome)) {
					sqlString.AppendFormat(" AND nome = '{0}' ", categoria.Nome);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxId = reader.GetOrdinal("id");
			int idxIdPai = reader.GetOrdinal("idpai");
			int idxNome = reader.GetOrdinal("nome");

			while (reader.Read()) {
				var _Categoria = new Entities.Categoria {
					Id = Utilities.GetIntNullCheck(reader, idxId),
					Nome = Utilities.GetStringNullCheck(reader, idxNome),
					IdPai = Utilities.GetIntNullCheck(reader, idxIdPai)
				};

				lstCategoria.Add(_Categoria);
			}

			reader.Close();
			conn.FechaConexao();

			return lstCategoria;
		}

		public IList<Entities.Categoria> ListaCategoriaUnica(Entities.Categoria categoria) {
			var lstCategoriaUnica = new List<Entities.Categoria>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT id, idpai, nome ");
			sqlString.AppendLine("  FROM Categoria ");
			sqlString.AppendFormat(" WHERE ID = {0}", categoria.Id);

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxId = reader.GetOrdinal("id");
			int idxIdPai = reader.GetOrdinal("idpai");
			int idxNome = reader.GetOrdinal("nome");

			while (reader.Read()) {
				var _Categoria = new Entities.Categoria {
					Id = Utilities.GetIntNullCheck(reader, idxId),
					Nome = Utilities.GetStringNullCheck(reader, idxNome),
					IdPai = Utilities.GetIntNullCheck(reader, idxIdPai)
				};

				lstCategoriaUnica.Add(_Categoria);
			}

			reader.Close();
			conn.FechaConexao();

			return lstCategoriaUnica;
		}

		public int SalvaCategoria(Entities.Categoria categoria) {
			int salvou = 0;

			if (categoria != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				if (categoria.Id > 0) {
					sqlString.AppendLine("UPDATE Categoria SET ");
					sqlString.AppendLine(String.Format(" nome = '{0}',", Utilities.campoString(categoria.Nome)));
					sqlString.AppendLine(String.Format(" idpai = '{0}'", categoria.IdPai == 0 ? categoria.Id : categoria.IdPai));
					sqlString.AppendLine(String.Format(" WHERE id = {0}", categoria.Id));
				} else {
					categoria.Id = conn.getLast("Categoria");

					//quando for raiz, ele mesmo é o pai..
                    if (categoria.IdPai.GetValueOrDefault() == 0)
                    {
						categoria.IdPai = categoria.Id;
					}

					sqlString.AppendLine("INSERT INTO Categoria(idpai, nome)");
					sqlString.AppendLine(String.Format("VALUES('{0}', '{1}')", categoria.IdPai.GetValueOrDefault(), Utilities.campoString(categoria.Nome)));
				}

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = categoria.Id;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public int SalvaCategoria(int id, int idpai, string nome) {
			return SalvaCategoria(new Entities.Categoria(id, idpai, nome));
		}

		public bool ExcluiCategoria(Entities.Categoria categoria) {
			bool salvou = false;

			if (categoria != null && categoria.Id > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM Categoria ");
				sqlString.AppendLine(String.Format("WHERE id = {0}", categoria.Id));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiCategoria(int id) {
			return ExcluiCategoria(new Entities.Categoria(id));
		}

	}
}
