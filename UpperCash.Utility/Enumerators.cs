using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UpperCash.Utility {
	/// <summary>
	/// Atributo para Tipos Enumerados.
	/// Contém o nome amigável do tipo enumerado - A descrição que o usuário vê
	/// </summary>
	public class Descricao : Attribute {
		public String Texto;

		/// <summary>
		/// Construtor padrão do Atributo
		/// </summary>
		/// <param name="texto">Recebe o Texto que é a Descrição do tipo enumerado</param>
		public Descricao(String texto) {
			Texto = texto;
		}
	}

	/// <summary>
	/// Atributo para Tipos Enumerados.
	/// Contém o valor que armazenado na base e que representa o tipo enumerado
	/// </summary>
	public class Chave : Attribute {
		public Object Key;

		/// <summary>
		/// Construtor padrão do Atributo
		/// </summary>
		/// <param name="key">Recebe o key que é a Chave do tipo enumerado. 
		/// A Chave é do tipo Object pois pode ser qualquer coisa. Número, String, etc.</param>
		public Chave(Object key) {
			Key = key;
		}
	}

	/// <summary>
	/// Tipo de comparação.
	/// </summary>
	public enum EnumeratorConditionType {
		/// <summary>Ignora elemento.</summary>
		Ignore,
		/// <summary>Apenas o elemento.</summary>
		Only
	}


	/// <summary>
	/// Classe que contém alguns métodos que auxiliam o tratamento 
	/// de tipos enumerados, utilizando os Atributos Descricao e Chave.
	/// </summary>
	public static class Enumerator {

		/// <summary>
		/// Retorna o Valor do Atributo Descricao de um Tipo Enumerado via Reflection
		/// </summary>
		/// <param name="en">Tipo enumerado para retornar a Descrição</param>
		/// <returns>Retorna o valor que está no Atributo Descricao</returns>
		public static string GetDescricao(Enum en) {
			string ret = en.ToString();

			FieldInfo memInfo = en.GetType().GetField(en.ToString());

			if (memInfo != null)
				ret = getDescricao(memInfo);

			return ret;
		}

		/// <summary>
		/// Interno. Pega a Descrição a partir de um MemberInfo
		/// </summary>
		/// <param name="memInfo"></param>
		/// <returns></returns>
		private static string getDescricao(FieldInfo memInfo) {
			string ret = "";
			if (memInfo != null) {
				ret = memInfo.Name;

				object[] attrs = memInfo.GetCustomAttributes(typeof(Descricao), false);
				if (attrs != null && attrs.Length > 0)
					return ((Descricao)attrs[0]).Texto;
			}

			return ret;
		}

		/// <summary>
		/// Retorna o Valor do Atributo Chave de um Tipo Enumerado via Reflection
		/// </summary>
		/// <param name="en">Tipo enumerado para retornar a Chave</param>
		/// <returns>Retorna o valor que está no Atributo Chave</returns>
		public static object GetChave(Enum en) {
			object ret = en.ToString();

			FieldInfo memInfo = en.GetType().GetField(en.ToString());

			if (memInfo != null)
				ret = getChave(memInfo);

			return ret;
		}

		private static object getChave(FieldInfo memInfo) {
			object ret = "";

			if (memInfo != null) {
				ret = memInfo.Name;

				object[] attrs = memInfo.GetCustomAttributes(typeof(Chave), false);
				if (attrs != null && attrs.Length > 0)
					ret = ((Chave)attrs[0]).Key;
				else
					ret = memInfo.GetRawConstantValue();
			}

			return ret;
		}

		public static Enum GetEnumByChave(Type en, object key) {
			foreach (FieldInfo fi in en.GetFields()) {
				if (HerdaDe(fi.FieldType, typeof(Enum))) {
					if ((getChave(fi).ToString() == key.ToString() || fi.GetRawConstantValue().ToString() == key.ToString())
						|| (getDescricao(fi).ToString() == key.ToString() || fi.Name == key.ToString()))
						return (Enum)fi.GetValue(fi);
				}
			}
			return null;
		}

		public static bool HerdaDe(Type t, Type comparetype) {
			if (t == comparetype)
				return true;

			if (t.BaseType == null || t.BaseType == typeof(object))
				return false;

			return HerdaDe(t.BaseType, comparetype);
		}

		/// <summary>
		/// Devolve o Valor Correspondente ao valor Chave (Atributo) de um tipo Enumerado
		/// </summary>
		/// <param name="en">O Tipo Enumerado para fazer os GetType() de Reflection</param>
		/// <param name="chave">O Valor chave que será usado para testar contra o Atributo Chave</param>
		/// <returns>Retorna um Tipo Enumerado com o valor encontrado. Ou retorna o próprio valor, caso não encontre nada </returns>
		public static Enum GetEnumByChave(Enum en, Object chave) {
			if (chave == null)
				return en;

			// Pega todos os valores possíveis
			MemberInfo[] memInfo = en.GetType().GetMembers();

			foreach (MemberInfo m in memInfo) {
				if (m.MemberType != MemberTypes.Field)
					continue;
				// Vamos pegar o valor da chave e verificar se bate com o que está vindo ...
				object[] attrs = m.GetCustomAttributes(typeof(Chave), false);
				// Verifica se pegou mesmo alguma coisa
				if (attrs != null && attrs.Length > 0)
					if (chave.Equals(((Chave)attrs[0]).Key))
						return (Enum)((FieldInfo)m).GetValue(m);
			}

			return en;
		}

		public static int? ChaveToIntEnum<TEnumType>(object chave) {
			if (chave == null)
				return null;

			// Pega todos os valores possíveis
			MemberInfo[] memInfo = typeof(TEnumType).GetMembers();

			foreach (MemberInfo m in memInfo) {
				if (m.MemberType != MemberTypes.Field)
					continue;
				// Vamos pegar o valor da chave e verificar se bate com o que está vindo ...
				object[] attrs = m.GetCustomAttributes(typeof(Chave), false);
				// Verifica se pegou mesmo alguma coisa
				if (attrs != null && attrs.Length > 0)
					if (chave.Equals(((Chave)attrs[0]).Key))
						return (int)((FieldInfo)m).GetValue(m);
			}

			return null;
		}

		public static TEnumType GetEnumByChave<TEnumType>(Object chave) where TEnumType : new() {
			int? valueEnum = ChaveToIntEnum<TEnumType>(chave);
			if (valueEnum == null)
				return default(TEnumType);
			return IntToEnum<TEnumType>(valueEnum.Value);
		}

		public static IList<T> GetLista<T>() {
			IList<T> list = new List<T>();
			foreach (FieldInfo item in typeof(T).GetFields()) {
				if (HerdaDe(item.FieldType, typeof(T)))
					list.Add((T)Enum.ToObject(typeof(T), Convert.ToInt32(item.GetRawConstantValue())));
			}
			return list;
		}

		public static IDictionary<string, string> GetListaEnum(Enum en) {
			FieldInfo[] memInfo = en.GetType().GetFields();

			IDictionary<string, string> ret = new Dictionary<string, string>();

			for (int i = 0;i <= memInfo.Length - 1;i++) {
				FieldInfo m = memInfo[i];
				// Precisa ter os Marcadores de Descricao e Chave
				object[] attrsKey = m.GetCustomAttributes(typeof(Chave), false);
				object[] attrsValue = m.GetCustomAttributes(typeof(Descricao), false);

				if ((attrsKey != null && attrsKey.Length > 0) && ((attrsValue != null && attrsValue.Length > 0))) {
					ret.Add(getChave(m).ToString(), (getDescricao(m)));
				}
			}

			return ret;
		}

		public static KeyValuePair<TKey, string> GetKeyValue<TKey>(Enum en) {
			return new KeyValuePair<TKey, string>((TKey)GetChave(en), GetDescricao(en));
		}

		public static EnumType IntToEnum<EnumType>(int value) where EnumType : new() {
			return (EnumType)Enum.ToObject(typeof(EnumType), value);
		}

		public static EnumType DescricaoToEnum<EnumType>(string descricao) where EnumType : new() {
			EnumType result = default(EnumType);
			bool found = false;
			foreach (FieldInfo fi in typeof(EnumType).GetFields()) {
				object[] attrsValue = fi.GetCustomAttributes(typeof(Descricao), false);
				if (attrsValue.Length <= 0) {
					//Se não tem o atributo, busca pelo nome do field.
					if (descricao == fi.Name) {
						found = true;
						result = (EnumType)fi.GetValue(fi);
						break;
					}
				} else {
					//Se tem o atributo, vai pelo valor dele.
					if (descricao == ((Descricao)attrsValue[0]).Texto) {
						result = (EnumType)fi.GetValue(fi);
						found = true;
						break;
					}
				}
			}
			if (!found)
				throw new Exception("Não foi possível determinar o tipo baseado na descrição " + descricao);
			return result;
		}

		[Obsolete("O método GetListaEnumerator, faz a mesma coisa.")]
		public static IDictionary<string, string> GetListaEnum(Type tipoEnum) {
			IDictionary<string, string> ret = GetListaEnumerator<string>(tipoEnum);
			return ret;
		}

		public static IDictionary<T, string> GetListaEnumerator<T>(Type tipoEnum) {
			return GetListaEnumerator<T>(tipoEnum, EnumeratorConditionType.Ignore, null);
		}

		public static IDictionary<T, string> GetListaEnumerator<T>(Type tipoEnum, EnumeratorConditionType tipoCopy, params Enum[] properties) {
			IDictionary<T, string> ret = new Dictionary<T, string>();

			foreach (FieldInfo fi in tipoEnum.GetFields()) {
				if (HerdaDe(fi.FieldType, typeof(Enum))) {
					//if (properties != null) {
					//	if (tipoCopy == EnumeratorConditionType.Ignore && properties.Exists(d => d.ToString() == fi.Name))
					//		continue;

					//	if (tipoCopy == EnumeratorConditionType.Only && !properties.Exists(d => d.ToString() == fi.Name))
					//		continue;
					//}

					//if (ret.ContainsKey((T)TypeHelper.ChangeType(getChave(fi), typeof(T))))
					//	continue;

					//ret.Add((T)TypeHelper.ChangeType(getChave(fi), typeof(T)), _catalog.GetString(getDescricao(fi)));
				}
			}

			return ret;
		}
	}
}