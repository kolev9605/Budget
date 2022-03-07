export class CreateRecordModel {
  constructor(
    public note: string,
    public amount: number,
    public accountId: number,
    public categoryId: number,
    public paymentTypeId: number,
    public recordType: string,
  ) {}
}
