using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class ScrapWindowHelper
    {
        public static void StopAndMakeScrapeWindowInvisible(int type = 0)
        {
            ScrapingEventArgs scrapingEventArgs = new ScrapingEventArgs();
            scrapingEventArgs.ActivityCollection = null;
            scrapingEventArgs.Type = type; //Type =1 means MakeScrapeWindowInvisible & close , 2 means only MakeScrapeWindowInvisible
            SelectHelper.OnScraping(scrapingEventArgs);
        }
    }
}
