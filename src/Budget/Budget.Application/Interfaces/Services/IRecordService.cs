using Budget.Application.Models.Pagination;
using Budget.Application.Models.Records;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Application.Interfaces.Services
{
    public interface IRecordService
    {
        Task<RecordModel> GetByIdAsync(int id, string userId);

        /// <summary>
        /// Gets the record for update. In case of updating a transfer record, returning the positive among the two records.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<RecordModel> GetByIdForUpdateAsync(int id, string userId);

        Task<IEnumerable<RecordsExportModel>> GetAllForExportAsync(string userId);

        Task<PaginationModel<RecordsGroupModel>> GetAllPaginatedAsync(PaginatedRequestModel requestModel, string userId);

        Task<RecordModel> CreateAsync(CreateRecordModel createRecordModel, string userId);

        Task<RecordModel> UpdateAsync(UpdateRecordModel updateRecordModel, string userId);

        Task<RecordModel> DeleteAsync(int recordId, string userId);

        Task<RecordsDateRangeModel> GetRecordsDateRangeAsync(string userId);
    }
}
