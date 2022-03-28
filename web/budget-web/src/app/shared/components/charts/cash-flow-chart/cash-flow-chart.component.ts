import { ChangeDetectorRef, Component, OnInit, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CashFlowChartModel } from 'src/app/shared/models/charts/cash-flow-chart.model';
import { CashFlowItemModel } from 'src/app/shared/models/charts/cash-flow-item.model';
import { ChartService } from 'src/app/shared/services/chart.service';
import { ChartColors } from '../../../constants';
import { addMonths, startOfMonth } from 'date-fns';
import { from, Subscription } from 'rxjs';
import { concatMap, tap } from 'rxjs/operators';

@Component({
  selector: 'app-cash-flow-chart',
  templateUrl: './cash-flow-chart.component.html',
  styleUrls: ['./cash-flow-chart.component.scss'],
})
export class CashFlowChartComponent implements OnInit {
  isLoading: boolean;
  cashFlowData: CashFlowChartModel;
  chartDate: Date;
  cashFlowDataSubscription: Subscription;
  monthEventEmitter: EventEmitter<number> = new EventEmitter();
  data: any;
  options: any;
  type: any;

  constructor(private chartService: ChartService, private toastr: ToastrService) {}

  ngOnInit(): void {
    console.log('oninit cash flow');
    this.chartDate = startOfMonth(new Date());

    this.cashFlowDataSubscription = from(this.monthEventEmitter)
      .pipe(
        tap(() => (this.isLoading = true)),
        concatMap((month) => this.chartService.getCashFlowData(month)),
      )
      .subscribe(
        (response) => {
          this.cashFlowData = response;
          this.data = this.getData(this.cashFlowData.items);
          this.options = this.getOptions();
          this.type = this.getType();
        },
        (error) => {
          this.toastr.error(error);
        },
        () => {
          this.isLoading = false;
        },
      );

    this.loadData();
  }

  loadData(): void {
    this.monthEventEmitter.emit(this.chartDate.getMonth() + 1);
  }

  nextMonth(): any {
    this.chartDate = addMonths(this.chartDate, 1);
    this.loadData();
  }

  previousMonth(): any {
    this.chartDate = addMonths(this.chartDate, -1);
    this.loadData();
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

  ngOnDestroy() {
    this.cashFlowDataSubscription.unsubscribe();
  }
}
