namespace EStore.Api.Common.ApiRoutes;

public static partial class ApiRoutes
{
    public static class Category
    {
        public const string Get = "{id:guid}";

        public const string GetBySlug = "slugs/{slug}";

        public const string GetFromParents = "parents";

        public const string GetAll = "all";

        public const string GetTree = "tree";

        public const string Update = "{id:guid}";

        public const string Delete = "{id:guid}";
    }
}
