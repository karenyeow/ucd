using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Comlib.Common.Helpers.Proxy
{
  public  class Proxy : IWebProxy
    {
        public Proxy(string proxyUri)
            : this(new Uri(proxyUri))
        {
        }

        public Proxy(Uri proxyUri)
        {
            ProxyUri = proxyUri;
        }

        public Uri ProxyUri { get; set; }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return ProxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}
