using Budget.Domain.Common.Errors;
using Budget.Domain.Interfaces.Repositories;
using ErrorOr;
using MediatR;

namespace Budget.Application.Accounts.Commands;

public record ToggleAccountActivationStatusCommand(Guid Id) : IRequest<ErrorOr<ToggleAccountActivationStatusCommandResult>>
{
    public Guid Id { get; init; } = Id;
}

public record ToggleAccountActivationStatusCommandResult();

public class ToggleAccountActivationStatusCommandHandler : IRequestHandler<ToggleAccountActivationStatusCommand, ErrorOr<ToggleAccountActivationStatusCommandResult>>
{
    private readonly IAccountRepository _accountRepository;

    public ToggleAccountActivationStatusCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<ToggleAccountActivationStatusCommandResult>> Handle(ToggleAccountActivationStatusCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.BaseGetByIdAsync(request.Id);
        if (account == null)
        {
            return Errors.Account.NotFound;
        }

        if (!account.IsActive)
        {
            account.Activate();
        }
        else
        {
            account.Deactivate();
        }

        await _accountRepository.SaveChangesAsync();

        return new ToggleAccountActivationStatusCommandResult();
    }
}
