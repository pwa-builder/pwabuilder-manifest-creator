using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PWABuilder.ManifestCreator
{
    public class HttpClientIgnoringSslErrors : HttpClient
    {
        public HttpClientIgnoringSslErrors()
            : base(new HttpClientHandler
            {
                // Don't worry about HTTPS errors
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                AllowAutoRedirect = true
            })
        {
        }
    }
}
