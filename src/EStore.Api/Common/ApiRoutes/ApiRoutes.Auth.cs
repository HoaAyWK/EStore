namespace EStore.Api.Common.ApiRoutes;

public static partial class ApiRoutes
{
    public static class Auth
    {
        public const string Login = "login";

        public const string Register = "register";

        public const string Logout = "logout";

        public const string SendConfirmationEmail = "send-confirmation-email";

        public const string VerifyEmail = "verify-email";

        public const string ForgetPassword = "forget-password";

        public const string ResetPassword = "reset-password";
    }
}
