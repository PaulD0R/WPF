using System.Security.Cryptography;
using System.Text;

namespace WPFTest.Services
{
    public static class TokenStorageService
    {
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("YourApp-Salt-123");

        public static void SaveRefreshToken(string token)
        {
            byte[] encryptedData = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(token),
                Entropy,
                DataProtectionScope.CurrentUser);

            Properties.Settings.Default.RefreshToken = Convert.ToBase64String(encryptedData);
            Properties.Settings.Default.Save();
        }

        public static string? LoadRefreshToken()
        {
            string encryptedToken = Properties.Settings.Default.RefreshToken;
            if (string.IsNullOrEmpty(encryptedToken))
                return null;

            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedToken),
                    Entropy,
                    DataProtectionScope.CurrentUser);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return null;
            }
        }

        public static void ClearRefreshToken()
        {
            Properties.Settings.Default.RefreshToken = string.Empty;
            Properties.Settings.Default.Save();
        }
    }
}
