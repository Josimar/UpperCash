using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using UpperCash.Entities;


namespace UpperCash.Net {
	public class ControlUser : UCPage {
		public new VisitorVO Visitor {
			get { return (VisitorVO)base.Visitor; }
		}

		protected override void OnPreLoad(EventArgs e) {
			checkUser();

			base.OnPreLoad(e);
		}

		protected virtual void checkUser() {
			if (Visitor == null || !Visitor.isUsuarioLogado) {
				Response.Redirect("Login.aspx");
			}
		}

		public int getCurrentUserID() {
			return Visitor.ID;
		}
	}

	public class UCWebMasterPage : MasterPage {
	}

	public class UCPageBase : Page {

		public IBVisitor Visitor {
			get {
				var authentication = new Authentication();
				var result = authentication.obterVisitor() ?? new EmptyVisitorVO();
				return result;
			}
			set {
				if (value == null) throw new ArgumentNullException("value");
				var sessionManager = new UCSessionManager(HttpContext.Current.Session);
				sessionManager.SetSessionData(UCSessionManager.SessionManagerData.VISITOR, Visitor);
			}
		}
	}

	public class UCPage : UCPageBase {
		public UCWebMasterPage MasterPage {
			get {
				MasterPage theMaster = Master;
				if (theMaster is UCWebMasterPage) {
					return Master as UCWebMasterPage;
				}
				return null;
			}
		}

		private UCSessionManager _sessionManager;
		public UCSessionManager SessionManager {
			get { return _sessionManager ?? (_sessionManager = new UCSessionManager(System.Web.HttpContext.Current.Session)); }
		}

		protected override void OnPreLoad(EventArgs e) {
			if ((Request.QueryString["isCalledFromPopupBusca"] ?? "") != "") {
				if (MasterPage != null)
					MasterPage.isPopup = true;
			}

			if (MasterPage != null) {
				if (Request.QueryString["superCleanPage"] == "true") {
					MasterPage.isPopup = true;
				}
				if (Request.QueryString["isPopup"] == "true") {
					MasterPage.isPopup = true;
				}
			}
			base.OnPreLoad(e);
			if (!checkCanShowPage()) {
				Response.Redirect("Default.aspx");
			}
		}

		protected virtual bool checkCanShowPage() {
			return true;
		}

		protected override void OnPreInit(EventArgs e) {
			base.OnPreInit(e);

			var theMaster = Master as UCWebMasterPage;
			if (theMaster != null) {
				theMaster.Visitor = Visitor;
				theMaster.UCPage = this;
			}
		}

		public class UCWebMasterPage : MasterPage {
			protected IBVisitor _visitor;
			public bool isPopup { get; set; }
			public IBVisitor Visitor {
				get { return _visitor; }
				set { _visitor = value; }
			}

			public UCPage UCPage { get; set; }
		}
	}

	[Serializable]
	public class VisitorVO : IBVisitor {
		private UsuarioVO _usuarioLogado;
		public UsuarioVO usuarioLogado {
			get { return _usuarioLogado; }
			set { _usuarioLogado = value; }
		}

		public DateTime ultimoAcesso { get; private set; }

		public bool isUsuarioLogado {
			get { return usuarioLogado != null; }
		}

		public int getID() {
			return _usuarioLogado == null ? 0 : _usuarioLogado.ID;
		}

		public int ID {
			get { return getID(); }
		}

		public string Nome {
			get { return _usuarioLogado.Nome; }
		}

		public string header { get; private set; }
		public string logotipo { get; private set; }
		public string tema { get; private set; }
	}

	public class EmptyVisitorVO : IBVisitor {
		public int ID { get; private set; }
		public string Nome { get; private set; }
		public DateTime ultimoAcesso { get; private set; }
		public bool isUsuarioLogado { get; private set; }
		public string header { get; private set; }
		public string logotipo { get; private set; }
		public string tema { get; private set; }
	}

	public class UCSessionManager {
		public enum SessionManagerData {
			HAS_VISITOR,
			VISITOR,
			ME_SITE_MAP,
			ME_SITE_MAP_HASH,
			ME_SITE_MAP_CULTURE
		}

		private readonly HttpSessionState _session;

		public UCSessionManager(HttpSessionState Session) {
			_session = Session;
		}

		public static bool HerdaDe(Type t, Type comparetype) {
			if (t == comparetype)
				return true;

			if (t.BaseType == null || t.BaseType == typeof(object))
				return false;

			return HerdaDe(t.BaseType, comparetype);
		}

		public SessionDataType GetSessionData<SessionDataType>(SessionManagerData SessionData) {
			if (_session == null)
				return default(SessionDataType);
			object o = _session[SessionData.GetType().FullName + "." + SessionData];
			//Fazendo os bool retornarem false e não null + Exception de null reference.
			if (HerdaDe(typeof(SessionDataType), typeof(bool)))
				o = o ?? false;
			return (SessionDataType)o;
		}

		public void SetSessionData(SessionManagerData SessionData, object Data) {
			if (_session == null)
				return;
			validateObject(Data);
			_session[SessionData.GetType().FullName + "." + SessionData] = Data;
		}

		public void RemoveSessionData(SessionManagerData SessionData) {
			_session.Remove(SessionData.GetType().Name + "." + SessionData);
			_session.Remove(SessionData.GetType().FullName + "." + SessionData);
		}

