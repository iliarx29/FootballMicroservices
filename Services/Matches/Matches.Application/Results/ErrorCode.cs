using System.ComponentModel;

namespace Matches.Application.Result;

public enum ErrorCode
{
    [Description("Bad request")]
    BadRequest = 400,

    [Description("Forbidden action")]
    Forbidden = 403,

    [Description("OK")]
    OK = 200,

    [Description("Server error")]
    InternalServerError = 500,

    [Description("No content")]
    NoContent = 204,

    [Description("Resourse not found")]
    NotFound = 404
}

public static class EnumExtensions
{
    public static string GetDescription(this Enum @enum)
    {
        var fi = @enum.GetType().GetField(@enum.ToString());

        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
        else
            return @enum.ToString();
    }
}