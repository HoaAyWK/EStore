namespace EStore.Api.Common.ApiRoutes;

public static partial class ApiRoutes
{
    public static class Notifications
    {
        public const string MarkAllAsRead = "mark-all-as-read";
        
        public const string MarkAsRead = "{id:guid}/mark-as-read";

        public const string GetMyNotifications = "my";
    }
}
