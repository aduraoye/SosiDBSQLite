
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Windows.Forms;

namespace SosiDB
{
    public static class ClickSendSMS
    {
        static string _UserName = "";
        /// <summary>
        /// Get or set username.
        /// </summary>
        public static string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        static string _Key = "";
        /// <summary>
        /// get or set user key.
        /// </summary>
        public static string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        static string _SMSTo = "";
        /// <summary>
        /// get or set only Destination number(s).
        /// </summary>
        public static string SMSTo
        {
            get { return _SMSTo; }
            set { _SMSTo = value; }
        }
 
        static string _Message = "";
        /// <summary>
        /// get or set message text
        /// </summary>
        public static string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
 
        static string _SenderID = "";
        public static string SenderID
        {
            get { return ClickSendSMS._SenderID; }
            set { ClickSendSMS._SenderID = value; }
        }

        static string _Schedule = "";
        public static string Schedule
        {
            get { return ClickSendSMS._Schedule; }
            set { ClickSendSMS._Schedule = value; }
        }
 
      
        /// <summary>
        /// Send single sms.
        /// </summary>
        /// <param name="strUserName">UserName</param>
        /// <param name="strKey">Key</param>
        /// <param name="strSMS">Destination number(s)</param>
        /// <param name="strMessage">SMS text</param>
        /// <param name="strSenderID">Sender ID</param>
        /// <param name="strSchedule">Schedule</param>
        /// <returns>Responds text with appropriate message</returns>
        public static SMSRespondsData SendSms(string strUserName, string strKey, string strSMS, string strMessage, string strSenderID, string strSchedule)
        {
            _UserName = strUserName;
            _Key = strKey;
            _SMSTo = strSMS;
            _Message = strMessage;
            _SenderID = strSenderID;
            _Schedule = strSchedule;
            return SendSms();
        }
   
