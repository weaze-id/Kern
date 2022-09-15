namespace Kern.Authorization.Metadatas;

public class AudienceMetadata
{
    public AudienceMetadata(string? audience = null)
    {
        Audience = audience;
    }

    public string? Audience { get; }
}