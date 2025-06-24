using System.Drawing.Text;
using System.Web;

namespace doc_push_service.models
{
    public class Enrollment
    {
        private readonly Uri? enrollmentUri;

        public string? Scheme => enrollmentUri == null ? null : enrollmentUri.Scheme;

        public string? Host => enrollmentUri == null ? string.Empty : enrollmentUri.Host;

        public int? Port => enrollmentUri == null ? 80 : enrollmentUri.Port;

        public string? Path => enrollmentUri == null ? string.Empty : enrollmentUri.AbsolutePath;

        public string? Query => enrollmentUri == null ? null : enrollmentUri.Query;

        public string AbsoluteUri => enrollmentUri.AbsoluteUri;

        public uint UriId => HashValue(AbsoluteUri);

        public Uri Uri => enrollmentUri;

        public Enrollment(string uri) 
        {
            HashSet<string> schemes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "http", "https" };

            if (uri == null) 
                throw new ArgumentNullException(nameof(uri));

            UriBuilder builder = new UriBuilder(uri);

            // validate enrollments are http[s] scheme
            if (!schemes.Contains(builder.Scheme))
            {
                throw new ArgumentException("Invalid scheme. It must be HTTP or HTTPS");
            }

            enrollmentUri = builder.Uri;
        }


        private static uint HashValue(string url)
        {
            string encoded = HttpUtility.UrlEncode(url);
            return (uint)encoded.GetHashCode();

        }
    }
}
