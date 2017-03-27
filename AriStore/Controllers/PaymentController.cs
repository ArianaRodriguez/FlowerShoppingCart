using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PayPal.Api;
using PayPal.Util;
using AriStore.Models;

namespace AriStore.Controllers
{
    public class PaymentController : Controller
    {
        private PayPal.Api.Payment payment;                
        /// <summary>
        /// Buy a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="shipping"></param>
        /// <param name="tax"></param>
        /// <returns></returns>  
        public ActionResult CreateOrder(int id, string name, double price, double shipping, double tax)
        {
            var bouquet = new Bouquet(id, name, price, shipping);
            var order = new BouquetOrder(1, tax, bouquet);
            APIContext apiContext = PayPalConfiguration.GetAPIContext();
            if (apiContext == null)
            {
                var error = new ErrorMessage();
                error.Message = "Configuration error";
                return View("FailureView", error);
            }
            return Payment(apiContext, order);          
        }
        /// <summary>
        /// Create url on which paypal sendsback the data
        /// </summary>
        /// <param name="apiContext"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult Payment(APIContext apiContext, BouquetOrder order)
        {              
            string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PayWithPaypal?";
            //guid will be used in the payment execution
            var guid = Convert.ToString((new Random()).Next(100000));
            var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, order);
            if (createdPayment == null)
            {
                var error = new ErrorMessage();
                error.Message = "Payment error";
                return View("FailureView", error);
            }
            //returned from paypal in response to Create function call
            var links = createdPayment.links.GetEnumerator();
            string paypalRedirectUrl = null;
            while (links.MoveNext())
            {
                Links lnk = links.Current;
                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    paypalRedirectUrl = lnk.href;
            }
            // saving the paymentID in the key guid
            Session.Add(guid, createdPayment.id);

            if (paypalRedirectUrl == null)
            {
                var error = new ErrorMessage();
                error.Message = "Url not found";
                return View("FailureView", error);
            }
            return Redirect(paypalRedirectUrl);
        }
        /// <summary>
        /// Create Payment object of PayPal
        /// </summary>
        /// <param name="apiContext"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Payment CreatePayment(APIContext apiContext, string redirectUrl, BouquetOrder order)
        {
            var itemList = new ItemList() { items = new List<Item>() };
            itemList.items.Add(new Item()
            {
                name = order.Bouquet.Name,
                currency = "USD",
                price = order.Bouquet.Price,
                quantity = order.Quatity,
                sku = Convert.ToString((new Random()).Next(200))
            });
            var payer = new Payer() { payment_method = "paypal" };
            //configure RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
            var details = new Details()
            {
                tax = order.Tax,
                shipping = order.Bouquet.Shipping,
                subtotal = order.SubTotal
            };
            // Total must be equal to sum of shipping, tax and subtotal.
            var amount = new Amount()
            {
                currency = "USD",
                total = order.Total,
                details = details
            };
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = order.Bouquet.Name,
                invoice_number = Convert.ToString((new Random()).Next(1000)),
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            try
            {
                return this.payment.Create(apiContext);
            }
            catch (PayPal.PaymentsException ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
        /// <summary>
        /// Pay with Paypal
        /// </summary>
        /// <returns></returns>
        public ActionResult PayWithPaypal()
        {
            APIContext apiContext = PayPalConfiguration.GetAPIContext();
            if (apiContext == null)
            {
                var error = new ErrorMessage();
                error.Message = "Configuration error";
                return View("FailureView", error);
            }
            string payerId = Request.Params["PayerID"];
            var guid = Request.Params["guid"];
            var executePayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
            if (executePayment.state.ToLower() != "approved")
            {
                var error = new ErrorMessage();
                error.Message = "The payment couldn't be processed";
                return View("FailureView", error);
            }

            return View("SuccessView", executePayment.transactions);
        }       
        /// <summary>
        /// Execute the payment 
        /// </summary>
        /// <param name="apiContext"></param>
        /// <param name="payerId"></param>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId, state = "failed" };
            try
            {
                return this.payment.Execute(apiContext, paymentExecution);
            }
            catch (PayPal.PaymentsException ex)
            {
                Console.WriteLine(ex);
                return this.payment;
            }
        }     
    }
}
