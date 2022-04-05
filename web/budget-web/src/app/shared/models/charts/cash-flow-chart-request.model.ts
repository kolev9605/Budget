export class CashFlowChartRequestModel {
  constructor(public startDate: Date, public endDate: Date, public accountIds: number[]) {}
}
