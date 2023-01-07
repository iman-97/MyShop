using System.Security.Principal;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private MockRequest _request;
        private MockResponse _response;
        private HttpCookieCollection _cookies;
        private IPrincipal _fakeUser;

        public MockHttpContext()
        {
            _cookies = new HttpCookieCollection();
            _request = new MockRequest(_cookies);
            _response = new MockResponse(_cookies);
        }

        public override IPrincipal User { get => _fakeUser; set => _fakeUser = value; }
        public override HttpRequestBase Request { get => _request; }
        public override HttpResponseBase Response { get => _response; }

    }

    public class MockResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection _cookies;

        public MockResponse(HttpCookieCollection cookies)
        {
            _cookies = cookies;
        }

        public override HttpCookieCollection Cookies { get => _cookies; }

    }

    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection _cookies;

        public MockRequest(HttpCookieCollection cookies)
        {
            _cookies = cookies;
        }

        public override HttpCookieCollection Cookies { get => _cookies; }

    }

}
