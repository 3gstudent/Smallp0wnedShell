using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security;
using System.Text;


namespace p0wnedShell
{
    public static class ExtensionMethods
    {
        public static SecureString ToSecureString(this string src)
        {
            SecureString result = new SecureString();
            src.ToCharArray().ToList().ForEach(c => result.AppendChar(c));
            return result;
        }
    }
}
