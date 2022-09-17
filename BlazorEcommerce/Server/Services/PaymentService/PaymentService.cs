using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService;

public class PaymentService : IPaymentService
{
    public PaymentService()
    {
        StripeConfiguration.ApiKey = "sk_test_yEAXk9pRtjYZ6m3sdVEnSuRc00vgBnmsfe";
    }
    public Task<Session> CreateCheckoutSession()
    {
        throw new NotImplementedException();
    }
}