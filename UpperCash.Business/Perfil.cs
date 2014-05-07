using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UpperCash.Data;
using UpperCash.Utility;

namespace UpperCash.Business {
	public class Perfil {
		private readonly string _connectionString = string.Empty;

		public Perfil(string conexao) {
			if (!string.IsNullOrEmpty(conexao)) {
				_connectionString = conexao;
			}
		}

		public IList<Entities.Perfil> ListaPerfil() {
			return ListaPerfil(null);
		}

		public Entities.Perfil LoadByPk(int kID) {
			var perfil = new Entities.Perfil{Id = kID};

			var perfils = ListaPerfil(perfil);

			if (perfils.Count == 0) return null;

			return ListaPerfil(perfil)[0];
		}

		public IList<Entities.Perfil> ListaPerfil(Entities.Perfil perfil) {
			var lstPerfil = new List<Entities.Perfil>();

			var conn = new Connection(_connectionString);
			conn.AbrirConexao();

			var sqlString = new StringBuilder();
			sqlString.AppendLine("SELECT id, nome, descricao, flags ");
			sqlString.AppendLine("  FROM Perfil ");
			sqlString.AppendLine(" WHERE 1 = 1   ");

			if (perfil != null) {
				if (!string.IsNullOrEmpty(perfil.Nome)) {
					sqlString.AppendFormat(" AND nome = '{0}' ", perfil.Nome);
				}
			}

			IDataReader reader = conn.RetornaDados(sqlString.ToString());

			int idxId = reader.GetOrdinal("id");
			int idxNome = reader.GetOrdinal("nome");
			int idxDescricao = reader.GetOrdinal("descricao");
			int idxFlags = reader.GetOrdinal("flags");

			while (reader.Read()) {
				var _Perfil = new Entities.Perfil{
					Id = Utilities.GetIntNullCheck(reader, idxId),
					Nome = Utilities.GetStringNullCheck(reader, idxNome),
					Descricao = Utilities.GetStringNullCheck(reader, idxDescricao),
					Flags = Utilities.GetIntNullCheck(reader, idxFlags)
				};

				lstPerfil.Add(_Perfil);
			}

			reader.Close();
			conn.FechaConexao();

			return lstPerfil;
		}

		public int SalvaPerfil(Entities.Perfil perfil) {
			int salvou = 0;

			if (perfil != null) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();

				if (perfil.Id > 0) {
					sqlString.AppendLine("UPDATE Perfil SET ");
					sqlString.AppendLine(String.Format(" nome = '{0}',", Utilities.campoString(perfil.Nome)));
					sqlString.AppendLine(String.Format(" descricao = '{0}'", Utilities.campoString(perfil.Descricao)));
					sqlString.AppendLine(String.Format(" WHERE id = {0}", perfil.Id));
				} else {
					perfil.Id = conn.getLast("Perfil");

					sqlString.AppendLine("INSERT INTO Perfil(nome, descricao, flags)");
					sqlString.AppendLine(String.Format("VALUES('{0}', '{1}', {2})", Utilities.campoString(perfil.Nome), Utilities.campoString(perfil.Descricao), 0));
				}

				int i = conn.ExecutaComando(sqlString.ToString());
				if (i > 0) {
					salvou = perfil.Id;
				}

				conn.FechaConexao();
			}

			return salvou;
		}

		public int SalvaPerfil(int id, string nome, string descricao, int flags) {
			return SalvaPerfil(new Entities.Perfil(id, nome, descricao, flags));
		}

		public bool ExcluiPerfil(Entities.Perfil Perfil) {
			bool salvou = false;

			if (Perfil != null && Perfil.Id > 0) {
				var conn = new Connection(_connectionString);
				conn.AbrirConexao();

				var sqlString = new StringBuilder();
				sqlString.AppendLine("DELETE FROM Perfil ");
				sqlString.AppendLine(String.Format("WHERE id = {0}", Perfil.Id));

				int i = conn.ExecutaComando(sqlString.ToString());
				salvou = i > 0;

				conn.FechaConexao();
			}

			return salvou;
		}

		public bool ExcluiPerfil(int id) {
			return ExcluiPerfil(new Entities.Perfil(id));
		}

	}
}
