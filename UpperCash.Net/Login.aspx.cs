using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Security;

namespace UpperCash.Net {
	public partial class Login : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			var connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString + ";Application Name=UpperCash - Login";

			#region oAuth Twitter
			//string consumerKey = ConfigurationManager.AppSettings["consumerKey"];
			//string consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];

			//string oauth_Token = Request.QueryString["oauth_token"];
			//var acessToken = OAuthUtility.GetAccessToken(consumerKey, consumerSecret, oauth_Token, "");

			//Response.Write("Token: " + acessToken.Token + " UserId: " + acessToken.UserId);
			#endregion

			var statusSistema = ConfigurationManager.AppSettings["Status"];
			if (String.IsNullOrEmpty(statusSistema) || statusSistema != "Instalado") {
				Response.Redirect("Instalar.aspx");
			}

			if (IsPostBack){
				var usuarioBo = new Business.Usuario(connectionString);
				var usuario = usuarioBo.AutenticaUsuario(user.Value, pass.Value);

				if ((!string.IsNullOrEmpty(user.Value)) && (!string.IsNullOrEmpty(pass.Value)) && usuario != null){
					HttpCookie c = Request.Cookies["UpperCash"] ?? new HttpCookie("UpperCash");
					c.Values["UpperCashID"] = usuario.ID.ToString(CultureInfo.InvariantCulture);
					Response.SetCookie(c);
					FormsAuthentication.RedirectFromLoginPage(user.Value, false);
				}else{
					msgErro.InnerHtml = "<table width='700px' cellspacing='0' cellpadding='0' border='0'>" +
															"	<tbody><tr><td>" +
															"		<table width='100%' cellspacing='0' cellpadding='0' bordercolor='#CC3300' border='1' style='border-collapse: collapse'>" +
															"			<tbody><tr><td>" +
															"				<table width='100%' cellspacing='0' cellpadding='10' bordercolor='#E4A7A5' border='1' bgcolor='#d88581' style='border-collapse: collapse'>" +
															"					<tbody><tr><td>" +
															"						<p align='center'><b><font size='1' face='Verdana' color='#692421'>Usuário ou Senha incorreto!</font></b></p>" +
															"					</td></tr></tbody>" +
															"				</table>" +
															"			</td></tr></tbody>" +
															"		</table>" +
															"	</td></tr><tr><td height='10'></td></tr></tbody>" +
															"</table>" +
															"<script>" +
															"$('#msgErro').slideDown('normal'); window.setTimeout( function(){ $('#msgErro').slideUp('slow'); } , 3000);" +
															"</script>";
				}
			}

		}
	}
}