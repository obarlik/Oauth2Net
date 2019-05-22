using NUnit.Framework;
using OAuth2Net;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Tests
{
    public class FacebookTests
    {
        HttpListener listener;
        OAuth2Facebook api;
        string result;


        [SetUp]
        public void Setup()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:6625/auth/facebook/");

            api = new OAuth2Facebook(
                "2293070497624844",
                "c530b7a2614a93b328f0c170c6fcd001",
                "http://localhost:6625/auth/facebook/default.aspx",
                success: api => result = "success",
                failure: api => result = "failure");
        }


        [Test]
        public void Test1()
        {
            var authUrl = api.GetAuthorizationUrl();

            var cli = new WebClient();

            cli.Encoding = Encoding.UTF8;

            listener.Start();

            var context = listener.GetContext();

            OAuth2App.Callback(
                context.Request.QueryString["code"],
                context.Request.QueryString["error"],
                context.Request.QueryString["state"],
                context.Request.QueryString["error_description"],
                context.Request.QueryString["error_uri"]);

            Assert.AreEqual(result, "success");
        }
    }
}