using Braintree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Utility
{
    public interface IBrainTreeGate
    {
        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateway();
    }
}
