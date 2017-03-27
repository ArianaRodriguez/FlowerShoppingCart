using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AriStore.Tests.Mock
{
    /// <summary>
    /// HttpContextBase Fake
    /// </summary>
    public class MockHttpContext : HttpContextBase
    {
        public MockHttpRequest m_request = new MockHttpRequest();
        public MockHttpResponse m_response = new MockHttpResponse();
        public MockHttpSession m_session = new MockHttpSession();

        public override HttpRequestBase Request
        { get { return m_request; } }

        public override HttpResponseBase Response
        { get { return m_response; } }

        public override HttpSessionStateBase Session
        { get { return m_session; } }
    }

    public class MockHttpRequest : HttpRequestBase
    {
        public NameValueCollection m_queryString = new NameValueCollection();   
     
        public override NameValueCollection Params
        {
            get { return m_queryString; }          
        }                  
        public override Uri Url
        {
            get { return new Uri("http://localhost:37256/Paypal/PaymentWithPaypal"); }
        }
        public override NameValueCollection QueryString
        {
            get { return m_queryString; } 
        }
    }

    public class MockHttpSession : HttpSessionStateBase
    {
        Dictionary<string, object> m_SessionStorage = new Dictionary<string, object>();
        public override object this[string name]
        {
            get { return m_SessionStorage[name]; }
            set { m_SessionStorage[name] = value; }
        }
        public override void Add(string name, object value)
        {
            m_SessionStorage[name] = value;
        }
    }

    public class MockHttpResponse : HttpResponseBase
    {       
    }

}
