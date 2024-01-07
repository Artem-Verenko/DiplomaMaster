using Diploma_DataAccess.DTOs;
using DiplomaMaster.Services.Models;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;

namespace DiplomaMaster.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly IConfiguration _configuration;
        private TwitterClient _twitterClient;

        public TwitterService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeClient();
        }
        private void InitializeClient()
        {
            var consumerKey = _configuration.GetValue<string>("TwitterAPI:ConsumerKey");
            var consumerSecret = _configuration.GetValue<string>("TwitterAPI:ConsumerSecret");
            var accessToken = _configuration.GetValue<string>("TwitterAPI:AccessToken");
            var accessSecret = _configuration.GetValue<string>("TwitterAPI:AccessSecret");

            _twitterClient = new TwitterClient(consumerKey, consumerSecret, accessToken, accessSecret);
        }

        public async Task<TweetResult> PostTweetAsync(string message)
        {
            var tweetRequest = BuildTweetRequest(new TwitterDTO { Text = message }, _twitterClient);
            var result = await _twitterClient.Execute.AdvanceRequestAsync(tweetRequest);


            return new TweetResult
            {
                IsSuccess = result.Response.IsSuccessStatusCode,
                Message = result.Response.IsSuccessStatusCode ? "Success: Tweet was sent successfully." : $"Error: Failed to send tweet - {result.Response.Content}"
            };
        }

        private static Action<ITwitterRequest> BuildTweetRequest(
            TwitterDTO newTweet,
            TwitterClient userClient)
        {
            return (ITwitterRequest request) =>
            {
                var jsonBody = userClient.Json.Serialize(newTweet);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                request.Query.Url = "https://api.twitter.com/2/tweets";
                 request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                request.Query.HttpContent = content;
            };
        }
    }
}
