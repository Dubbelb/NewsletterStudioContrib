﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using NewsletterStudio.Bll.Providers;

namespace NewsletterStudioContrib.SubscriptionProviders
{

    /// <summary>
    /// This class demonstrates a simple approach do implement a custom subscription provider that downloads the list of emails from an external web-page. 
    /// Probably not a real world scenario so think of this class more as a demonstration of what can be done.
    /// </summary>
    public class DownloadSubscriptionProvider : SubscriptionProviderBase
    {
        /* Setting generic settings */

        public override string UniqName { get { return "DownloadSubscriptionProvider"; } }

        public override string DisplayName { get { return "Downloaded Lists"; } }

        public override bool CanEditSubsciber { get { return false; } }


        /* Lists and fetching */

        public override IEnumerable<ListItem> GetListItems()
        {
            return new List<ListItem>()
                       {
                           new ListItem() {Value = "myCoolList", Text = "The test list"},
                       };

        }

        public override List<Receiver> GetSubscribersForSendOut(string listItemValue, SendOutParams parameters)
        {
            // Since there is only one list we dont care about the myCoolList-value that will be passed in the listItemValue-parameter.

            var list = new List<Receiver>();

            // Downloads this webpage where each email is on one row.
            string page = NewsletterStudio.Helper.DownloadWebPage("http://localhost/email.html");

            // Spliting the list on every new line
            string[] arrEmails = page.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var email in arrEmails)
            {
                if(NewsletterStudio.Common.IsValidEmail(email))
                {
                    list.Add(new Receiver()
                    {
                        DataProviderKey = email,
                        Fullname = "",
                        Email = email                    
                    });
                }
            }

            return list;

        }



        /* Update and unsubscribe */

        public override string GetEditUrl(string subscriptionId, string email)
        {
            return "";
        }

        public override bool Unsubscribe(string email)
        {
            return true;
        }

        
    }
}
