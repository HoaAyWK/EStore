namespace EStore.Api.Common.Contexts;

public class WorkContextSource : IWorkContextSource
{
    const string CookiesName = "Guest";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkContextSource(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentCustomerId()
    {
        if (_httpContextAccessor.HttpContext is HttpContext httpContext)
        {
            if (httpContext.User.Identity is not null && httpContext.User.Identity.IsAuthenticated)
            {
                var id = httpContext.User.Identity.Name;

                if (Guid.TryParse(id, out Guid customerFromCtxId))
                {
                    return customerFromCtxId;
                }
            }

            if (httpContext.Request.Cookies.ContainsKey(CookiesName))
            {
                var guestCookie = httpContext.Request.Cookies[CookiesName];

                if (guestCookie is not null && Guid.TryParse(guestCookie, out var customerId))
                {
                    return customerId;
                }
            }

            return null;
        }

        return null;
    }

    public Guid CreateGuestId()
    {
        return Guid.NewGuid();
    }

    public void AppendGuestCookies(Guid guestId)
    {
        if (_httpContextAccessor.HttpContext is HttpContext httpContext)
        {
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddYears(1)
            };

            httpContext.Response.Cookies.Append(CookiesName, guestId.ToString(), cookieOptions);
        }
    }

    public void RemoveGuestCookies()
    {
        if (_httpContextAccessor.HttpContext is HttpContext httpContext &&
            httpContext.Request.Cookies.ContainsKey(CookiesName))
        {
           httpContext.Response.Cookies.Delete(CookiesName);
        }
    }
}
