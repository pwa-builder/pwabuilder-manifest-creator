using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PWABuilder.ManifestCreator
{
    public static class HtmlAgilityPackExtensions
    {
        /// <summary>
        /// Selects child nodes matching the specified xpath. If no nodes are found, an empty enumerable is returned.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> SelectNodesOrEmpty(this HtmlNode node, string xpath)
        {
            return node.SelectNodes(xpath) ?? Enumerable.Empty<HtmlNode>();
        }
    }
}
