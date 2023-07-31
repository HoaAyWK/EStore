namespace EStore.Api.Common.Contexts;

public interface IWorkContextSource
{
    Guid? GetCurrentCustomerId();
    Guid CreateGuestId();
    void AppendGuestCookies(Guid guestId);
    void RemoveGuestCookies();
}
