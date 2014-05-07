using System;

namespace UpperCash.Utility {
	/// <summary>
	/// The Property is a primary key field for the underlying table.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class PrimaryKey : Attribute {
	}

	/// <summary>
	/// The Property is a NOT NULL field for the underlying table.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class NotNull : Attribute {
	}

	/// <summary>
	/// The Property is Translatable for the underlying table.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class TranslatableAttribute : Attribute {
	}

	/// <summary>
	/// The Property is Translatable for the underlying table.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class UnTranslatableAttribute : Attribute {
	}

	/// <summary>
	/// The Property define the IdMain value 
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class IdMainPropertyAttribute : Attribute {
	}

	/// <summary>
	/// The Class is a HasTranslatableProperties
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class HasTranslatablePropertiesAttribute : Attribute {
		public TranslationType Translation { get; set; }

		public HasTranslatablePropertiesAttribute(TranslationType translationType) {
			Translation = translationType;
		}

		public HasTranslatablePropertiesAttribute()
			: this(TranslationType.Auto) {
		}

		public enum TranslationType {
			Auto,
			Input,
			Output
		}
	}

	/// <summary>
	/// The Class is a ProxyTranslatable
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ProxyTranslatableAttribute : Attribute {
	}

	/// <summary>
	/// The Property is an IDENTITY field for the underlying table.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class Identity : Attribute {
	}

	/// <summary>
	/// The Property is an TEXT/BLOB/CLOB field for the underlying table.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class LongText : Attribute {
	}


	/// <summary>
	/// The Property a string, with a given maximum length in the underlying table.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class MaxStringLength : Attribute {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="maxLen"></param>
		public MaxStringLength(int maxLen) {
			Length = maxLen;
		}

		/// <summary>
		/// The string's maximum length.
		/// </summary>
		public int Length { get; set; }
	}
	[AttributeUsage(AttributeTargets.Property)]
	public class TemplateMarker : Attribute {
		public string FriendlyName { get; set; }
		public string ToolTip { get; set; }

		private NumberType _numberType = NumberType.None;
		public NumberType NumberType {
			get { return _numberType; }
			set { _numberType = value; }
		}

		public TemplateMarker(string friendlyName, string toolTip) {
			FriendlyName = friendlyName;
			ToolTip = toolTip;
		}
	}
}