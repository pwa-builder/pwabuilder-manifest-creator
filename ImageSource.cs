using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.PWABuilder.ManifestCreator
{
    public class ImageSource
    {
        /// <summary>
        /// Creates a new ImageSource from the specific HTML node. Inspects the node and finds href or content attribute, type attribute, and sizes attribute.
        /// Works best on HTML nodes like: &lt;link rel="icon" href="/images/foo.png" type="image/png" sizes="32x32" /&gt;
        /// </summary>
        /// <param name="node">The parent node whose children to select.</param>
        /// <returns>The content attribute value of the child nodes.</returns>
        public ImageSource(HtmlNode node)
        {
            var href = node.GetAttributeValue("href", string.Empty);
            var content = node.GetAttributeValue("content", string.Empty);
            var url = string.IsNullOrWhiteSpace(href) ? content : href;
            
            // See if we have a mime type. If not, best guess it.
            var mimeType = node.GetAttributeValue("type", string.Empty);
            if (mimeType == null && url != null)
            {
                mimeType = GuessMimeTypeFromUrl(url);
            }

            this.Url = url ?? string.Empty;
            this.MimeType = mimeType;
            this.Sizes = node.GetAttributeValue("sizes", string.Empty);
        }

        public ImageSource(string url, string? mimeType, string? sizes)
        {
            this.Url = url;
            this.MimeType = mimeType;
            this.Sizes = sizes;
        }

        public string Url { get; private set; }
        public string? MimeType { get; private set; }
        public string? Sizes { get; private set; }

        private static string? GuessMimeTypeFromUrl(string url)
        {
            var extensionMimeTypes = new Dictionary<string, string>
            {
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" },
                { ".ico", "image/x-icon" },
                { ".svg", "image/svg+xml" },
                { ".webp", "image/webp" }
            };
            return extensionMimeTypes.FirstOrDefault(ext => url.Contains(ext.Key)).Value;
        }

        public class ImageSourceUrlComparer : System.Collections.Generic.IEqualityComparer<ImageSource>
        {
            public bool Equals(ImageSource? x, ImageSource? y)
            {
                var xUrl = x?.Url;
                var yUrl = y?.Url;
                return string.Equals(xUrl, yUrl, StringComparison.Ordinal);
            }

            public int GetHashCode([DisallowNull] ImageSource obj)
            {
                return obj.Url?.GetHashCode(StringComparison.Ordinal) ?? 0;
            }
        }
    }
}
