using Budget.Core.Entities;

namespace Budget.Core.Models.PaymentTypes
{
    public class PaymentTypeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static PaymentTypeModel FromPaymentType(PaymentType paymentType)
        {
            return new PaymentTypeModel
            {
                Id = paymentType.Id,
                Name = paymentType.Name,
            };
        }
    }
}
