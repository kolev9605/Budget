using Budget.Domain.Constants;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.PaymentTypes;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Budget.Application.PaymentTypes.Queries;

public record GetAllPaymentTypesQuery() : IRequest<ErrorOr<IEnumerable<PaymentTypeModel>>>;

public class GetAllPaymentTypesQueryHandler : IRequestHandler<GetAllPaymentTypesQuery, ErrorOr<IEnumerable<PaymentTypeModel>>>
{
    private readonly IPaymentTypeRepository _paymentTypeRepository;
    private readonly ICacheManager _cacheManager;

    public GetAllPaymentTypesQueryHandler(IPaymentTypeRepository paymentTypeRepository, ICacheManager cacheManager)
    {
        _paymentTypeRepository = paymentTypeRepository;
        _cacheManager = cacheManager;
    }

    public async Task<ErrorOr<IEnumerable<PaymentTypeModel>>> Handle(GetAllPaymentTypesQuery request, CancellationToken cancellationToken)
    {
        // TODO: Caching Mediatr behavior can be implemented
        return (await _cacheManager.GetOrCreateAsync(
            CacheConstants.PaymentTypes.Key,
            CacheConstants.PaymentTypes.ExpirationInSeconds,
            _paymentTypeRepository.GetAllAsync)).ToErrorOr();
    }
}
