using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SiteLoader
{
    public class HTMLParser
    {
        int depthLevel = 0;
        int _depth = 0;
        public HTMLParser (int depth)
        {
            _depth = depth;
        }
        public void ParseSite(string url, string path)
        {
            HtmlDocument html = new HtmlDocument();
            Loader loader = new Loader();
            var response = loader.GetResponse(url);

            html.Load(response.Content.ReadAsStreamAsync().Result);
            var nodes = html.DocumentNode.Descendants().Where(x => x.Name == "a");
            if (nodes != null && depthLevel != _depth)
            {                
                foreach (HtmlNode node in nodes)
                {
                    string link = node.GetAttributeValue("href", null);
                    if (link != null && link.StartsWith("http"))
                    {
                        Console.WriteLine(link);
                        var document = loader.GetResponse(link);
                        html.Load(document.Content.ReadAsStreamAsync().Result);
                        depthLevel++;
                        using (StreamWriter outputFile = new StreamWriter(
                            Path.Combine(path, $"{GetFileNameFromUrl(link)}.html"), false))
                        {
                            html.Save(outputFile);
                        }
                        var childNodes = html.DocumentNode.Descendants().Where(x => x.Name == "a");
                        if (childNodes.Any())
                        {
                            foreach (var childNode in childNodes)
                            {
                                var childLink = childNode.GetAttributeValue("href", null);
                                if (childLink != null && childLink.StartsWith("http"))
                                {
                                    var siteName = GetFileNameFromUrl(childLink);
                                    var newPath = Path.Combine(path, siteName);
                                    Directory.CreateDirectory(newPath);
                                    ParseSite(childLink, newPath);
                                }
                            }
                        }
                    }
                }
            }
        }

        private string GetFileNameFromUrl(string url)
        {
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                uri = new Uri(url);

            return Path.GetFileName(uri.LocalPath);
        }
    }
}
