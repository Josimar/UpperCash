using System.Text;
using System.Web.Mvc;

namespace UpperCash.Tools {
	public static class LoginExtension {

		public static MvcHtmlString Login(this HtmlHelper htmlHelper){

			var table = new StringBuilder();
			table.AppendLine("<table border='0' width='100%' cellspacing='0' cellpadding='0' height='95%'>" +
			                 "  <tbody>" +
			                 "    <tr>" +
			                 "      <td>" +
			                 "        <div align='center' style='height: 275px; background-color: #DDDDDD;' background='ds.images/fundologin.gif'>" +
			                 "          <table border='0' width='900' cellspacing='0' cellpadding='0' style='border-collapse: collapse'>" +
			                 "            <tr>" +
			                 "              <td valign='middle'>" +
			                 "                <table border='0' width='100%' height='100%' cellspacing='5' cellpadding='0'>" +
			                 "                  <tr>" +
			                 "                    <td align='center'>" +
			                 "                      <div id='msgErro' runat='server' style='float: left; position: absolute; text-align: center; width: 700px;'></div>" +
			                 "                    </td>" +
			                 "                  </tr>" +
			                 "                  <tr>" +
			                 "                    <td>" +
			                 "                      <table border='0' width='100%' cellspacing='10' cellpadding='10' height='100%'>" +
			                 "                        <tr>" +
			                 "                          <td width='520px'>" +
			                 "                            <table border='0' id='table20' cellspacing='5' cellpadding='0'>" +
			                 "                              <tr>" +
			                 "                                <td>" +
			                 "                                  <asp:image runat='server' id='logotipo' ImageUrl='~/ds.images/logoPresencaDigital.jpg' />" +
			                 "                                </td>" +
			                 "                              </tr>" +
			                 "                            </table>" +
			                 "                          </td>" +
			                 "                          <td width='450px'>" +
			                 "                            <table border='0' width='100%' cellspacing='10' cellpadding='1'>" +
			                 "                              <tr>" +
			                 "                                <td colspan='2' width='100%'><div class='fonte_bold'><b>Login:</b><br><input runat='server' name='user' id='user' value='' class='campo required input_text ipbfs_login_input ipbfs_luser' /></div></td>" +
			                 "                              </tr>" +
			                 "                              <tr>" +
			                 "                                <td colspan='2' width='100%'><div class='fonte_bold'><b>Senha:</b><br><input runat='server' name='pass' id='pass' class='campo required input_text ipbfs_login_input ipbfs_lpassword' type='password' /></div></td>" +
			                 "                              </tr>" +
			                 "                              <tr>" +
			                 "                                <td width='90%'>&nbsp;</td>" +
			                 "                                <td width='10%'><input type='submit' name='btnLogar' id='btnLogar' class='botao' value='Entrar' /></td>" +
			                 "                              </tr>" +
			                 "                            </table>" +
			                 "                          </td>" +
			                 "                        </tr>" +
			                 "                      </table>" +
			                 "                    </td>" +
			                 "                  </tr>" +
			                 "                </table>" +
			                 "              </td>" +
			                 "           </tr>" +
			                 "         </table>" +
			                 "       </div>" +
			                 "     </td>" +
			                 "   </tr>" +
			                 " </table>");

			return MvcHtmlString.Create(table.ToString());
		}

	}
}
