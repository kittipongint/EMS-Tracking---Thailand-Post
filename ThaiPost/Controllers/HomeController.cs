using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace ThaiPost.Controllers
{
    public class HomeController : Controller
    {
        private string Username = ""; // Username from Thailand Post
        private string Password = ""; // Password from Thailand Post
        private string Languege = "TH"; //"EN"
        private string Barcode = "ER787920xxxTH";
        private string Barcodes = "ER787920xxxTH,ER438917xxxTH,ER438906xxxTH";

        public ActionResult Index()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult EMSTracking()
        {

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string xmlKey = rsa.ToXmlString(false);
            string xmlPrivKey = rsa.ToXmlString(true);

            thailandpost.track.TrackandTraceService sParams = new thailandpost.track.TrackandTraceService();
            sParams.Url = " http://track.thailandpost.co.th/TTPOSTWebService/TrackandTrace.asmx";
            //sParams.Proxy

            sParams.PublicKeySoapHeaderValue = new thailandpost.track.PublicKeySoapHeader();
            sParams.PublicKeySoapHeaderValue.PublicXmlKey = xmlKey;
            thailandpost.track.TrackItem item = sParams.GetItems(Username, Password, Languege, Barcode);
            //if (item.ItemsData.Length > 0)
            //{
            //}

            ViewBag.Barcode = Barcode;

            return View(item);
        }

        public ActionResult EMSTrackingBatch()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string xmlKey = rsa.ToXmlString(false);
            string xmlPrivKey = rsa.ToXmlString(true);

            thailandpost.track.TrackandTraceService sParams = new thailandpost.track.TrackandTraceService();
            sParams.Url = " http://track.thailandpost.co.th/TTPOSTWebService/TrackandTrace.asmx";
            //sParams.Proxy

            sParams.PublicKeySoapHeaderValue = new thailandpost.track.PublicKeySoapHeader();
            sParams.PublicKeySoapHeaderValue.PublicXmlKey = xmlKey;

            string strQuery = Barcodes;
            strQuery = strQuery.Trim().Replace("\r\n", ",");
            strQuery = strQuery.Replace(",,", ",");
            if (strQuery.Substring(strQuery.Length - 1, 1) == ",")
                strQuery = strQuery.Substring(0, strQuery.Length - 1);

            string[] ItemArr = strQuery.Split(',');

            thailandpost.track.TrackRequest item = sParams.RequestItems(Username, Password, Languege, ItemArr);

            //if (item.ItemsData.Length > 0)
            //{
            //}

            ViewBag.Barcode = Barcode;
            ViewBag.BarcodeCount = item.RequestCountData.CountNumber;

            return View(item);
        }
    }
}