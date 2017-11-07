using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsApi.Models;
using System.Net.Http;
using System.Xml.Linq;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<FeedItem>> Get()
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("https://blogs.windows.com/feed/");
            XDocument xdoc = XDocument.Parse(result);
            int cont = 0;
            List<FeedItem> items = new List<FeedItem>();
            foreach (XElement item in xdoc.Descendants("item"))
            {
                var news = new FeedItem
                {
                    Id = cont + 1,
                    Title = (string)item.Element("title"),
                    Description = (string)item.Element("description"),
                    Link = (string)item.Element("link"),
                    PublishDate = DateTime.Parse((string)item.Element("pubDate"))
                };
                items.Add(news);
                cont++;
            }
            return items;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
