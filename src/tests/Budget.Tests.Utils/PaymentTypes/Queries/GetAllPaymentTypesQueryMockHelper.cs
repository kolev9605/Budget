using Budget.Application.PaymentTypes.Queries;
using Budget.Domain.Models.PaymentTypes;
using Mapster;

namespace Budget.Tests.Utils.PaymentTypes.Queries;

public static class GetAllPaymentTypesQueryMockHelper
{
    // TODO: Accept the arguments
    public static GetAllPaymentTypesQueryHandler SetupHandler()
    {
        var paymentType = EntityMockHelper.SetupPaymentType();

        var paymentTypes = new List<PaymentTypeModel>() { paymentType.Adapt<PaymentTypeModel>() };
        var handler = new GetAllPaymentTypesQueryHandler(
            RepositoryMockHelper.SetupPaymentTypeRepository(paymentType),
            ServiceMockHelper.SetupCacheManager(paymentTypes.AsEnumerable()));

        return handler;
    }

    public static GetAllPaymentTypesQuery SetupQuery()
    {
        var command = new GetAllPaymentTypesQuery();

        return command;
    }
}
