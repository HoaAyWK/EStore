namespace EStore.Api.Common.ApiRoutes;

public static partial class ApiRoutes
{
    public static class Product
    {
        public const string Get = "{id:guid}";

        public const string AddImages = "{id:guid}/images";

        public const string Update = "{id:guid}";

        public const string AddAttribute = "{id:guid}/attributes";

        public const string UpdateAttribute = "{id:guid}/attributes/{attributeId:guid}";

        public const string AddAttributeValue = "{id:guid}/attributes/{attributeId:guid}/values";

        public const string UpdateAttributeValue = "{id:guid}/attributes/{attributeId:guid}/values/{attributeValueId:guid}";

        public const string DeleteAttributeValue = "{id:guid}/attributes/{attributeId:guid}/values/{attributeValueId:guid}";

        public const string AddVariant = "{id:guid}/variants";

        public const string UpdateVariant = "{id:guid}/variants/{productVariantId:guid}";

        public const string Delete = "{id:guid}";
    }
}
