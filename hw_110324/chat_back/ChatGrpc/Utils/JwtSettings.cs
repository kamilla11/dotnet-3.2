namespace ChatGrpc;

public static class JwtSettings
{
    public const string Issuer = "issuer";
    public const string Audience = "audience";
    public static readonly byte[] Key = "4A485D40-2D3C-42D7-BFFA-13BADE5763C8-4A485D40-2D3C-42D7-BFFA-13BADE5763C8"u8.ToArray();
}