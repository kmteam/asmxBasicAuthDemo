using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for BasicAuthHttpModule
/// </summary>
public class BasicAuthHttpModule : IHttpModule
{
	public void Dispose()
	{
	}

	public void Init(HttpApplication application)
	{
		application.AuthenticateRequest += new
				EventHandler(this.OnAuthenticateRequest);
		application.EndRequest += new
				EventHandler(this.OnEndRequest);
	}

	public void OnAuthenticateRequest(object source, EventArgs
											eventArgs)
	{
		HttpApplication app = (HttpApplication)source;

		string authHeader = app.Request.Headers["Authorization"];
		if (!string.IsNullOrEmpty(authHeader))
		{
			string authStr = app.Request.Headers["Authorization"];

			if (authStr == null || authStr.Length == 0)
			{
				return;
			}

			authStr = authStr.Trim();
			if (authStr.IndexOf("Basic", 0) != 0)
			{
				return;
			}

			authStr = authStr.Trim();

			string encodedCredentials = authStr.Substring(6);

			byte[] decodedBytes =
			Convert.FromBase64String(encodedCredentials);
			string s = new ASCIIEncoding().GetString(decodedBytes);

			string[] userPass = s.Split(new char[] { ':' });
			string username = userPass[0];
			string password = userPass[1];

			var MyUserValidator = new HardCodedAuthValidation();

			if (!MyUserValidator.Validate(username, password))
			{
				DenyAccess(app);
				return;
			}
		}
		else
		{
			app.Response.StatusCode = 401;
			app.Response.End();
		}
	}
	public void OnEndRequest(object source, EventArgs eventArgs)
	{
		if (HttpContext.Current.Response.StatusCode == 401)
		{
			HttpContext context = HttpContext.Current;
			context.Response.StatusCode = 401;
			context.Response.AddHeader("WWW-Authenticate", "Basic Realm");
		}
	}

	private void DenyAccess(HttpApplication app)
	{
		app.Response.StatusCode = 401;
		app.Response.StatusDescription = "Access Denied";
		app.Response.Write("401 Access Denied");
		app.CompleteRequest();
	}
	public class HardCodedAuthValidation
	{
		public Boolean Validate(String UserName, String password)
		{
			return UserName == "root" && password == "secret12345";
		}
	}
}

