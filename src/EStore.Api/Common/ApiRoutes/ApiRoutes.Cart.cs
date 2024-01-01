namespace EStore.Api.Common.ApiRoutes;

public static partial class ApiRoutes
{
    public static class Cart
    {
        public const string GetMyCart = "my";

        public const string RemoveItem = "{id:guid}/items/{itemId:guid}";
    }
}
