namespace EStore.Api.Common.ApiRoutes;

public static partial class ApiRoutes
{
    public static class Customer
    {
        public const string Update = "{id:guid}";

        public const string Get = "{id:guid}";

        public const string AddAddress = "{id:guid}/addresses";

        public const string UpdateAddress = "{id:guid}/addresses/{addressId:guid}";

        public const string GetStats = "stats";

        public const string All = "all";
    }
}
