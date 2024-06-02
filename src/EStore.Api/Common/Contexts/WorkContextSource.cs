namespace EStore.Api.Common.Contexts;

public class WorkContextSource : IWorkContextSource
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<WorkContextSource> _logger;

    public WorkContextSource(
        IHttpContextAccessor httpContextAccessor,
        ILogger<WorkContextSource> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
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
                    _logger.LogInformation("GetCurrentCustomerId - Customer Id from context: {CustomerId}", customerFromCtxId);

                    return customerFromCtxId;
                }
            }

            if (httpContext.Request.Cookies.ContainsKey(Constants.Cookies.Guest))
            {
                var guestCookie = httpContext.Request.Cookies[Constants.Cookies.Guest];

                if (guestCookie is not null && Guid.TryParse(guestCookie, out var customerId))
                {
                    _logger.LogInformation("GetCurrentCustomerId - Customer Id from cookie: {CustomerId}", customerId);
                    return customerId;
                }
            }
        }

        _logger.LogInformation("GetCurrentCustomerId - Customer Id is null.");

        return null;
    }

    public Guid CreateGuestId()
    {
        return Guid.NewGuid();
    }

    public void AppendCookies(string cookieName, Guid customerId, int expireMinutes = 3600)
    {
        if (_httpContextAccessor.HttpContext is HttpContext httpContext)
        {
            var cookieOptions = new CookieOptions
            {
                IsEssential = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                Path = "/",
                HttpOnly = true,
                Secure = true
            };

            httpContext.Response.Cookies.Append(
                cookieName,
                customerId.ToString(),
                cookieOptions);
        }
    }

    public void RemoveCookies(string cookieName)
    {
        if (_httpContextAccessor.HttpContext is HttpContext httpContext &&
            httpContext.Request.Cookies.ContainsKey(cookieName))
        {
           httpContext.Response.Cookies.Delete(cookieName);
        }
    }
}
