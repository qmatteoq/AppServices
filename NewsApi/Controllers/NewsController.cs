using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using NewsApi.Models;

namespace NewsApi.Controllers
{
    public class NewsController : ApiController
    {
        // GET api/values
        public async Task<IEnumerable<FeedItem>> Get()
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("https://blogs.windows.com/feed/");
            var xdoc = XDocument.Parse(result);
            int cont = 0;
            List<FeedItem> items = new List<FeedItem>();
            foreach (XElement item in xdoc.Descendants("item"))
            {
                var news = new FeedItem
                {
                    Id = cont+1,
                    Title = (string) item.Element("title"),
                    Description = (string) item.Element("description"),
                    Link = (string) item.Element("link"),
                    PublishDate = DateTime.Parse((string) item.Element("pubDate"))
                };
                items.Add(news);
                cont++;
            }
            return items;
        }

        // GET api/values/5
        public async Task<FeedItem> Get(int id)
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("https://blogs.windows.com/feed/");
            var xdoc = XDocument.Parse(result);
            int cont = 0;
            List<FeedItem> items = new List<FeedItem>();
            foreach (XElement item in xdoc.Descendants("item"))
            {
                var news = new FeedItem
                {
                    Id = cont+1,
                    Title = (string)item.Element("title"),
                    Description = (string)item.Element("description"),
                    Link = (string)item.Element("link"),
                    PublishDate = DateTime.Parse((string)item.Element("pubDate"))
                };
                items.Add(news);
                cont++;
            }

            return items[id];
        }
    }
}
