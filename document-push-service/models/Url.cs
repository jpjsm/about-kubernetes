using System.ComponentModel.DataAnnotations;

namespace document_push_service.models
{
    public class Url
    {
        private string _url = string.Empty;

        [Key]
        public string AbsoluteUrl => _url;
        public Url() { }
        public Url(string url) { _url = url; }
    }
}
