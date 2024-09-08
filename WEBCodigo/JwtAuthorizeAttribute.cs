using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class JwtAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var token = filterContext.HttpContext.Session.GetString("JwtToken");

        if (string.IsNullOrEmpty(token))
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Account", action = "Login" })
            );
        }

        base.OnActionExecuting(filterContext);
    }
}



