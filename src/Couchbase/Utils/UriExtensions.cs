using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Couchbase.Core.Configuration.Server;

namespace Couchbase.Utils
{
    public static class UriExtensions
    {
        public static string Http = "http";
        public static string Https = "https";

        public static string QueryPath = "/query";
        public const string AnalyticsPath = "/analytics/service";
        public static string BaseUriFormat = "{0}://{1}:{2}/pools";

        public static Uri GetQueryUri(this IPEndPoint endPoint, Configuration configuration, NodesExt nodesExt)
        {
            if (nodesExt.services.n1ql == 0)
            {
                return new UriBuilder
                {
                    Scheme = configuration.UseSsl ? Https : Http,
                    Host = nodesExt.hostname,
                }.Uri;
            }
            return new UriBuilder
            {
                Scheme = configuration.UseSsl ? Https : Http,
                Host = nodesExt.hostname,
                Port = configuration.UseSsl ? nodesExt.services.n1qlSSL : nodesExt.services.n1ql,
                Path = QueryPath
            }.Uri;
        }

        public static Uri GetAnalyticsUri(this IPEndPoint endPoint, Configuration configuration, NodesExt nodesExt)
        {
            if (nodesExt.services.cbas == 0)
            {
                return new UriBuilder
                {
                    Scheme = configuration.UseSsl ? Https : Http,
                    Host = nodesExt.hostname,
                }.Uri;
            }
            return new UriBuilder
            {
                Scheme = configuration.UseSsl ? Https : Http,
                Host = nodesExt.hostname,
                Port = configuration.UseSsl ? nodesExt.services.cbasSSL : nodesExt.services.cbas,
                Path = AnalyticsPath
            }.Uri;
        }

        public static Uri GetSearchUri(this IPEndPoint endPoint, Configuration configuration, NodesExt nodesExt)
        {
            if (nodesExt.services.fts == 0)
            {
                return new UriBuilder
                {
                    Scheme = configuration.UseSsl ? Https : Http,
                    Host = nodesExt.hostname,
                }.Uri;
            }
            return new UriBuilder
            {
                Scheme = configuration.UseSsl ? Https : Http,
                Host = nodesExt.hostname,
                Port = configuration.UseSsl ? nodesExt.services.ftsSSL : nodesExt.services.fts
            }.Uri;
        }

        public static Uri GetViewsUri(this IPEndPoint endPoint, Configuration configuration, NodesExt nodesExt)
        {
            if (nodesExt.services.capi == 0)
            {
                return new UriBuilder
                {
                    Scheme = configuration.UseSsl ? Https : Http,
                    Host = nodesExt.hostname,
                }.Uri;
            }
            return new UriBuilder
            {
                Scheme = configuration.UseSsl ? Https : Http,
                Host = nodesExt.hostname,
                Port = configuration.UseSsl ? nodesExt.services.capiSSL : nodesExt.services.capi,
            }.Uri;
        }

        /*public static Uri ReplaceCouchbaseSchemeWithHttp(this Uri uri, IConfiguration configuration, string bucketName)
        {
            if (uri.Scheme == "couchbase")
            {
                var useSsl = configuration.UseSsl ? Https : Http;
                var newUri = new UriBuilder(uri) {Scheme = useSsl ? "https" : "http", Port = configuration.MgmtPort};
                return newUri.Uri;
            }
            return uri;
        }*/

        public static IPEndPoint GetIpEndPoint(this Uri uri, int port, bool useInterNetworkV6Addresses)
        {
            var ipAddress = GetIpAddress(uri, useInterNetworkV6Addresses);
            return new IPEndPoint(ipAddress, port);
        }

        public static IPAddress GetIpAddress(this Uri uri, bool useInterNetworkV6Addresses)
        {
            if (!IPAddress.TryParse(uri.Host, out var ipAddress))
            {
                try
                {
                    var hostEntry = Dns.GetHostEntry(uri.DnsSafeHost);

                    //use ip6 addresses only if configured
                    var hosts = useInterNetworkV6Addresses
                        ? hostEntry.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetworkV6)
                        : hostEntry.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork);

                    foreach (var host in hosts)
                    {
                        ipAddress = host;
                        break;
                    }

                    //default back to IPv4 addresses if no IPv6 can be resolved
                    if (useInterNetworkV6Addresses && ipAddress == null)
                    {
                        hosts = hostEntry.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork);
                        foreach (var host in hosts)
                        {
                            ipAddress = host;
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not resolve hostname to IP", e);
                }
            }
            if (ipAddress == null)
            {
                throw new Exception(uri.OriginalString);
            }
            return ipAddress;
        }
    }
}
