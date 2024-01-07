using Diploma_Model.Models.ViewModels;
using DiplomaMaster.Services.Models;

namespace DiplomaMaster.Services
{
    public interface ITwitterService
    {
        Task<TweetResult> PostTweetAsync(string message);
    }
}
