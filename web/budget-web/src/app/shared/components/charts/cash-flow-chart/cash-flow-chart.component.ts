import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { endOfMonth, startOfMonth } from 'date-fns';
import { ToastrService } from 'ngx-toastr';
import { CashFlowChartModel } from 'src/app/shared/models/charts/cash-flow-chart.model';
import { CashFlowItemModel } from 'src/app/shared/models/charts/cash-flow-item.model';
import { ChartService } from 'src/app/shared/services/chart.service';
import { ChartColors, Formats } from '../../../constants';

@Component({
  selector: 'app-cash-flow-chart',
  templateUrl: './cash-flow-chart.component.html',
  styleUrls: ['./cash-flow-chart.component.scss'],
})
export class CashFlowChartComponent implements OnInit, OnChanges {
  @Input() cashFlowData: CashFlowChartModel;
  data: any;
  options: any;
  type: any;

  constructor() {}
  ngOnChanges(changes: SimpleChanges): void {
    this.loadChart();
  }

  ngOnInit(): void {
    this.loadChart();
  }

  loadChart() {
    if (this.cashFlowData) {
      this.data = this.getData(this.cashFlowData.items);
      this.options = this.getOptions();
      this.type = this.getType();
    }
  }

  getData(items: CashFlowItemModel[]): any {
    const firstDayOfCurrentMonth = startOfMonth(new Date(this.cashFlowData.startDate));
    items.splice(0, 0, { sum: 0, date: firstDayOfCurrentMonth });

    let data = {
      labels: [],
      datasets: [
        {
          label: 'Cash Flow',
          data: items,
          backgroundColor: ChartColors.Accent,
          borderColor: ChartColors.Accent,
        },
      ],
    };
    return data;
  }

  getOptions(): any {
    const firstDayOfCurrentMonth = startOfMonth(new Date(this.cashFlowData.startDate));
    const lastDayOfCurrentMonth = endOfMonth(new Date(this.cashFlowData.endDate));

    let options = {
      responsive: true,
      aspectRatio: 1.7,
      scales: {
        x: {
          type: 'time',
          time: {
            unit: 'week',
            tooltipFormat: Formats.DateFormt,
          },
          min: firstDayOfCurrentMonth,
          max: lastDayOfCurrentMonth,
        },
      },
      parsing: {
        xAxisKey: 'date',
        yAxisKey: 'sum',
      },
    };

    return options;
  }

  getType(): any {
    return 'line';
  }
}
