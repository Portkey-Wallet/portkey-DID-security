namespace CASecurity.Silo.MongoDB;

public class GrainCollectionNameOptions
{
    public Dictionary<string, string> GrainSpecificCollectionName { get; set; } = new();
}