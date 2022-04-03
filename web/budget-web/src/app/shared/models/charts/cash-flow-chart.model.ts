import { CashFlowItemModel } from './cash-flow-item.model';

export class CashFlowChartModel {
  public startDate: Date;
  public endDate: Date;
  public cashFlowForPeriod: number;
  public items: CashFlowItemModel[];
}
