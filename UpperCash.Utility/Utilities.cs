using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace UpperCash.Utility {
	public enum NumberType {
		None,
		Quantity,
		Price,
		Percent,
		Integer
	}

	public class Utilities {
		public static string GetStringNullCheck(IDataReader reader, int ordinal) {
			try {
				return reader.IsDBNull(ordinal) ? "" : reader.GetFieldType(ordinal).Name == "Int32" ? reader.GetInt32(ordinal).ToString() : HttpUtility.HtmlDecode(reader.GetString(ordinal));
			} catch (Exception) {
				return "";
			}
		}

		public static int GetIntNullCheck(IDataReader reader, int ordinal) {
			try {
				return reader.IsDBNull(ordinal) ? 0 : reader.GetFieldType(ordinal).Name == "Int32" ? reader.GetInt32(ordinal) : Convert.ToInt32(reader.GetString(ordinal));
			} catch (Exception) {
				return 0;
			}
		}

		public static double GetDoubleNullCheck(IDataReader reader, int ordinal){
			try {
				return reader.IsDBNull(ordinal) ? 0 : reader.GetFieldType(ordinal).Name == "Double" ? reader.GetDouble(ordinal) : Convert.ToDouble(reader.GetString(ordinal));
			} catch (Exception) {
				return 0;
			}
		}

		public static string GetDateNullCheck(IDataReader reader, int ordinal, string myFormat = "dd/MM/yyyy HH:mm:ss") {
			try {
				return reader.IsDBNull(ordinal) ? "" : reader.GetDateTime(ordinal).ToString(myFormat);
			} catch (Exception) {
				return "";
			}
		}

		public static bool GetBoolNullCheck(IDataReader reader, int ordinal) {
			try {
				return reader.IsDBNull(ordinal) || (reader.GetFieldType(ordinal).Name == "Int32" ?
					reader.GetInt32(ordinal) > 0 : reader.GetFieldType(ordinal).Name == "Boolean" ? reader.GetBoolean(ordinal) : Convert.ToInt32(reader.GetString(ordinal)) > 0);
			} catch (Exception) {
				return true;
			}
		}

		public static string campoString(string campo) {
			return string.IsNullOrEmpty(campo) ? "" : campo.Replace("'", "''");
		}

		public static string GetMd5Hash(MD5 md5Hash, string input) {

			// Convert the input string to a byte array and compute the hash. 
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes 
			// and create a string.
			var sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data  
			// and format each one as a hexadecimal string. 
			foreach (byte t in data) {
				sBuilder.Append(t.ToString("x2"));
			}

			// Return the hexadecimal string. 
			return sBuilder.ToString();
		}

	}
}
