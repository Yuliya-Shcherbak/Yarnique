using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;
using System.Text;
using Yarnique.Common.Application.Pagination;

namespace Yarnique.API.Configuration.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeNames.Text.Csv);
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(PaginatedResponse<>);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            var objectType = context.Object?.GetType();
            if (objectType != null && objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(PaginatedResponse<>))
            {
                var itemsProperty = objectType.GetProperty("Items");
                if (itemsProperty != null)
                {
                    var items = itemsProperty.GetValue(context.Object) as IEnumerable<object>;

                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            buffer.AppendLine(string.Join(",", item.GetType().GetProperties().Select(p => p.GetValue(item))));
                        }
                    }
                }

                buffer.AppendLine($"Page: {objectType.GetProperty("PageNumber")}, Size: {objectType.GetProperty("PageSize")}");
            }

            await response.WriteAsync(buffer.ToString(), selectedEncoding);
        }
    }
}
