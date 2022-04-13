export class StatisticsRequestModel {
  constructor(public startDate: Date, public endDate: Date, public accountIds: number[]) {}
}
