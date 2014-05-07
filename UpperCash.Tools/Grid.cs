using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Web.Routing;
using System.Xml.Serialization;
using NativeExcel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UpperCash.Utility;

namespace UpperCash.Tools {

	public enum GridColumnDataType {
		String,
		Integer,
		Price,
		Quantity,
		Percent,
		DateTime,
		Date,
		Time,
		Boolean,
		Button
	}

	public enum ResultType {
		Json = 0,
		Excel = 1,
		XML = 2,
		PDF = 3
	}

	public enum LetrasGrid{
		a,
		b,
		c,
		d,
		e
	}

	public static class GridExtension {

		public static MvcHtmlString Grid(this HtmlHelper htmlHelper, string id, IList lista,
			Action<GridColumns> getColumnsMethod, Action<GridBuilder> optionsHandle = null, object htmlAttributes = null) {
			// var controller = (BaseController)htmlHelper.ViewContext.Controller;

			// var cols = new GridColumns(controller.visitor.precisaoQuantidade, controller.visitor.precisaoPreco, controller.visitor.precisaoPercentual);
			var cols = new GridColumns();
			getColumnsMethod(cols);

			var options = new GridBuilder();
			if (optionsHandle != null)
				optionsHandle(options);

			return Grid(htmlHelper, id, lista, "grid", cols, options, htmlAttributes);
		}

		public static MvcHtmlString GridFor<TModel>(this HtmlHelper<TModel> htmlHelper, string ID, IList lista,
			Action<GridColumns<TModel>> getColumnsMethod, Action<GridBuilder> optionsHandle = null, object htmlAttributes = null) {

			var cols = new GridColumns<TModel>(htmlHelper);
			getColumnsMethod(cols);

			var options = new GridBuilder();
			if (optionsHandle != null)
				optionsHandle(options);

			return Grid(htmlHelper, ID, lista, "grid", cols, options, htmlAttributes);
		}

		private static MvcHtmlString Grid(this HtmlHelper htmlHelper, string id, IList lista, string ClassDataTable,
			GridColumns columns, GridBuilder options, object htmlAttributes, bool renderScript = false) {
			// var controller = (BaseController)htmlHelper.ViewContext.Controller;

			var div = new TagBuilder("div");
			div.AddCssClass("ui-grid-" + (LetrasGrid)(columns.GetList().Count > 3 ? 3 : columns.GetList().Count - 1) + " ui-responsive");
			if (htmlAttributes != null)
				div.MergeAttributes<string, object>(new RouteValueDictionary(htmlAttributes), true);
			div.GenerateId(id);

			int i = -1;
			var divHeader = "";
			foreach (GridColumnBuilder header in columns.GetList()){
				i++;
				if (!String.IsNullOrEmpty(header.GetTitle()))
					divHeader += "<div class='ui-block-" + (LetrasGrid)i + "'><div class='ui-body ui-body-b' style='height:60px'>" + header.GetTitle() + "</div></div>";
				else
					divHeader += "<div class='ui-block-" + (LetrasGrid)i + "'><div class='ui-body ui-body-b' style='height:60px'>&nbsp;</div></div>";
				
			}
			divHeader += "";

			var divConteudo = "";
			
			PropertyInfo[] properties = lista.GetType().GetProperties();
			foreach (PropertyInfo property in properties){
				if ((property.PropertyType.Name == "Usuario") || (property.PropertyType.Name == "UsuarioVO")) {
					foreach (Entities.UsuarioVO usuario in lista){
						i = -1;
						foreach (GridColumnBuilder header in columns.GetList()){
							PropertyInfo[] pis = usuario.GetType().GetProperties();
							foreach (PropertyInfo p in pis){
								if (header.GetType() == GridColumnDataType.Button){
									i++;
									divConteudo += "<div class='ui-block-" + (LetrasGrid) i + "'><a href='/Usuario/Editar/" + usuario.ID + "' class='ui-shadow ui-btn ui-corner-all ui-icon-edit ui-btn-icon-notext ui-btn-inline'>" + header.GetTitle() + "</a></div>";
									break;
								}
								if (p.Name == header.GetDataField()){
									i++;
									divConteudo += "<div class='ui-block-" + (LetrasGrid)i + "'><div class='ui-body ui-body-a' style='height:60px'>" + p.GetValue(usuario, null) + "</div></div>";
									break;
								}
							}
						}
					}
				}
                else if (property.PropertyType.Name == "Categoria")
                {
                    foreach (Entities.Categoria categoria in lista)
                    {
                        foreach (GridColumnBuilder header in columns.GetList())
                        {
                            PropertyInfo[] pis = categoria.GetType().GetProperties();
                            foreach (PropertyInfo p in pis)
                            {
                                if (p.Name == header.GetDataField())
                                {
                                    divConteudo += "<div class='ui-block-" + (LetrasGrid)i + "'><div class='ui-body ui-body-a' style='height:60px'>" + p.GetValue(categoria, null) + "</div></div>";
                                }
                            }
                        }
                    }
                }
			}

			div.InnerHtml = divHeader + divConteudo;

			string script = RegisterScript(htmlHelper, id, lista, ClassDataTable, columns, options, renderScript);
			if (renderScript)
				return MvcHtmlString.Create(script);

			return MvcHtmlString.Create(div.ToString());
		}

