using System.Security.Cryptography;
using System.Text;

namespace S.I.A.C.Service
{
    /// <summary>
    /// To apply in the next wipe out.
    /// </summary>
    public class Encrypt
    {
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            foreach (var t in stream)
                sb.AppendFormat("{0:x2}", t);

            return sb.ToString();
        }

    }
}