import { CashFlowItemModel } from './cash-flow-item.model';

export class CashFlowChartModel {
  public startDate: Date;
  public endDate: Date;
  public balance: number;
  public items: CashFlowItemModel[];
}
