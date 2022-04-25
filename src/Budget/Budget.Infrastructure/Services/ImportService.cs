﻿using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Records;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
{
    public class ImportService : IImportService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRepository<PaymentType> _paymentTypeRepository;
        private readonly IRecordRepository _recordRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ImportService(
            IAccountRepository accountRepository, 
            IRepository<PaymentType> paymentTypeRepository, 
            IRecordRepository recordRepository, 
            ICategoryRepository categoryRepository)
        {
            _accountRepository = accountRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _recordRepository = recordRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task ImportRecords(string recordsFileJson)
        {
            var records = JsonConvert.DeserializeObject<IEnumerable<RecordsExportModel>>(recordsFileJson);
            var accounts = await _accountRepository.BaseAllAsync();
            var paymentTypes = await _paymentTypeRepository.BaseAllAsync();
            var categories = await _categoryRepository.BaseAllAsync();

            var counter = 0;
            foreach (var recordModel in records)
            {
                var record = RecordsExportModel.ToRecord(recordModel);

                var account = accounts.FirstOrDefault(a => a.Name == recordModel.Account);
                if (account == null)
                {
                    throw new System.Exception();
                }

                if (recordModel.FromAccount != null)
                {
                    var fromAccount = accounts.FirstOrDefault(a => a.Name == recordModel.FromAccount);
                    if (fromAccount == null)
                    {
                        throw new System.Exception();
                    }

                    record.FromAccount = fromAccount;
                }

                var paymentType = paymentTypes.FirstOrDefault(pt => pt.Name == recordModel.PaymentType);
                if (paymentType == null)
                {
                    throw new System.Exception();
                }

                var category = categories.FirstOrDefault(pt => pt.Name == recordModel.Category);
                if (category == null)
                {
                    throw new System.Exception();
                }

                record.Account = account;
                record.PaymentType = paymentType;
                record.Category = category;

                await _recordRepository.CreateAsync(record, saveChanges: false);

                counter++;
                if (counter % 500 == 0)
                {
                    await _recordRepository.SaveChangesAsync();
                }
            }
        }
    }
}