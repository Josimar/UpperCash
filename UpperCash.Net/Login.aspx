<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UpperCash.Net.Login" %>

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head id="Head1" runat="server">
    <title>UpperCash - Login</title>
		
		<style type='text/css'>
			.campo	{ font-family: arial; font-size: 10pt; color: #666666; background: #FFFFFF; background-image : url( "Content/images/adm.gif" ); border-right:1px solid #666666; border-left:1px solid #666666; border-top:1px solid #666666; border-bottom:1px solid #666666; width:100%; }
			.selec	{ font-family: arial; font-size: 08pt; color: #666666; background: #FFFFFF; background-image : url( "Content/images/adm.gif" ); border-right:1px solid #666666; border-left:1px solid #666666; border-top:1px solid #666666; border-bottom:1px solid #666666; width:100%; }
			.botao	{ font-family: arial; font-size: 09pt; color: #666666; background: #FFFFFF; background-image : url( "Content/images/adm.gif" ); border-right:1px solid #666666; border-left:1px solid #666666; border-top:1px solid #666666; border-bottom:1px solid #666666; }

			.fonte_bold		{ font-family: Verdana; font-size: 10px; color: #333333; font-weight: bold }
			label.error { float: none; color: red; padding-left: .5em; vertical-align: top; }
			
			input[data-val-required], textarea[data-val-required], select[data-val-required], input[required], textarea[required], select[required] {
				border-left: solid 2px #FF6262!important;
			}
			
			input.required, textarea.required, select.required{
				border-left: solid 2px #FF6262!important;
			}
			
			.ipbfs_luser {
					background-image: url("Content/images/user.png");
					background-position: 7px 50%;
					background-repeat: no-repeat;
			}
			.ipbfs_lpassword {
					background-image: url("Content/images/key.png");
					background-position: 7px 50%;
					background-repeat: no-repeat;
			}
			.ipbfs_login_input {
					font-size: 14px;
					margin-top: 10px;
					padding: 6px 0 6px 28px;
					width: 250px;
			}
		</style>

		<script type="text/javascript" language="javascript" src="Content/js/jquery-2.1.0.js"></script>
		<script type="text/javascript" language="javascript" src="Content/js/jquery.validate.js"></script>
		<script type="text/javascript">
		    $(document).ready(function () {
		        $('#formulario').validate({
		            rules: {
		                user: {
		                    required: true
		                },
		                pass: {
		                    required: true
		                },
		                senha: {
		                    required: true
		                }//,
		                //conf_senha: {
		                //	required: true,
		                //	equalTo: "#senha"
		                //},
		                //termos: "required"
		            },
		            messages: {
		                user: {
		                    required: "Nome é obrigatorio."
		                },
		                pass: {
		                    required: "Senha é obrigatorio."
		                },
		                senha: {
		                    required: "O campo senha é obrigatorio."
		                }//,
		                //conf_senha: {
		                //	required: "O campo confirmação de senha é obrigatorio.",
		                //	equalTo: "O campo confirmação de senha deve ser identico ao campo senha."
		                //},
		                //termos: "Para se cadastrar você deve aceitar os termos de uso."
		            }
		        });
		    });
</script>
</head>
<body marginwidth="0" marginheight="0" bgcolor="#015F9D" bottommargin="0" rightmargin="0" leftmargin="0" topmargin="0">
    <form id="formulario" runat="server" method="POST">
		<input type="hidden" name="post" value="ok">
		
		<table border='0' width='100%' cellspacing='0' cellpadding='0' height='95%'>
			<tbody>
			<tr>
				<td>
					<div align='center' style="height: 275px; background-color: #DDDDDD;" background="ds.images/fundologin.gif">
						<table border='0' width='900' cellspacing='0' cellpadding='0' style='border-collapse: collapse'>
							<tr>
								<td valign="middle">
									<table border='0' width='100%' height='100%' cellspacing='25' cellpadding='0'>
										<tr>
											<td align="center">
												<div id="msgErro" runat="server" style="float: left; position: absolute; text-align: center; width: 700px;">

												</div>	
											</td>
										</tr>
										<tr>
											<td>
												<table border='0' width='100%' cellspacing='10' cellpadding='10' height='100%'>
													<tr>
														<td width='520px'>
															<table border="0" id="table20" cellspacing="5" cellpadding="0">
																<tr>
																	<td>
																		<asp:image runat="server" id="logotipo" ImageUrl="Content/images/logotipo.jpg" />
																	</td>
																</tr>
															</table>
														</td>
														<td width="450px">
															<table border='0' width='100%' cellspacing='10' cellpadding='1'>
																<tr>
																	<td colspan="2" width='100%'><div class='fonte_bold'><b>Login:</b><br><input runat="server" name='user' id='user' value='' class="campo required input_text ipbfs_login_input ipbfs_luser" /></div></td>
																</tr>
																<tr>
																	<td colspan="2" width='100%'><div class='fonte_bold'><b>Senha:</b><br><input runat="server" name='pass' id="pass" class="campo required input_text ipbfs_login_input ipbfs_lpassword" type='password' /></div></td>
																</tr>
																<tr>
																	<td width='90%'>&nbsp;</td>
																	<td width='10%'><input type="submit" name="btnLogar" id="btnLogar	" class="botao" value="Entrar" /></td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</div>
				</td>
			</tr>

		</table>

	</form>
</body>
</html>