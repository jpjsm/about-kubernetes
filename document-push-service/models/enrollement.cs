using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace document_push_service.models
{
    public class Enrollement
    {
        private readonly Uri? enrollementUri;

        public string? Scheme => enrollementUri == null ? null : enrollementUri.Scheme;

        public string? Host => enrollementUri == null ? string.Empty : enrollementUri.Host;

        [Key]
        public int? Port => enrollementUri == null ? 80 : enrollementUri.Port;

        public string? Path => enrollementUri == null ? string.Empty : enrollementUri.AbsolutePath;

        public string? Query => enrollementUri == null ? null : enrollementUri.Query;

        [NotMapped]
        public string? AbsoluteUri => enrollementUri == null ? null : enrollementUri.AbsoluteUri;

        [NotMapped]
        public Uri? Uri => enrollementUri;

        public Enrollement()
        {
            enrollementUri = null;
        }

        public Enrollement(string uri) 
        {
            try
            {
                UriBuilder builder = new UriBuilder(uri);
                enrollementUri = builder.Uri;
            }
            catch (UriFormatException ex)
            {
                Console.WriteLine($"{ex.HResult}: {ex.Message}\n{ex.Source}");
                throw;
            }

        }
    }
}
