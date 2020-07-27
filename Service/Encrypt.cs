using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

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
            StringBuilder sb = new StringBuilder();
            byte[] stream = sha256.ComputeHash(encoding.GetBytes(str));
            foreach (var t in stream)
            {
                sb.AppendFormat("{0:x2}", t);
            }

            return sb.ToString();
        }

        public static string Unprotect(string protectedText)
        {
            var protectedBytes = Convert.FromBase64String(protectedText);
            var unprotectedBytes = MachineKey.Unprotect(protectedBytes);
            var unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);
            return unprotectedText;
        }

        public static string Protect(string unprotectedText)
        {
            var unprotectedBytes = Encoding.UTF8.GetBytes(unprotectedText);
            var protectedBytes = MachineKey.Protect(unprotectedBytes);
            var protectedText = Convert.ToBase64String(protectedBytes);
            return protectedText;
        }
    }
}