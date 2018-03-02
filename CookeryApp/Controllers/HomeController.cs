using CookeryApp.Entities;
using CookeryApp.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CookeryApp.Controllers
{
    public class HomeController : Controller
    {
        //injeção de depenência do DB
        public HomeController()
        {
            _db = new AppContextDB();
        }
        private AppContextDB _db;


        [HttpGet]
        public ActionResult Index()
        {
            //popula IVM com list de eventos
            var model = new IndexVM
            {
                Events = _db.Events.ToList()
            };
            //flag para layout da page, muda ordem do texto e imagem
            Session["flag"] = "false";

            //return event list
            return View(model);
        }

        [HttpGet]
        public ActionResult Purchase(int? id)
        {
            //recebe event, trocar 1 pelo param id
            var ev = _db.Events.FirstOrDefault(m => m.Id == 1);

            var model = new PurchaseVM()
            {
                events = ev
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Purchase(PurchaseVM model)
        {
            if (ModelState.IsValid)
            {
                //recebe o event
                var ev = _db.Events.FirstOrDefault(m => m.Id == model.events.Id);
                
                //cria ticket
                var ticket = new Ticket()
                {
                    FName = model.ticket.FName,
                    LName = model.ticket.LName,
                    Email = model.ticket.Email,
                    Event = ev
                };

                _db.Tickets.Add(ticket);
                _db.SaveChanges();

                //cria objeto pagamento
                var payment = new Payment
                {
                    //id gerado pelo controller WebExperience
                    experience_profile_id = "XP-S55K-F7UY-BEM8-RXG8",
                    intent = "sale",
                    payer = new Payer
                    {
                        payment_method = "paypal"
                    },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = "User: cookeryappninja-buyer@gmail.com Senha:mar982836",
                            amount = new Amount
                            {
                                currency = "BRL",
                                total = ev.Price.ToString()
                            },
                            item_list = new ItemList()
                            {
                                items = new List<Item>()
                                {
                                    new Item
                                    {
                                        description = $"Evento: {ev.Title} Preço: {ev.Price.ToString("c")}",
                                        currency = "BRL",
                                        quantity = "1",
                                        price = ev.Price.ToString()
                                    }
                                }
                            }
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = Url.Action("Return", "Home",null,Request.Url.Scheme),
                        cancel_url = Url.Action("Cancel","Home",null, Request.Url.Scheme)
                    }
                };

                //cria api context do paypal com as credências(paypal developer site) em web.config
                var apiContext = GetAPIContext();

                //envia o objeto pagamento para o paypal api
                var createdPayment = payment.Create(apiContext);

                //salva a referência do pagamento no ticket, referência criada pelo paypal api, e salva no DB
                ticket.PaypalReference = createdPayment.id;
                _db.SaveChanges();

                //procura a url no createdPayment(objeto pagamento paypal), o mesmo retorna Return ou Cancel, se ok envia para page de login do paypal :)
                var approvalUrl = createdPayment.links.FirstOrDefault(m => m.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));

                //envia tela para validar dados do user, e finalizar compra com o paypal
                return Redirect(approvalUrl.href);
            }
            else
            {
                var pvm = new PurchaseVM
                {
                    events = _db.Events.Find(model.events.Id)
                };

                return View(pvm);
            }

        }

        public ActionResult Return(string payerId, string paymentId)
        {
            //busca o ticket criado pela referência do pagamento no ticket PaypalReference
            var ticket = _db.Tickets.FirstOrDefault(m => m.PaypalReference == paymentId);
            var ev = _db.Events.Find(ticket.Event_Id);
            //popula purchaseVM
    

            //obtém Paypal api context
            var apiContext = GetAPIContext();

            //configura o pagador com o pagamento
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            //cria payment para exe
            var payment = new Payment()
            {
                id = paymentId
            };

            //exe pagamento
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            //const com email do remetente, popula PVM com event e ticket para usar no email
            const string emailFrom = "cookeryappninja@gmail.com";
            var PVM = new PurchaseVM
            {
                events = ev,
                ticket = ticket
            };
            Mail(ticket.Email, emailFrom, PVM);

            //direciona para view thank's , end of the line 
            return RedirectToAction("ThankY");
        }

        public void Mail(string to, string from, PurchaseVM PVM)
        {
            //LOGIC para enviar senho no corpo o email, se email exist no DB
            string body;
            string sub = "Paypal API";

            //body do email escrito com html, concat indos da compra no meio do html, em <h4> com um pouco de css
            body = " <html> <head> 	<style>  *{ margin:0; 	padding:0; } body {   font-family: 'Roboto', Helvetica, Arial, sans-serif;   font-weight: 100;   font-size: 12px;   line-height: 30px;   height: 400px;   color: #777;   background: blue; }  .container {  	position:absolute; 	top:0; 	left:0; 	z-index:11; 	background-color:#000; 	width:100%; 	height:100%;  }  #contact input[type='text'], #contact input[type='email'], #contact input[type='tel'], #contact input[type='url'], #contact textarea, #contact button[type='submit'] {   font: 400 12px/16px 'Roboto', Helvetica, Arial, sans-serif; }  #contact {   background: #F9F9F9;   padding: 25px;   margin:  0;   box-shadow: 0 0 20px 0 rgba(0, 0, 0, 0.2), 0 5px 5px 0 rgba(0, 0, 0, 0.24); }  #contact h3 {   display: block;   font-size: 30px;   font-weight: 300;   margin-bottom: 10px; }  #contact h4 {   margin: 5px 0 15px;   display: block;   font-size: 13px;   font-weight: 400; }  fieldset {   border: medium none !important;   margin: 0 0 10px;   min-width: 100%;   padding: 0;   width: 100%; }  #contact input[type='text'], #contact input[type='email'], #contact input[type='tel'], #contact input[type='url'], #contact textarea {   width: 100%;   border: 1px solid #ccc;   background: #FFF;   margin: 0 0 5px;   padding: 10px; }  #contact input[type='text']:hover, #contact input[type='email']:hover, #contact input[type='tel']:hover, #contact input[type='url']:hover, #contact textarea:hover {   -webkit-transition: border-color 0.3s ease-in-out;   -moz-transition: border-color 0.3s ease-in-out;   transition: border-color 0.3s ease-in-out;   border: 1px solid #aaa; }  #contact textarea {   height: 100px;   max-width: 100%;   resize: none; }  #contact button[type='submit'] {   cursor: pointer;   width: 100%;   border: none;   background: green;   color: #FFF;   margin: 0 0 5px;   padding: 10px;   font-size: 15px; }  #contact button[type='submit']:hover {   background: #43A047;   -webkit-transition: background 0.3s ease-in-out;   -moz-transition: background 0.3s ease-in-out;   transition: background-color 0.3s ease-in-out; }  #contact button[type='submit']:active {   box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.5); }  .copyright {   text-align: center; }  #contact input:focus, #contact textarea:focus {   outline: 0;   border: 1px solid #aaa; }  ::-webkit-input-placeholder {   color: #888; }  :-moz-placeholder {   color: #888; }  ::-moz-placeholder {   color: #888; }  :-ms-input-placeholder {   color: #888; } 	</style> </head> <body> 	<div class='container'>  	  <div id='contact'> 		<h3>Descrição da compra feita com Paypal</h3> 		<h4>API Paypal com ASP.NET MVC e EF 6, somente para uma compra!</h4> <h4>Referência do pagamento(PaypalReference): "+ PVM.ticket.PaypalReference + "</h4>	<h4>Nome: " + PVM.ticket.FName+" "+ PVM.ticket.LName + "</h4> <h4>E-mail: " + PVM.ticket.Email + "</h4> <h4>Preço: " + PVM.events.Price + "</h4>	<hr /> 		  		<p class='copyright'>Consumindo API Paypal, GitHub: <a href='https://colorlib.com' target='_blank' title='Colorlib'>Cookery Ninja</a></p> 	  </div> 	</div> </body> </html>";

            //Instância classe email
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from);
            mail.Subject = sub;
            mail.Body = body;
            mail.IsBodyHtml = true;

            //Instância smtp do servidor, neste caso o gmail.
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential
            ("cookeryappninja@gmail.com", "mar142536");// Login e senha do e-mail.
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(mail);
            }
            catch (SmtpException smtpNotFound)
            {
                Console.WriteLine("Server hostname is invalid: " + smtpNotFound.Message);
            }
        }

        public ActionResult Cancel()
        {
            return View();
        }

        public ActionResult ThankY(int? id)
        {
            return View();
        }

        private APIContext GetAPIContext()
        {
            //authenticate for paypal ninja :P
            var config = ConfigManager.Instance.GetProperties();
            var accessToekn = new OAuthTokenCredential(config).GetAccessToken();
            var apicontext = new APIContext(accessToekn);

            return apicontext;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}