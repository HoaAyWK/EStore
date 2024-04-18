namespace EStore.Api.Common.Contexts;

public class WorkContext : IWorkContext
{
    private readonly IWorkContextSource _workContextSource;
    private Guid? _customerId;

    public WorkContext(IWorkContextSource workContextSource)
    {
        _workContextSource = workContextSource;
    }

    public Guid CustomerId
    {
        get
        {
            if (_customerId is null)
            {
                var customerId = _workContextSource.GetCurrentCustomerId();

                if (customerId is null)
                {
                    var guestId = _workContextSource.CreateGuestId();                    

                    _workContextSource.AppendCookies(Constants.Cookies.Guest, guestId);
                    _customerId = guestId;

                    return guestId;
                }

                _customerId = customerId;

                return customerId.Value;
            }

            return _customerId.Value;
        }
    }
}