		private static string RegisterScript(HtmlHelper htmlHelper, string id, IList lista, string ClassDataTable,
			GridColumns columns, GridBuilder options, bool renderScript){

			string jsInit = "";
			int curr = 0;

			foreach (GridColumnBuilder c in columns.GetList()) {
				Dictionary<string, string> d = BuildDictionaryForColProps(c, curr, htmlHelper);
				if (curr > 0 && curr < columns.GetList().Count)
					jsInit += ", \n";
				jsInit += "      " + GetGridColJSString(d);

				curr++;
			}
			
			jsInit = ("<div class='ui-grid-b'>");
			jsInit += ("    <div class='ui-block-a'><div class='ui-bar ui-bar-a' style='height:60px'>Block A</div></div>");
			jsInit += ("    <div class='ui-block-b'><div class='ui-bar ui-bar-a' style='height:60px'>Block B</div></div>");
			jsInit += ("    <div class='ui-block-c'><div class='ui-bar ui-bar-a' style='height:60px'>Block C</div></div>");
			jsInit += ("    <div class='ui-block-a'><div class='ui-bar ui-bar-a' style='height:60px'>Block A</div></div>");
			jsInit += ("    <div class='ui-block-b'><div class='ui-bar ui-bar-a' style='height:60px'>Block B</div></div>");
			jsInit += ("    <div class='ui-block-c'><div class='ui-bar ui-bar-a' style='height:60px'>Block C</div></div>");
			jsInit += ("    <div class='ui-block-a'><div class='ui-bar ui-bar-a' style='height:60px'>Block A</div></div>");
			jsInit += ("    <div class='ui-block-b'><div class='ui-bar ui-bar-a' style='height:60px'>Block B</div></div>");
			jsInit += ("    <div class='ui-block-c'><div class='ui-bar ui-bar-a' style='height:60px'>Block C</div></div>");
			jsInit += ("</div>");

			return jsInit;
		}

		static string GetGridColJSString(Dictionary<string, string> d) {
			int curr = 0;
			string tmp = "";
			foreach (KeyValuePair<string, string> p in d) {
				if (curr > 0 && curr < d.Count)
					tmp += ", ";
				tmp += p.Key + ": " + p.Value;
				curr++;
			}
			return "{ " + tmp + " } ";
		}

		private static Dictionary<string, string> BuildDictionaryForColProps(GridColumnBuilder c, int order,
			HtmlHelper htmlHelper){
			
			var d = new Dictionary<string, string>();
			
			if (!String.IsNullOrEmpty(c.GetTitle()))
				d.Add("sTitle", c.GetTitle().QuotedString());
			if (!String.IsNullOrEmpty(c.GetDataField()))
				d.Add("sDataField", c.GetDataField().QuotedString());

			return d;
		}

		public static string QuotedString(this string str) {
			return "'" + str + "'";
		}
	}

	public class GridBuilder{
	}

	public class GridColumns {

		public GridColumns() {
			Columns = new List<GridColumnBuilder>();
		}

		public GridColumns(int precisionQuantity, int precisionPrice, int precisionPercent): this() {
			PrecisionPercent = precisionPercent;
			PrecisionPrice = precisionPrice;
			PrecisionQuantity = precisionQuantity;
		}

		protected IList<GridColumnBuilder> Columns { get; private set; }
		protected int PrecisionQuantity { get; set; }
		protected int PrecisionPrice { get; set; }
		protected int PrecisionPercent { get; set; }

