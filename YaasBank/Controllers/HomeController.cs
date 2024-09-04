using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using YaasBank.ir.shaparak.pecConfirm;
using YaasBank.ir.shaparak.pecReverse;
using YaasBank.ir.shaparak.pecSale;
using YaasBank.Models;

namespace YaasBank.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string SalePayment(int price, int idFactor)
        {
            long token = 0;
            short paymentStatus = Int16.MinValue;
            ClientPaymentResponseDataBase responseData = null;
            using (var service = new SaleService())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    new System.Net.Security.RemoteCertificateValidationCallback((o, xc, xch, sslP) => true);

                service.Url = Helper.ConfigHelper.ParsianPGWSaleServiceUrl;

                var saleRequest = new ClientSaleRequestData();

                string loginAccount = Helper.ConfigHelper.LoginAccount;
                saleRequest.LoginAccount = loginAccount;

                saleRequest.CallBackUrl = Helper.ConfigHelper.SalePaymentCallback;
                saleRequest.Amount = price;
                saleRequest.OrderId = idFactor;

                responseData = service.SalePaymentRequest(saleRequest);

                paymentStatus = responseData.Status;

                if (responseData.Status == Helper.ParsianPaymentGateway.Successful)
                {
                    token = responseData.Token;
                    //Session["Token"] = token;
                }
                else
                {
                    string jsonString = JsonConvert.SerializeObject(new BankTokenStatus
                    {
                        Message = responseData.Message,
                        Token = string.Empty,
                        Status = paymentStatus
                    });

                    return jsonString;
                }
            }

            if (paymentStatus == Helper.ParsianPaymentGateway.Successful && token > 0L)
            {
                var redirectUrl = string.Format(Helper.ConfigHelper.ParsianIPGPageUrl, responseData.Token);
                string jsonString = JsonConvert.SerializeObject(new BankTokenStatus
                {
                    Message = responseData.Message,
                    Token = redirectUrl,
                    Status = paymentStatus
                });
                return jsonString;
            }
            else
            {
                string jsonString = JsonConvert.SerializeObject(new BankTokenStatus
                {
                    Message = responseData.Message,
                    Token = string.Empty,
                    Status = paymentStatus
                });

                return jsonString;
            }
        }

        public class PaymentCallbackModel
        {
            public long? Token { get; set; }

            [Display(Name = "شناسه سفارش")] public long? OrderId { get; set; }

            public int? TerminalNo { get; set; }

            [Display(Name = "شماره مرجع تراکنش")] public long? RRN { get; set; }

            [Display(Name = "کد وضعیت تراکنش")] public short? status { get; set; }

            [Display(Name = "شماره کارت هش")] public string HashCardNumber { get; set; }

            [Display(Name = "مبلغ")] public string Amount { get; set; }

            [Display(Name = "TSP Token")] public string TspToken { get; set; }
        }

        public partial class tbFactor
        {
            public int id { get; set; }
            public Nullable<long> token { get; set; }
            public Nullable<int> terminalNum { get; set; }
            public Nullable<long> rrn { get; set; }
            public Nullable<int> status { get; set; }
            public string hashCartNumber { get; set; }
            public Nullable<long> price { get; set; }
            public Nullable<bool> success { get; set; }
            public string describtion { get; set; }
        }

        [HttpPost]
        public async Task PaymentCallback(PaymentCallbackModel model)
        {
            if (model != null)
            {
                var tbfacotr = new tbFactor();

                if (model.status == Helper.ParsianPaymentGateway.Successful && (model.Token ?? 0L) > 0L)
                {
                    //ایجاد یک نمونه از سرویس تایید پرداخت
                    using (var confirmSvc = new ConfirmService())
                    {
                        confirmSvc.Url = Helper.ConfigHelper.ParsianPGWConfirmServiceUrl;

                        //ایجاد یک نمونه از نوع پارامتر سرویس تایید پرداخت
                        var confirmRequestData = new ClientConfirmRequestData();

                        //شناسه پذیرندگی باید در فراخوانی سرویس تایید تراکنش پرداخت ارائه شود
                        confirmRequestData.LoginAccount = Helper.ConfigHelper.LoginAccount;

                        //توکن باید ارائه شود
                        confirmRequestData.Token = model.Token ?? -1;

                        //فراخوانی سرویس و دریافت نتیجه فراخوانی
                        var confirmResponse = confirmSvc.ConfirmPayment(confirmRequestData);

                        //کنترل کد وضعیت نتیجه فراخوانی
                        //درصورتی که موفق باشد، باید خدمات یا کالا به کاربر پرداخت کننده ارائه شود
                        if (confirmResponse.Status == Helper.ParsianPaymentGateway.Successful)
                        {
                            // Convert the payment request to JSON
                            string jsonString = JsonConvert.SerializeObject(model);
                            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                            // Create HttpClient and send the POST request
                            using (var client = new HttpClient())
                            {
                                var response = await client.PostAsync($"https://yaasgold.ir/Payment/UpdateFactor/",
                                    content);
                            }
                  
                        }
                    }
                }

                if (model.status == Helper.ParsianPaymentGateway.CancelPay)
                {
                    string jsonString = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync("https://yaasgold.ir/Payment/UpdateFactor",
                            content);
                    }
                }

            }
        }

        public ActionResult Reverse(int idFacotr)
        {
            //ایجاد یک نمونه از سرویس برگشت وجه پرداخت شده
            using (var reversalSvc = new ReversalService())
            {
                // var db = new parsianEntities();
                // var tbfactor = (from r in db.tbFactors where r.id == idFacotr select r).SingleOrDefault();
                reversalSvc.Url = Helper.ConfigHelper.ReversalServiceUrl;

                var requestData = new ClientReversalRequestData()
                {
                    LoginAccount = Helper.ConfigHelper.LoginAccount,
                    // Token = tbfactor.token ?? -1
                };

                //فراخوانی متد برگشت وجه از نمونه وب سرویس برگشت وجه
                var response = reversalSvc.ReversalRequest(requestData);

                //کنترل کد وضعیت عملیات درخواست برگشت وجه
                //درصورت موفق بودن، می توانید از ارائه کالا و یا خدمات درخواستی کاربر به او انصراف دهید
                if (response.Status == Helper.ParsianPaymentGateway.Successful)
                {
                    // tbfactor.status = response.Status;
                    // tbfactor.describtion = "بازگشت وجه";
                    // tbfactor.success = false;
                    // db.SaveChanges();
                    // ViewBag.msg = "بازگشت وجه انجام شد.";

                    //reversal was successful
                }
                else
                {
                    ViewBag.msg = response.Message;
                }
                // var tbfactors = from r in db.tbFactors select r;


                return View("ShowFactor");
            }
        }
    }
}