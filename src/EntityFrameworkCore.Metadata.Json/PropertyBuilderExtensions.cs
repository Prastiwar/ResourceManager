using EntityFrameworkCore.Metadata.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore
{
    public static class PropertyBuilderExtensions
    {
        /// <summary> Creates conversion between T and json string. </summary>
        /// <param name="converter"> Custom json converter, if it is null, NewtonsoftJsonConverter is used. </param>
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, IJsonValueConverter<T> converter = null)
            where T : class
        {
            if (converter == null)
            {
                converter = new NewtonsoftJsonConverter<T>();
            }
            propertyBuilder.HasConversion(value => value != null ? converter.Serialize(value) : null,
                                          json => !string.IsNullOrEmpty(json) ? converter.Deserialize(json) : null,
                                          new JsonValueComparer<T>(converter));
            return propertyBuilder;
        }
    }
}
