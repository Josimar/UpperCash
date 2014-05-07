using System;

namespace UpperCash.Net {
	public partial class _default : ControlUser {
		protected void Page_Load(object sender, EventArgs e){
			var usuario = string.Format("Id: {0} - Nome: {1}", Visitor.ID, Visitor.Nome);

			Response.Write(usuario);

			Response.Write("<br /><br /><a href='logoff.aspx'>LogOff</a>");
		}
	}
}