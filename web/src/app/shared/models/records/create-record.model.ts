export class CreateRecordModel {
  constructor(
    public note: string,
    public amount: number,
    public accountId: string,
    public categoryId: string,
    public paymentTypeId: string,
    public recordType: string,
    public recordDate: Date,
    public fromAccountId?: string,
  ) {}
}
