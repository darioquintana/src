using System;
using System.Web.UI;
using Microsoft.IdentityModel.Claims;
using System.Linq;
namespace HelloAllOfYouQu
{
	public partial class Default : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			var claims = User.Identity as ClaimsIdentity;
			
			claims
				.Claims
				.ToList()
				.ForEach(c => Response.Write("<br>Type: " + c.ClaimType + " ; Value: " + c.Value));
		}
	}
}