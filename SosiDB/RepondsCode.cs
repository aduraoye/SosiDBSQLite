using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SosiDB
{
    internal static class RespondsCode
    {
        internal static string RespondsCodeToString(string strCode)
        {

            switch (strCode)
            {
                case "0000":
                    return "Message added to queue OK.";
                case "2006":
                    return "Not enough information has been supplied for authentication. Please ensure that your Username and Unique Key are supplied in your request";
                case "2007":
                    return "Your account has not been activated.";
                case "2015":
                    return "The destination mobile number is invalid.";
                case "2016":
                    return "Identical message already sent to this recipient. Please try again in a few seconds.";
                case "2017":
                    return "Invalid Sender ID. Please ensure Sender ID is no longer than 11 characters (if alphanumeric), and contains no spaces.";
                case "2018":
                    return "You have reached the end of your message credits. You will need to purchase more messages.";
                case "2022":
                    return "Your Unique Key is incorrect. This may be caused by a recent Key change.";
                case "2051":
                    return "Message is empty.";
                case "2052":
                    return "Too many recipients.";
                default:
                    if (Convert.ToInt32(strCode) >= 2100 && Convert.ToInt32(strCode) <= 2199)
                        return "Internal Error";
                    else
                        return "Code unspecified.";
            }
        }
    }
}