		public GridColumnBuilder Add(string title = "") {
			var column = new GridColumnBuilder();
			column.Title(title);
			Columns.Add(column);
			return column;
		}

		public IList<GridColumnBuilder> GetList() {
			return Columns;
		}
		
	}

	public class GridColumns<TModel> : GridColumns{
		HtmlHelper<TModel> htmlHelper;
		
		public GridColumns (HtmlHelper<TModel> htmlHelper) {
			this.htmlHelper = htmlHelper;
		}

		public GridColumnBuilder Add<TProperty>(Expression<Func<TModel, TProperty>> expression, string title = "") {
			ModelMetadata model = MvcUtility.GetModelMetadata(expression, htmlHelper.ViewData);
			if (model == null)
				throw new ArgumentNullException("The model is null");

			GridColumnBuilder c = Add(title);
			c.DataField(model.PropertyName);
			c.DataType(this.GetGridTypeOfModel(model.ModelType));
			return c;
		}

		public GridColumnBuilder Add<TProperty, TPropertySort>(Expression<Func<TModel, TProperty>> expression, Expression<Func<TModel, TPropertySort>> expressionSort, string title = "") {
			ModelMetadata model = MvcUtility.GetModelMetadata(expression, htmlHelper.ViewData);
			if (model == null)
				throw new ArgumentNullException("The model is null");

			ModelMetadata modelSort = MvcUtility.GetModelMetadata(expressionSort, htmlHelper.ViewData);
			if (modelSort == null)
				throw new ArgumentNullException("The model is null");

			GridColumnBuilder c = this.Add(title);
			c.DataField(model.PropertyName);
			c.DataType(this.GetGridTypeOfModel(model.ModelType));
			return c;
		}

		GridColumnDataType GetGridTypeOfModel(Type modelType) {
			//if (TypeHelper.HerdaDeOuEhNullableQueHerdaDe(modelType, typeof(DateTime)))
			//	return GridColumnDataType.Date;

			//if (TypeHelper.HerdaDeOuEhNullableQueHerdaDe(modelType, typeof(bool)))
			//	return GridColumnDataType.Boolean;

			//if (TypeHelper.EhAlgumTipoDeInteiro(modelType, true))
			//	return GridColumnDataType.Integer;

			//if (TypeHelper.EhAlgumTipoDeNumero(modelType, true))
			//	return GridColumnDataType.Quantity;

			return GridColumnDataType.String;
		}
	}

	public class GridColumnBuilder{
		string title;
		string name;
		bool active = true;
		string dataField;
		GridColumnDataType dataType;

		/// <summary>
		/// Define o título da Coluna. Pode ser passado também pelo método Add de MEGridColumnsBuilder
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public GridColumnBuilder Title(string t) {
			name = t;
			title = t;
			return this;
		}

		/// <summary>
		/// Define se a coluna está ativa. Ativa "true", significa que a coluna será exibida.
		/// O Default é "true"
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public GridColumnBuilder Active(bool a) {
			active = a;
			return this;
		}

		/// <summary>
		/// Define o nome da propriedade que representa o valor desta coluna
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public GridColumnBuilder DataField(string d) {
			dataField = d;
			return this;
		}

		public GridColumnBuilder DataType(GridColumnDataType d) {
			dataType = d;
			return this;
		}

		public string GetTitle() {
			return title;
		}
		public string GetDataField() {
			return dataField;
		}
		public GridColumnDataType GetType() {
			return dataType;
		}
	}

	public static class MvcUtility{
		private static Regex _stripIndexerRegex = new Regex(@"\[(?<index>\d+)\]", RegexOptions.Compiled);

