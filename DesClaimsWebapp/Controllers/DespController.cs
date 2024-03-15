using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Identity.Web;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using WebApp_OpenIDConnect_DotNet.Models;
using System.Threading.Tasks;

/// <summary>
/// Controller used in web apps to manage accounts.
/// </summary>
/// [AllowAnonymous]
[Area("MicrosoftIdentity")]
[Route("[area]/[controller]/[action]")]
public class DespController : Controller
{

    public IActionResult SignIn([FromRoute] string scheme)
    {
        scheme ??= OpenIdConnectDefaults.AuthenticationScheme;
        var redirectUrl = Url.Content("~/");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        properties.Items["policy"] = "B2C_1A_DESP_LOGIN_RP";
        return Challenge(properties, scheme);
    }

    public IActionResult SignUp([FromRoute] string scheme)
    {
        scheme ??= OpenIdConnectDefaults.AuthenticationScheme;
        var redirectUrl = Url.Content("~/");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        properties.Items["policy"] = "B2C_1A_DESP_REGISTRATION_RP";
        var loginResult= Challenge(properties, scheme);
        return loginResult;
    }

    public IActionResult DespOIDCLoginTest([FromRoute] string scheme)
    {
        scheme ??= OpenIdConnectDefaults.AuthenticationScheme;
        var redirectUrl = Url.Content("~/");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        properties.Items["policy"] = "B2C_1A_DESP_REGISTRATION_RP";
        var loginResult = Challenge(properties, scheme);
        return loginResult;
    }
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        return View(new ErrorViewModel { userMessage = "Something went wrong please try again later" });
    }

}

