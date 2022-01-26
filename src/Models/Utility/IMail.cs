namespace Snap.Genshin.Website.Models.Utility
{
    public interface IMail
    {
        string Title { get; }
        string Content { get; }
        string Destination { get; }
        string Sender { get; }
    }
}