		public static ModelMetadata GetModelMetadata<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression, ViewDataDictionary<TModel> viewData) {
			return ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, viewData);
		}
	}

	public class GridResult : ActionResult {

		public IEnumerable GridData {
			get;
			set;
		}

		public GridResult(IEnumerable GridData) {
			this.GridData = GridData;
		}

		public IGridResultExporter GetGridExporter(ResultType type) {
			IGridResultExporter res = null;
			if (type == ResultType.Json)
				res = new JsonGridExporter();
			else if (type == ResultType.Excel)
				res = new ExcelGridExporter();
			else if (type == ResultType.XML)
				res = new XMLGridExporter();
			else if (type == ResultType.PDF)
				res = new PDFGridExporter();
			return res;
		}

		public override void ExecuteResult(ControllerContext context) {
			var type = ResultType.Json;
			try {
				type = Enumerator.IntToEnum<ResultType>(Convert.ToInt32(context.RequestContext.HttpContext.Request["GridFormat"]));
			} catch {
				type = ResultType.Json;
			}
			string gridProperties = context.RequestContext.HttpContext.Request["GridProperties"];
			GridPropertiesVO p = null;
			if (!String.IsNullOrEmpty(gridProperties))
				p = JsonConvert.DeserializeObject<GridPropertiesVO>(gridProperties);

			IGridResultExporter exporter = GetGridExporter(type);
			exporter.ExportGrid(context, GridData, p);
		}

	}

	public class GridPropertiesVO {
		private List<GridPropertiesColumnVO> _aoColumns = new List<GridPropertiesColumnVO>();
		public List<GridPropertiesColumnVO> aoColumns {
			get { return _aoColumns; }
			set { _aoColumns = value; }
		}
	}

	public class GridPropertiesColumnVO {
		public string sTitle {
			get;
			set;
		}

		public string sDataField {
			get;
			set;
		}

		public bool bActive {
			get;
			set;
		}

		public bool bSortable {
			get;
			set;
		}

		public string sTooltip {
			get;
			set;
		}

		public string sDateTimeFormat {
			get;
			set;
		}

		public string sDataType {
			get;
			set;
		}

		public string sDecimalSeparator {
			get;
			set;
		}

		public string sThousandSeparator {
			get;
			set;
		}

		public int iDecimalPlaces {
			get;
			set;
		}

		public int iOrder {
			get;
			set;
		}
	}

	public class JsonGridExporter : IGridResultExporter {
		public void ExportGrid(ControllerContext context, IEnumerable GridData, GridPropertiesVO GridProperties) {
			string json = JsonConvert.SerializeObject(GridData, new JavaScriptDateTimeConverter());
			context.HttpContext.Response.ContentType = "application/json";
			context.HttpContext.Response.Write(json);
		}
	}

	public class ExcelGridExporter : IGridResultExporter {
		private void ExportExcel(ControllerContext context, IEnumerable GridData, GridPropertiesVO GridProperties) {
			IWorkbook book = NativeExcel.Factory.CreateWorkbook();
			IWorksheet sheet = book.Worksheets.Add();
			sheet.Name = "Dados";

			int currCol = 1;
			foreach (GridPropertiesColumnVO c in GridProperties.aoColumns.OrderBy(c => c.iOrder)) {
				if (!c.bActive)
					continue;
				sheet.Cells[1, currCol].Value = c.sTitle;
				sheet.Cells[1, currCol].Font.Bold = true;
				currCol++;
			}
			int currRow = 2;
			foreach (object o in GridData) {
				Type objType = o.GetType();
				currCol = 1;
				foreach (GridPropertiesColumnVO c in GridProperties.aoColumns.OrderBy(c => c.iOrder)) {
					if (!c.bActive)
						continue;
					if (String.IsNullOrEmpty(c.sDataField))
						continue;
					if (objType.GetProperty(c.sDataField) == null)
						continue;

					object value = objType.GetProperty(c.sDataField).GetValue(o, null);
					if (value != null)
						sheet.Cells[currRow, currCol].Value = value.ToString();

					currCol++;
				}
				currRow++;
			}

			sheet.Cells.Columns.Autofit();

			book.SaveAs(context.HttpContext.Response.OutputStream);
		}

		public void ExportGrid(ControllerContext context, IEnumerable GridData, GridPropertiesVO GridProperties) {
			context.HttpContext.Response.Clear();
			HttpHeadersHelper.downloadFile(HttpContext.Current.Response, "MEGridExport.xls", 0);

			context.HttpContext.Response.ContentType = "application/vnd.ms-excel";
			context.HttpContext.Response.AddHeader("Content-Type", "application/vnd.ms-excel");
			//context.HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=MEGridExport.xls");
			context.HttpContext.Response.Flush();

			ExportExcel(context, GridData, GridProperties);

			context.HttpContext.Response.End();
		}
	}

	public class XMLGridExporter : IGridResultExporter {
		public void ExportGrid(System.Web.Mvc.ControllerContext context, IEnumerable GridData, GridPropertiesVO GridProperties) {
			HttpHeadersHelper.downloadFile(HttpContext.Current.Response, "GridExport.xml", 0);

			Type extraType = null;
			if (GridData != null) {
				foreach (object item in GridData) {
					extraType = item.GetType();
					break;
				}
			}

			XmlSerializer ser = null;
			if (extraType == null)
				ser = new XmlSerializer(GridData.GetType());
			else
				ser = new XmlSerializer(GridData.GetType(), new Type[] { extraType });
			ser.Serialize(context.HttpContext.Response.OutputStream, GridData);
			context.HttpContext.Response.End();
		}
	}

	public class PDFGridExporter : IGridResultExporter {
		public void ExportGrid(System.Web.Mvc.ControllerContext context, IEnumerable GridData, GridPropertiesVO GridProperties) {
			HttpHeadersHelper.downloadFile(HttpContext.Current.Response, "GridExport.pdf", 0);
			//Document pdfDocument = new Document(iTextSharp.text.PageSize.A4);
			//PdfWriter.GetInstance(pdfDocument, context.HttpContext.Response.OutputStream);
			//pdfDocument.Open();

			//Image logo = Image.GetInstance(HTMLImageRenderer.getPhysicalPath(MEAssortedImage.LOGO_ME_NOVO_PEQ));
			//Paragraph headerPara = new Paragraph();
			//headerPara.Add(logo);
			//headerPara.Add(
			//		new Chunk(DateTime.Now.ToLongDateString(),
			//							FontFactory.GetFont(FontFactory.TIMES_ROMAN, 16, iTextSharp.text.Font.BOLD)));
			//headerPara.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
			//pdfDocument.Add(headerPara);

			//foreach (object o in GridData) {
			//	pdfDocument.Add(new Paragraph("__________________________________________________________________"));

			//	foreach (GridPropertiesColumnVO column in GridProperties.aoColumns) {
			//		if (!column.bActive)
			//			continue;

			//		if (String.IsNullOrEmpty(column.sDataField))
			//			continue;

			//		PropertyInfo oProperty = o.GetType().GetProperty(column.sDataField);

			//		if (oProperty == null)
			//			continue;

			//		Paragraph para = new Paragraph();
			//		para.Add(
			//			new Chunk(column.sTitle + ": ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));

			//		object value = oProperty.GetValue(o, null);
			//		if (value != null) {
			//			para.Add(
			//				new Chunk(value.ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12)));
			//		}
			//		pdfDocument.Add(para);
			//	}
			//}
			//pdfDocument.Close();
			context.HttpContext.Response.End();
		}
	}

	public interface IGridResultExporter {
		void ExportGrid(ControllerContext context, IEnumerable GridData, GridPropertiesVO GridProperties);
	}

	public class HttpHeadersHelper {

		public static string getContentType(string fileName) {
			/* Não testa mais o type. Download é *sempre* octet-stream.
			string extensaoArquivo = Path.GetExtension(fileName).ToLower();
			switch (extensaoArquivo) {
				case ".xls":
					return "application/vnd.ms-excel";
				case ".zip":
					return "application/octet-stream"; // tem que ser esse por causa do HTTP Compression no IE
				default:
					return "application/octet-stream";
			}
			*/
			return "application/octet-stream";
		}

		/// <summary>
		/// Preparar os "HTTP Response headers" para um "download".
		/// </summary>
		/// <param name="httpResponse">HTTP Response</param>
		/// <param name="fileName">Nome simples do arquivo a baixar</param>
		/// <param name="fileSize">Tamanho em "bytes" do arquivo a baixar</param>
		public static void downloadFile(HttpResponse httpResponse, string fileName, long fileSize) {
			httpResponse.Clear();
			httpResponse.ClearHeaders();

			//Não posso mandar o no-cache porque o download no IE via HTTPS não funciona (ver "http://support.microsoft.com/kb/2549423")
			httpResponse.Cache.SetLastModified(DateTime.Now);

			//httpResponse.Charset = System.Text.UTF8Encoding.UTF8.WebName;
			//httpResponse.Charset = "utf-8";
			//httpResponse.ContentEncoding = System.Text.UTF8Encoding.UTF8;

			httpResponse.AddHeader("Pragma", "public");
			httpResponse.AddHeader("Content-Transfer-Encoding", "binary");
			httpResponse.AddHeader("Content-Description", "File Transfer");
			httpResponse.AddHeader("Content-Type", "application/octet-stream");

			// Vamos tratar o fileName. Não pode ser vazio, nem conter espaços.
			if (String.IsNullOrEmpty(fileName)) fileName = "download.txt";
			fileName = fileName.Replace(" ", "_");
			//#0216029-MSIE v7-9 têm "bug" se não fizermos isto
			// (ver http://forums.asp.net/t/1332340.aspx/1)
			// Nota: MSIE v1-6 não devem ser usadas, pois têm vários "bugs"
			//       relativos ao "Content-Disposition HTTP header":
			if (HttpContext.Current.Request.Browser.Browser == "IE") {
				fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
			}

			if (fileSize > 0) {
				httpResponse.AddHeader("Content-Length", fileSize.ToString());
				httpResponse.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "; size=" + fileSize.ToString());
			} else {
				httpResponse.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
			}
		}

		/// <summary>
		/// Preparar os "HTTP Response headers" para um "download".
		/// </summary>
		/// <param name="httpResponse">HTTP Response</param>
		/// <param name="fileName">Nome simples do arquivo a baixar</param>
		/// <param name="fileSize">Tamanho em "bytes" do arquivo a baixar</param>
		public static void downloadFileBase(HttpResponseBase httpResponse, string fileName, long fileSize) {
			httpResponse.Clear();
			httpResponse.ClearHeaders();

			//Não posso mandar o no-cache porque o download no IE via HTTPS não funciona (ver "http://support.microsoft.com/kb/2549423")
			httpResponse.Cache.SetLastModified(DateTime.Now);

			//httpResponse.Charset = System.Text.UTF8Encoding.UTF8.WebName;
			//httpResponse.Charset = "utf-8";
			//httpResponse.ContentEncoding = System.Text.UTF8Encoding.UTF8;

			httpResponse.AddHeader("Pragma", "public");
			httpResponse.AddHeader("Content-Transfer-Encoding", "binary");
			httpResponse.AddHeader("Content-Description", "File Transfer");
			httpResponse.AddHeader("Content-Type", "application/octet-stream");

			// Vamos tratar o fileName. Não pode ser vazio, nem conter espaços.
			if (String.IsNullOrEmpty(fileName)) fileName = "download.txt";
			fileName = fileName.Replace(" ", "_");
			//#0216029-MSIE v7-9 têm "bug" se não fizermos isto
			// (ver http://forums.asp.net/t/1332340.aspx/1)
			// Nota: MSIE v1-6 não devem ser usadas, pois têm vários "bugs"
			//       relativos ao "Content-Disposition HTTP header":
			if (HttpContext.Current.Request.Browser.Browser == "IE") {
				fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
			}

			if (fileSize > 0) {
				httpResponse.AddHeader("Content-Length", fileSize.ToString());
				httpResponse.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "; size=" + fileSize.ToString());
			} else {
				httpResponse.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
			}
		}

		/// <summary>
		/// Lê o Content como URL Encoded e troca quebra de linha por &
		/// </summary>
		/// <param name="httpRequest">HTTP Request</param>
		/// <returns>Lista de variáveis</returns>
		public static NameValueCollection getContentAsUrlEncoded(HttpRequest httpRequest) {
			string contentString = "";
			using (StreamReader streamRequest = new StreamReader(httpRequest.InputStream)) {
				contentString = streamRequest.ReadToEnd();
			}
			contentString = contentString.Replace("\r\n", "&");
			return HttpUtility.ParseQueryString(contentString);
		}

		/// <summary>
		/// Converte todo caracter (exceto de letras e número) para ASCII Hexadecimal precedido de %
		/// </summary>
		/// <param name="valor">String a ser convertida</param>
		/// <returns>String como URLEncoded</returns>
		public static string getUrlEncoded(string valor) {
			string urlEncoded = "";
			Regex re = new Regex("^[a-zA-Z0-9]$"); // letras ou números
			for (int i = 0;i < valor.Length;i++) {
				if (re.IsMatch(valor[i].ToString()))
					urlEncoded += valor[i].ToString();
				else
					urlEncoded += "%" + (((byte)valor[i] >= 10 && (byte)valor[i] <= 15) ? "0" : "") + ((byte)valor[i]).ToString("X");
			}
			return urlEncoded;
		}
	}


}