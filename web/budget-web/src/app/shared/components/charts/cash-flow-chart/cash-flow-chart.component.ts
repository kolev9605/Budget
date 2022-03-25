import { ChangeDetectorRef, Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CashFlowChartModel } from 'src/app/shared/models/charts/cash-flow-chart.model';
import { CashFlowItemModel } from 'src/app/shared/models/charts/cash-flow-item.model';
import { ChartService } from 'src/app/shared/services/chart.service';
import { ChartColors } from '../../../constants';
import { addMonths, format, startOfMonth } from 'date-fns';

@Component({
  selector: 'app-cash-flow-chart',
  templateUrl: './cash-flow-chart.component.html',
  styleUrls: ['./cash-flow-chart.component.scss'],
})
export class CashFlowChartComponent implements OnInit {
  isLoading: boolean;
  cashFlowData: CashFlowChartModel;
  chartDate: Date;
  monthName: string;
  monthNumber: number;

  data: any;
  options: any;
  type: any;

  constructor(
    private chartService: ChartService,
    private toastr: ToastrService,
    private chRef: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    console.log('oninit cash flow');

    this.chartDate = startOfMonth(new Date());
    this.loadChart();
  }

  getData(items: CashFlowItemModel[]): any {
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
    let options = {
      responsive: true,
      aspectRatio: 1.7,
      scales: {
        x: {
          type: 'time',
          time: {
            unit: 'week',
          },
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

  loadChart(): void {
    this.monthName = format(this.chartDate, 'LLLL yyyy');
    this.monthNumber = +format(this.chartDate, 'M');

    this.isLoading = true;

    this.chartService.getCashFlowData(this.monthNumber).subscribe(
      (response) => {
        this.isLoading = false;

        this.cashFlowData = response;

        this.data = this.getData(this.cashFlowData.items);
        this.options = this.getOptions();
        this.type = this.getType();

        this.chRef.detectChanges();
      },
      (error) => {
        this.isLoading = false;

        this.toastr.error(error);
      },
    );
  }

  nextMonth(): any {
    this.chartDate = addMonths(this.chartDate, 1);
    this.loadChart();
  }

  previousMonth(): any {
    this.chartDate = addMonths(this.chartDate, -1);
    this.loadChart();
  }
}
