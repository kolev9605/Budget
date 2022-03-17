export class UpdateRecordModel {
  constructor(
    public id: number,
    public note: string,
    public amount: number,
    public accountId: number,
    public categoryId: number,
    public paymentTypeId: number,
    public recordType: string,
    public recordDate: Date,
  ) {}
}
