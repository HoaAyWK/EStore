namespace EStore.Api.Common.Contexts;

public interface IWorkContextSource
{
    Guid? GetCurrentCustomerId();
    Guid CreateGuestId();
    void AppendCookies(string cookieName, Guid customerId, int expireMinutes = 3600);
    void RemoveCookies(string cookieName);
}
