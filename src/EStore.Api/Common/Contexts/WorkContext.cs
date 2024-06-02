namespace EStore.Api.Common.Contexts;

public class WorkContext : IWorkContext
{
    private readonly IWorkContextSource _workContextSource;
    private readonly ILogger<WorkContext> _logger;
    private Guid? _customerId;

    public WorkContext(
        IWorkContextSource workContextSource,
        ILogger<WorkContext> logger)
    {
        _workContextSource = workContextSource;
        _logger = logger;
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

                    _logger.LogInformation("Customer Id is null. Creating a guest id: {GuestId}", guestId);

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
