using Budget.Core.Entities;
using Budget.Core.Models.PaymentTypes;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Infrastructure.Factories
{
    public static class PaymentTypeFactory
    {
        public static PaymentTypeModel ToPaymentTypeModel(this PaymentType paymentType)
        {
            if (paymentType == null) return null;

            return new PaymentTypeModel
            {
                Id = paymentType.Id,
                Name = paymentType.Name,
            };
        }

        public static IEnumerable<PaymentTypeModel> ToPaymentTypeModels(this IEnumerable<PaymentType> paymentTypes)
        {
            if (paymentTypes == null) return Enumerable.Empty<PaymentTypeModel>();

            return paymentTypes.Select(pt => pt.ToPaymentTypeModel());
        }
    }
}
