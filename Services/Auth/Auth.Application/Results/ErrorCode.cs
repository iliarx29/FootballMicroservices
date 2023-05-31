using System.ComponentModel;

namespace Auth.Application.Results;
public enum ErrorCode
{
    [Description("Resource not found")]
    NotFound = 1,

    [Description("User already exists")]
    AlreadyExist,

    [Description("Bad request")]
    BadRequest
}

public static class EnumExtensions
{
    public static string GetDescription(this Enum @enum)
    {
        var field = @enum.GetType().GetField(@enum.ToString());

        if (field is null)
            throw new ArgumentException("Description is null");

        DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;

        return @enum.ToString();
    }
}