namespace Snap.HutaoAPI.Models.Utility
{
    public interface IMail
    {
        string Title { get; }
        string Content { get; }
        string Destination { get; }
        string Sender { get; }
    }
}