		private void validateObject(object Data) {
			if (Data != null && !Data.GetType().IsSerializable)
				throw new Exception(String.Format("{0} não é serializável, não pode ser gravada na sessão.", Data.GetType().FullName));
		}
	}

	public class UCCookies {

		/// <summary>
		/// Seta um cookie
		/// </summary>
		/// <param name="Request">O HttpRequest</param>
		/// <param name="Response">O HttpResponse</param>
		/// <param name="CookName">Nome do Cookie que será setado</param>
		/// <param name="CookValue">Valor do Cookie que será setado</param>
		public static void SetCookie(HttpRequest Request, HttpResponse Response, string CookName, string CookValue) {
			HttpCookie c = Request.Cookies["UpperCash"] ?? new HttpCookie("UpperCash");

			c.Values[CookName] = CookValue;

			Response.SetCookie(c);
		}

		private static string GetCookie(HttpCookieCollection cookies, string cookieName) {
			HttpCookie c = cookies["UpperCash"];

			if (c == null)
				return "";
			return c[cookieName];
		}

		public static int GetIntCookie(HttpCookieCollection cookies, string cookName) {
			string tmp = GetCookie(cookies, cookName);
			if (string.IsNullOrEmpty(tmp))
				return 0;

			return Convert.ToInt32(tmp);
		}

		public static void EatCookie(HttpRequest Request, HttpResponse Response) {
			Request.Cookies.Remove("UpperCash");
		}
	}

	[Serializable]
	public class UCException : Exception {
		private string _message;
		public override string Message {
			get { return _message; }
		}

		public UCException(string Message)
			: base(Message) {
			_message = Message;
		}
	}

	[Serializable]
	public class UCNotLoggedInException : UCException {
		public UCNotLoggedInException(string message)
			: base(message) {
		}
	}

	public class Authentication {
		private string _connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString + ";Application Name=UpperCash - Visitor";

		public IBVisitor obterVisitor() {
			var sessionManager = new UCSessionManager(HttpContext.Current.Session);

			int ucid = UCCookies.GetIntCookie(HttpContext.Current.Request.Cookies, "UpperCashID");

			VisitorVO visitor = getVisitorFromSession(sessionManager, ucid);

			if (visitor != null)
				return visitor;

			if (ucid > 0) {
				visitor = LoadUser(ucid);

				sessionManager.SetSessionData(UCSessionManager.SessionManagerData.VISITOR, visitor);
				sessionManager.SetSessionData(UCSessionManager.SessionManagerData.HAS_VISITOR, true);
				return visitor;
			}

			throw new UCNotLoggedInException("User not loggedIn");
		}

		public VisitorVO LoadUser(int ID) {
			var result = new VisitorVO();

			result.usuarioLogado = getUsuarioVOByID(ID, false);
			if (result.usuarioLogado == null)
				throw new Exception("Erro ao obter usuário");

			return result;
		}

		public UsuarioVO getUsuarioVOByID(int ID, bool cacheRelated) {
			var usuarioBO = new Business.Usuario(_connectionString);
			Usuario u = usuarioBO.LoadByPk(ID);
			if (u == null)
				return null;
			var vo = new UsuarioVO();
			UCBase.copyTo(u, vo, false, false);

			return vo;
		}

		private VisitorVO getVisitorFromSession(UCSessionManager sessionManager, int dsid) {
			if (!sessionManager.GetSessionData<bool>(UCSessionManager.SessionManagerData.HAS_VISITOR))
				return null;

			var visitor = sessionManager.GetSessionData<VisitorVO>(UCSessionManager.SessionManagerData.VISITOR);

			if (visitor == null || visitor.ID != dsid)
				return null;

			return visitor;
		}
	}

	public static class UCBase {
		public static void copyTo(this object orig, object dest, bool ignoreNoValueTypes, bool onlyMatchPropertyType) {
			foreach (PropertyInfo pi in orig.GetType().GetProperties()) {
				if (ignoreNoValueTypes && !(pi.PropertyType.IsValueType || pi.PropertyType == typeof(string)))
					continue;
				PropertyInfo propDest;
				if (onlyMatchPropertyType) {
					propDest = (dest.GetType().GetProperty(pi.Name, pi.PropertyType) ??
											dest.GetType().GetProperty(pi.Name.ToUpper(), pi.PropertyType)) ??
										 dest.GetType().GetProperty(pi.Name.ToLower(), pi.PropertyType);
				} else {
					propDest = (dest.GetType().GetProperty(pi.Name) ?? dest.GetType().GetProperty(pi.Name.ToUpper())) ??
										 dest.GetType().GetProperty(pi.Name.ToLower());
				}
				if (propDest != null) {
					Object value = pi.GetValue(orig, null);
					try {
						propDest.SetValue(dest, value, null);
					} catch {
					}
				}
			}
		}
	}

	public interface IBaseVisitor {
		int ID { get; }
		string Nome { get; }
		DateTime ultimoAcesso { get; }
		bool isUsuarioLogado { get; }
	}

	public interface IBVisitor : IBaseVisitor {
		string header { get; }
		string logotipo { get; }
		string tema { get; }
	}

}
