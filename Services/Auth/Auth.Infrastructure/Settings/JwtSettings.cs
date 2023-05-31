using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Settings;
public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; }
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}
