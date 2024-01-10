using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.API.Controllers
{
    [Authorize]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _WHSecret = "whsec_395c7039b05ad3930fc7412d0e923c9976011149eb67994fa06df41ebc4b8dff";
        public PaymentsController(IPaymentService paymentService , ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (basket == null) return BadRequest(new ApiErrorResponse(400, "A Problem happened with your basket!"));
            return Ok(basket);
        }

        [HttpPost("webhook")] //api/Payments/webhook
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
            var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _WHSecret);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                Order order;

                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        order = await  _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                        _logger.LogInformation("Payment Succeeded BABYYYY!", paymentIntent.Id);
                        break;
                        
                    case Events.PaymentIntentPaymentFailed:
                        order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                        _logger.LogInformation("Payment Failed ! :( ", paymentIntent.Id);
                        break;

                }

                return new EmptyResult();
        }

    }
}
