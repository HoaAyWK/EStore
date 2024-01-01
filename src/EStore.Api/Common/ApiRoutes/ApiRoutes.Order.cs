namespace EStore.Api.Common.ApiRoutes;

public static partial class ApiRoutes
{
    public static class Order
    {

        public const string Get = "{id:guid}";

        public const string Refund = "{id:guid}/refund";
    }
}
