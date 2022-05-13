namespace Snap.HutaoAPI.Configurations
{
    public class SecretManagerConfiguration
    {
        public string SymmetricKey { get; set; } = null!;
        public string SymmetricSalt { get; set; } = null!;
        public string HashSalt { get; set; } = null!;
    }
}
