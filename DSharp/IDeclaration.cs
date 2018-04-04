namespace DSharp
{
    public interface IDeclaration : IHasMembers
    {
        string FullName { get; }
    }
}