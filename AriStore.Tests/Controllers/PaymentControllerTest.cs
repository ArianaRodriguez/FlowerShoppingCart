using System;
using System.Configuration;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AriStore.Controllers;
using AriStore.Tests.Mock;
using AriStore.Models;

namespace AriStore.Tests.Controllers
{
    [TestClass]
    public class PaymentControllerTest
    {    
        /// <summary>
        /// Create Payment and redirected
        /// </summary>
        [TestMethod]
        public void Payment()
        {
            AppSettingsReader appSettingsReader = new AppSettingsReader();
            string clientId = (string)appSettingsReader.GetValue("clientId", typeof(string));
            string clientSecret = (string)appSettingsReader.GetValue("clientSecret", typeof(string));
            var apicontext = PayPalConfiguration.GetAPIContext(clientId, clientSecret);

            PaymentController controller = new PaymentController();
            controller.ControllerContext = new ControllerContext { HttpContext = new Mock.MockHttpContext(), RouteData = new RouteData() };
            BouquetOrder bouquet = new BouquetOrder(3, 1, new Bouquet(6,"gardenia", 2.00, 0));

            var result = controller.Payment(apicontext, bouquet);
            var url = ((RedirectResult)(result)).Url;
            var permanent = ((RedirectResult)(result)).Permanent;
            Assert.IsTrue(permanent == false, "Should have redirected");
            Assert.IsNotNull(result, "url null");
        }
        /// <summary>
        /// Create and execute a payment
        /// </summary>
        [TestMethod]
        public void CreateAndExecutePayment()
        {
            AppSettingsReader appSettingsReader = new AppSettingsReader();
            string clientId = (string)appSettingsReader.GetValue("clientId", typeof(string));
            string clientSecret = (string)appSettingsReader.GetValue("clientSecret", typeof(string));
            var apicontext = PayPalConfiguration.GetAPIContext(clientId, clientSecret);

            PaymentController controller = new PaymentController();
            var guid = Convert.ToString((new Random()).Next(100000));
            string baseURI = "http://localhost:37256/Paypal/PayWithPayPal?guid=" + guid;
            BouquetOrder bouquet = new BouquetOrder(1, 1, new Bouquet(6, "gardenia", 2.00, 0));
            var payment = controller.CreatePayment(apicontext, baseURI, bouquet);

            var context = new Mock.MockHttpContext();
            context.m_request.m_queryString.Add("PayerID", guid);           
            context.m_request.m_queryString.Add("guid", guid);
            context.Session.Add(guid, payment.id);
           
            controller.ControllerContext = new ControllerContext { HttpContext = context , RouteData = new RouteData() };
            var result = controller.ExecutePayment(apicontext, guid, payment.id);

            Assert.IsNotNull(apicontext, "PayPal API context is null");
            Assert.IsNotNull(payment.id, "Fail to create payment");
            //must be failed because the data are fake.
            Assert.IsTrue(result.state.ToLower() != "approved", result.state.ToLower());         
        }
     
    }
}
