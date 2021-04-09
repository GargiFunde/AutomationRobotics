using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace Bot.Activity.Twitter
{
    public class Get_Tweets : CodeActivity
    {
        [Category("Input Authentication")]
        [RequiredArgument]
        public InArgument<String> ConsumerKey { get; set; }

        [Category("Input Authentication")]
        [RequiredArgument]
        public InArgument<String> ConsumerSecret { get; set; }

        [Category("Input Authentication")]
        [RequiredArgument]
        public InArgument<String> AccessToken { get; set; }

        [Category("Input Authentication")]
        [RequiredArgument]
        public InArgument<String> AccessSecret { get; set; }

        [Category("Tweet Details")]
        [RequiredArgument]
        public InArgument<int> Count { get; set; }

        [Category("Tweet Details")]
        [RequiredArgument]
        public InArgument<String> SearchKey { get; set; }

        /*
        public enum type
        {
            Popular = TwitterSearchResultType.Popular,
            Recent = TwitterSearchResultType.Recent,
            Mixed = TwitterSearchResultType.Mixed
        }

        [Category("Tweet Details")]
        [RequiredArgument]
        public type SearchType { get; set; }
        */
        [Category("Output")]
        [RequiredArgument]
        public OutArgument<DataTable> OutputResult { get; set; }

        public static TwitterService service;
        DataTable dtTweet = new DataTable();

        protected override void Execute(CodeActivityContext context)
        {

            service = new TwitterService(ConsumerKey.Get(context),
                      ConsumerSecret.Get(context),
                      AccessToken.Get(context),
                      AccessSecret.Get(context));

            service.TraceEnabled = true;

            var tweetsSearch = service.Search(new SearchOptions
            {
                Q = SearchKey.Get(context),
                Resulttype = TwitterSearchResultType.Recent,
                Count = Count.Get(context)
            });

            List<TwitterStatus> resultList = new List<TwitterStatus>(tweetsSearch.Statuses);


            dtTweet.Columns.Add("Tweet Id");
            dtTweet.Columns.Add("User Name");
            dtTweet.Columns.Add("User Screen Name");
            dtTweet.Columns.Add("Text");
            dtTweet.Columns.Add("Created Date");
            dtTweet.Columns.Add("Retweet Count");
            dtTweet.Columns.Add("Favorite Count");
            dtTweet.Columns.Add("Profile Image URL");

            foreach (var tweet in tweetsSearch.Statuses)
            {
                
                dtTweet.Rows.Add(tweet.Id,
                    tweet.User.Name,
                    tweet.User.ScreenName,
                    tweet.Text,
                    tweet.CreatedDate,
                    tweet.RetweetCount,
                    tweet.FavoriteCount,
                    tweet.User.ProfileImageUrl);
            }
            OutputResult.Set(context, dtTweet);
        }
    }
}
