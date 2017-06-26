﻿namespace Magicodes.Admin.MultiTenancy.Payments.Cache
{
    public interface IPaymentCache
    {
        PaymentCacheItem GetCacheItemOrNull(SubscriptionPaymentGatewayType gateway, string paymentId);

        void AddCacheItem(PaymentCacheItem item);
    }
}
