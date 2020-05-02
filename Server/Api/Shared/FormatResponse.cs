using System.Collections;
using Api.Shared.Exceptions;

namespace Api.Shared
{
    public static class FormatResponse
    {
        public static object Get(object obj)
        {
            switch (obj)
            {
                case ServiceException _: return new { error = obj };
                case IEnumerable _: return new { response = new { array = obj } };
                default: return new { response = new { array = new ArrayList { obj } } };
            }
        }
    }
}
