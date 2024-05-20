using System.ServiceModel.Syndication;
using System.Xml;

namespace MyMovies.MoviesApi.WebApp.Services
{
    public class MovieFeedService : IMovieFeedService
    {
        private readonly string feedUrl;

        public MovieFeedService(IConfiguration configuration)
        {
            this.feedUrl = configuration["movieFeedService:Url"]!;
        }

        private IList<SyndicationItem> GetSyndicationItems(DateTime publishedDate)
        {
            List<SyndicationItem> itemsCollection = new();
            if (!string.IsNullOrEmpty(this.feedUrl))
            {
                using XmlReader reader = XmlReader.Create(this.feedUrl);

                SyndicationFeed feed = SyndicationFeed.Load(reader);
                itemsCollection.AddRange(feed.Items.Where(e => e.PublishDate.Date.CompareTo(publishedDate.Date) == 0));
            }
            return itemsCollection;
        }

        public IList<MovieFeedItem> GetMovieFeedItems(DateTime publishedDate)
        {
            List<MovieFeedItem> itemsCollection = new();

            foreach (var item in GetSyndicationItems(publishedDate))
            {
                itemsCollection.Add(new MovieFeedItem(item.Title.Text, item.Summary.Text, item.PublishDate.Date));
            }

            return itemsCollection;
        }
    }
}
