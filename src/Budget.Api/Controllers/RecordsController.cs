using Budget.Api.Models.Records;
using Budget.Application.Records.Commands;
using Budget.Application.Records.Queries;
using Budget.Common;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;
using Budget.Domain.Models.Records.Statistics;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Budget.Api.Controllers;

public class RecordsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly Kernel _kernel;
    private readonly IRecordRepository _recordRepository;
    private readonly IQuickAddService _quickAddService;
    private readonly ICategoryRepository _categoryRepository;

    public RecordsController(
        IMediator mediator,
        Kernel kernel,
        IRecordRepository recordRepository,
        IQuickAddService quickAddService,
        ICategoryRepository categoryRepository)
    {
        _mediator = mediator;
        _kernel = kernel;
        _recordRepository = recordRepository;
        _quickAddService = quickAddService;
        _categoryRepository = categoryRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var request = new GetRecordByIdRequest(id);
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetRecordByIdQuery>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPaginated([FromQuery] GetAllRecordsRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetAllRecordsQuery>());

        return MatchResponse<IPagedListContainer<RecordModel>, IPagedListContainer<RecordResponse>>(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRecordRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<CreateRecordCommand>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateRecordRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<UpdateRecordCommand>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete([FromQuery] DeleteRecordRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<DeleteRecordCommand>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpGet("Types")]
    public IActionResult GetRecordTypes()
        => Ok(EnumHelpers.GetListFromEnum<RecordType>());

    [HttpGet]
    [Route(nameof(GetRecordsDateRange))]
    public async Task<IActionResult> GetRecordsDateRange()
    {
        var result = await _mediator.Send(CurrentUser.Adapt<GetRecordsDateRangeQuery>());

        return MatchResponse(result);
    }

    [HttpGet("totalbalance")]
    public async Task<IActionResult> GetTotalBalance()
    {
        var result = await _mediator.Send(new GetTotalBalanceQuery(CurrentUser.Id));

        return MatchResponse<decimal, decimal>(result);
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetCashFlow([FromQuery] GetRecordsStatisticsRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetRecordsStatisticsQuery>());

        return MatchResponse<IEnumerable<GetRecordsStatisticsResult>, IEnumerable<GetRecordsStatisticsResponse>>(result);
    }

    [HttpPost("ask")]
    [AllowAnonymous]
    public async Task<IActionResult> Ask([FromBody] string prompt)
    {
        // var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        // var history = new ChatHistory();
        // history.AddUserMessage(prompt);

        // OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        // {
        //     FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        // };

        // var result = await chatService.GetChatMessageContentAsync(history,
        //     openAIPromptExecutionSettings, _kernel);
        var last100records = await _recordRepository.GetLastRecordsAsync("d8f5bbe2-888b-42e8-bbe0-f3a6bec4e37e", 1000);
        var historyCsv = string.Join(Environment.NewLine, last100records.Select(r => r.ToCsv()));

        var categories = await _categoryRepository.GetAllAsync("d8f5bbe2-888b-42e8-bbe0-f3a6bec4e37e");
        var categoriesCsv = string.Join(", ", categories.Select(c => c.Name));

        var response = await _quickAddService.ParseQuickAddAsync(prompt, historyCsv, categoriesCsv);

        return Ok(response);
    }
}