        /// <summary>
        /// Send single sms.
        /// </summary>
        /// <returns>Responds text with appropriate message</returns>
        public static SMSRespondsData SendSms()
        {
            try
            {
                if (_UserName == "")
                    throw new Exception("Username can not be empty");
                if (_Key == "")
                    throw new Exception("Key can not be empty");
                if (_Message == "")
                    throw new Exception("Message can not be empty");
                if (_SMSTo == "")
                    throw new Exception("Number(s) can not be empty");

                WebClient wc = new WebClient();
                string sRequestURL;


               sRequestURL = "https://api.clicksend.com/http/v2/send.php?method=http&username=" + _UserName + "&key=" + _Key + "&to=" + _SMSTo + "&message=" + _Message;
               if (_SenderID != "")
                    sRequestURL += "&senderid=" + _SenderID;

                if (_Schedule != "")
                    sRequestURL += "&schedule=" + _Schedule;

                byte[] response = wc.DownloadData(sRequestURL);
                string sResult = Encoding.ASCII.GetString(response);
                return SetSMSData(sResult);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
 

 
        /// <summary>
        /// Send single sms.
        /// </summary>
        /// <param name="strUserName">UserName</param>
        /// <param name="strKey">Key</param>
        /// <param name="strSMS">Multiple destination numbers, Seperated with comma</param>
        /// <param name="strMessage">SMS text</param>
        /// <param name="strSenderID">Sender ID</param>
        /// <param name="strSchedule">Schedule</param>
        /// <returns>Responds text with appropriate message</returns>
        public static List<SMSRespondsData> SendMultiSms(string strUserName, string strKey, string strSMS, string strMessage, string strSenderID, string strSchedule)
        {
            _UserName = strUserName;
            _Key = strKey;
            _SMSTo = strSMS;
            _Message = strMessage;
            _SenderID = strSenderID;
            _Schedule = strSchedule;
            return SendMultiSms();
        }
        

     
        /// <summary>
        /// Send single sms.
        /// </summary>
        /// <returns>Responds text with appropriate message</returns>
        public static List<SMSRespondsData> SendMultiSms()
        {
            try
            {
                if (_UserName == "")
                    throw new Exception("Username can not be empty");
                if (_Key == "")
                    throw new Exception("Key can not be empty");
                if (_Message == "")
                    throw new Exception("Message can not be empty");
                if (_SMSTo == "")
                    throw new Exception("Number(s) can not be empty");

                WebClient wc = new WebClient();
                string sRequestURL;
                // sRequestURL = "http://inteltech.com.au/secure-api/send.php?username=" + _UserName + "&key=" + _Key + "&sms=" + _SMSTo + "&method=csharp" + "&message=" + _Message;
                sRequestURL = "https://api.clicksend.com/http/v2/send.php?method=http&username=" + _UserName + "&key=" + _Key + "&to=" + _SMSTo + "&message=" + _Message;
                if (_SenderID != "")
                    sRequestURL += "&senderid=" + _SenderID;

                if (_Schedule != "")
                    sRequestURL += "&schedule=" + _Schedule;
                byte[] response = wc.DownloadData(sRequestURL);
                string sResult = Encoding.ASCII.GetString(response);
                return SetMultipleSMSData(sResult);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }


        /// <summary>
        /// Check credit of particular account
        /// </summary>
        /// <param name="strUserName">Username</param>
        /// <param name="strKey">Key</param>
        /// <returns>Credit responds data</returns>
        public static CreditRespondsData CheckCredit(string strUserName, string strKey)
        {
            try
            {
                _UserName = strUserName;
                _Key = strKey;
                return CheckCredit();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }


        /// <summary>
        /// Check credit of particular account
        /// </summary>
        /// <returns>Credit responds data</returns>
        public static CreditRespondsData CheckCredit()
        {
            try
            {
                if (_UserName == "")
                    throw new Exception("Username can not be empty");
                if (_Key == "")
                    throw new Exception("Key can not be empty");

                WebClient wc = new WebClient();
                String sRequestURL;
                // sRequestURL = "http://inteltech.com.au/secure-api/credit.php?username=" + _UserName + "&key=" + _Key;
                sRequestURL = "https://api.clicksend.com/http/v2/balance.php?username=" + _UserName + "&key=" + _Key;
                byte[] response = wc.DownloadData(sRequestURL);
                string sResult = Encoding.ASCII.GetString(response);
                return SetCreditData(sResult);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }



        static List<SMSRespondsData> SetMultipleSMSData(string _XMLResponds)
        {
            List<SMSRespondsData> _SMSRespondsDatas = new List<SMSRespondsData>();
            XmlDocument _Doc = new XmlDocument();
            _Doc.LoadXml(_XMLResponds);
            XmlNode _Node = _Doc.SelectSingleNode("xml");
            foreach (XmlNode item in _Node)
            {
                SMSRespondsData _Data = new SMSRespondsData();
                _Data.SMSTo = item.ChildNodes[0].InnerText;
                _Data.RespondsCode = Convert.ToInt32(item.ChildNodes[3].InnerText);
                _Data.RespondsText = RespondsCode.RespondsCodeToString(item.ChildNodes[3].InnerText);
                // if (_Data.RespondsCode == 0)
                //{
                //    _Data.MessageID = item["messageid"].InnerText;
                //}
                _Data.Key = _Key;
                _Data.UserName = _UserName;
                _Data.SenderID = item.ChildNodes[2].InnerText;
                _SMSRespondsDatas.Add(_Data);
            }

            return _SMSRespondsDatas;
        }



        static SMSRespondsData SetSMSData(string _XMLResponds)
        {
            XmlDocument _Doc = new XmlDocument();
            _Doc.LoadXml(_XMLResponds);
            XmlNode _Node = _Doc.SelectSingleNode("xml");
            XmlNode _resultNode = _Node.SelectSingleNode(".//" + "result");
           
            SMSRespondsData _SMSRespondsData = new SMSRespondsData();
            _SMSRespondsData.RespondsCode = System.Convert.ToInt32(_resultNode.InnerText);
            _SMSRespondsData.RespondsText = RespondsCode.RespondsCodeToString( _resultNode.InnerText);
            _SMSRespondsData.SMSTo = _SMSTo;
            _SMSRespondsData.Message = _Message;
            _SMSRespondsData.Key = _Message;
            _SMSRespondsData.UserName = _UserName;
            return _SMSRespondsData;
        }


        static CreditRespondsData SetCreditData(string _XMLResponds)
        {
            XmlDocument _Doc = new XmlDocument();
            _Doc.LoadXml(_XMLResponds);
            XmlNode _Node = _Doc.SelectSingleNode("xml");
            XmlNode _typeNode = _Node.SelectSingleNode(".//" + "type");
            XmlNode _valueNode = _Node.SelectSingleNode(".//" + "balance");

            CreditRespondsData _CreditRespondsData = new CreditRespondsData();
            _CreditRespondsData.Key = _Key;
            _CreditRespondsData.UserName = _UserName;
            _CreditRespondsData.Type = _typeNode.InnerText;
            _CreditRespondsData.Value = Convert.ToDecimal( _valueNode.InnerText);
            return _CreditRespondsData;
        }
    }
    
    public class CreditRespondsData : RespondsData
    {
        string _Type = "";
        /// <summary>
        /// Get the SMS limit type, e.g. Credit...
        /// </summary>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        decimal _Value = 0;
        /// <summary>
        /// Get the value of SMS, like 100,200...
        /// </summary>
        public decimal Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }

    public class RespondsData
    {
        string _UserName = "";
        /// <summary>
        /// Get username.
        /// </summary>
        public string UserName
        {
            set { _UserName = value; }
            get { return _UserName; }
        }

        string _Key = "";
        /// <summary>
        /// get user key.
        /// </summary>
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        int _RespondsCode = 0;
        /// <summary>
        /// Get the Message Responds Code
        /// </summary>
        public int RespondsCode
        {
            get { return _RespondsCode; }
            set { _RespondsCode = value; }
        }

        string _RespondsText = "";
        /// <summary>
        /// Get the Message Responds Text
        /// </summary>
        public string RespondsText
        {
            get { return _RespondsText; }
            set { _RespondsText = value; }
        }

        string _SenderID = "";
        /// <summary>
        /// get or set Sender id
        /// </summary>
        public string SenderID
        {
            get { return _SenderID; }
            set { _SenderID = value; }
        }
    }
    
    public class SMSRespondsData : RespondsData
    {
        string _SMSTo = "";
        /// <summary>
        /// get or set only Destination number.
        /// </summary>
        public string SMSTo
        {
            set { _SMSTo = value; }
            get { return _SMSTo; }
        }

        string _Message = "";
        /// <summary>
        /// get or set message text
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
    }
}