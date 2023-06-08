using System.Collections;
using Newtonsoft.Json;

namespace EStore.Domain.Common.Collections;

internal class MultiMapJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(MultiMap<,>);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var genericArguments = objectType.GetGenericArguments();
        var keyType = genericArguments[0];
        var valueType = genericArguments[1];
        var sequenceType = typeof(IEnumerable<>)
            .MakeGenericType(typeof(KeyValuePair<,>)
            .MakeGenericType(keyType, typeof(IEnumerable<>)
            .MakeGenericType(valueType)));

        var list = serializer.Deserialize(reader, sequenceType);

        if (list is null)
        {
            return Activator.CreateInstance(objectType, new object[] { });
        }

        if (keyType == typeof(string))
        {
            return Activator.CreateInstance(objectType, new object[] { list, StringComparer.OrdinalIgnoreCase });
        }
        else
        {
            return Activator.CreateInstance(objectType, new object[] { list });
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        {
            var enumerable = value as IEnumerable;

            if (enumerable is not null)
            {
                foreach (var item in enumerable)
                {
                    serializer.Serialize(writer, item);
                }
            }

        }
        writer.WriteEndArray();
    }
}