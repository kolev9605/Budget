using Budget.Core.Models.Pagination;
using Budget.Core.Models.Records;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
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

        Task<IEnumerable<RecordsGroupModel>> GetAllAsync(string userId);

        Task<PaginationModel<RecordsGroupModel>> GetAllPaginatedAsync(PaginatedRequestModel requestModel, string userId);

        Task<int> CreateAsync(CreateRecordModel createRecordModel, string userId);

        Task<int> UpdateAsync(UpdateRecordModel updateRecordModel, string userId);

        Task<int> DeleteAsync(int recordId, string userId);

        Task<RecordsDateRangeModel> GetRecordsDateRangeAsync(string userId);
    }
}
