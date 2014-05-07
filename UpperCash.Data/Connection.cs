using System;
using System.Data;
using System.Data.SqlClient;

/*
 * ToDo: Outras bases de dados
System.Data.OleDb
System.Data.OracleClient
MySql.Data.MySqlClient
System.Data.SQLite
FirebirdSql.Data.FirebirdClient
*/

namespace UpperCash.Data {
	public class Connection {

		private SqlConnection Conexao;

		/// <summary>
		/// Dados de conexao para SqlServer
		/// </summary>
		public string ConnectionString { get; set; }

		public Connection() { }

		/// <summary>
		/// Construtor que recebe como parametro a ConnectionString
		/// </summary>
		/// <param name="connectionString"></param>
		public Connection(string connectionString) {
			ConnectionString = connectionString;
		}

		/// <summary>
		/// Abre conexao
		/// </summary>
		public void AbrirConexao() {
			if (string.IsNullOrEmpty(ConnectionString)) throw new Exception("Não foi informado a ConnectionString.");

			SqlConnection.ClearAllPools();

			if (Conexao == null) {
				Conexao = new SqlConnection { ConnectionString = ConnectionString };
			}

			try {
				Conexao.Open();
			} catch (Exception ex) {
				throw;
			}

		}

		/// <summary>
		/// Fecha conexao
		/// </summary>
		public void FechaConexao() {
			if (Conexao != null && Conexao.State == ConnectionState.Open) {
				Conexao.Close();
			}
		}

		/// <summary>
		/// Retorna os dados via IDataReader
		/// </summary>
		/// <param name="sql">Comando SQL</param>
		/// <returns>Retorna coleção de dados</returns>
		public IDataReader RetornaDados(string sql) {
			if (string.IsNullOrEmpty(sql)) throw new Exception("Não foi informado a query SQL.");
			if (Conexao == null || Conexao.State == ConnectionState.Closed) throw new Exception("A conexão fechada. Execute o comando AbrirConexao e não se esqueça de FecharConexao no final.");

			var command = new SqlCommand { Connection = Conexao, CommandText = sql };
			IDataReader readerDados = command.ExecuteReader();

			return readerDados;
		}

		/// <summary>
		/// Executa comando e retorna o total de linhas  afetadas
		/// </summary>
		/// <param name="sql">Comando SQL</param>
		/// <returns>Retorna o total de linhas afetadas</returns>
		public int ExecutaComando(string sql) {
			if (string.IsNullOrEmpty(sql)) throw new Exception("Não foi informado a query SQL.");
			if (Conexao == null || Conexao.State == ConnectionState.Closed) throw new Exception("A conexão fechada. Execute o comando AbrirConexao e não se esqueça de FecharConexao no final.");

			var command = new SqlCommand { Connection = Conexao, CommandText = sql };
			int result = command.ExecuteNonQuery();

			return result;
		}

		/// <summary>
		/// Retorna a primeira coluna da query, as outras são ignoradas
		/// </summary>
		/// <param name="sql">Comando SQL</param>
		/// <returns>A primeira coluna da query</returns>
		public int ExecutaContador(string sql) {
			if (string.IsNullOrEmpty(sql)) throw new Exception("Não foi informado a query SQL.");
			if (Conexao == null || Conexao.State == ConnectionState.Closed) throw new Exception("A conexão fechada. Execute o comando AbrirConexao e não se esqueça de FecharConexao no final.");

			int result;
			SqlCommand command;
			using (command = new SqlCommand()) {
				command.Connection = Conexao;
				command.CommandText = sql;
				try {
					result = (int)command.ExecuteScalar();
				} catch (Exception e) {
					result = 0;
				}
			}

			return result;
		}

		/// <summary>
		/// Busca o ultimo id livre da tabela
		/// </summary>
		/// <param name="tabela">Nome da Tabela</param>
		/// <returns>Busca o último id livre da tabela</returns>
		public int getLast(string tabela) {
			if (string.IsNullOrEmpty(tabela)) throw new Exception("Não foi informado a tabela do banco de dados.");
			if (Conexao == null || Conexao.State == ConnectionState.Closed) throw new Exception("A conexão fechada. Execute o comando AbrirConexao e não se esqueça de FecharConexao no final.");

			int ultimoId = ExecutaContador(string.Format("SELECT TOP 1 ID FROM {0} ORDER BY ID DESC", tabela));

			return ultimoId + 1;
		}

	}
}
