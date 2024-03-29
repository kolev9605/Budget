import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountModel } from '../shared/models/accounts/account.model';
import { AccountService } from '../shared/services/account.service';
import { forkJoin, Observable, Subject, Subscription } from 'rxjs';
import { CashFlowChartRequestModel } from '../shared/models/charts/cash-flow-chart-request.model';
import { addMonths, startOfMonth, endOfMonth } from 'date-fns';
import { concatMap, tap } from 'rxjs/operators';
import { ChartService } from '../shared/services/chart.service';
import { CashFlowChartModel } from '../shared/models/charts/cash-flow-chart.model';
import { RecordService } from '../shared/services/record.service';
import { RecordsDateRangeModel } from '../shared/models/records/records-date-range.model';
import { StatisticsService } from '../shared/services/statistics.service';
import { StatisticsResultModel } from '../shared/models/statistics/statistics-request.model';
import { StatisticsRequestModel } from '../shared/models/statistics/statistics-response.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: [],
})
export class DashboardComponent implements OnInit {
  accounts: AccountModel[];
  selectedAccountIds: number[];
  isLoading: boolean = false;
  selectedDate: Date;
  cashFlowData: CashFlowChartModel;
  statistics: StatisticsResultModel;

  cashFlowDataSubscription: Subscription;
  cashFlowRequestSubject: Subject<CashFlowChartRequestModel> = new Subject();
  cashFlowDateObservable: Observable<CashFlowChartModel> = new Observable();
  statisticsObservable: Observable<StatisticsResultModel> = new Observable();
  accountsSubscription: Subscription = new Subscription();
  recordsDateRange: RecordsDateRangeModel;
  hasPreviousMonth: boolean;
  hasNextMonth: boolean;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router,
    private chartService: ChartService,
    private recordService: RecordService,
    private statisticsService: StatisticsService,
  ) {}

  ngOnInit(): void {
    this.selectedDate = startOfMonth(new Date());
    this.isLoading = true;

    this.cashFlowDateObservable = this.cashFlowRequestSubject.pipe(
      tap(() => (this.isLoading = true)),
      concatMap((request) => this.chartService.getCashFlowData(request)),
      tap(() => (this.isLoading = false)),
    );

    this.cashFlowDateObservable.subscribe({
      next: (response) => {
        this.cashFlowData = response;
      },
      error: (error) => {
        this.toastr.error(error);
      },
    });

    this.statisticsObservable = this.cashFlowRequestSubject.pipe(
      tap(() => (this.isLoading = true)),
      concatMap((request) =>
        this.statisticsService.getStatistics(
          new StatisticsRequestModel(request.startDate, request.endDate, request.accountIds),
        ),
      ),
      tap(() => (this.isLoading = false)),
    );

    this.statisticsObservable.subscribe({
      next: (response) => {
        this.statistics = response;
      },
      error: (error) => {
        this.toastr.error(error);
      },
    });

    forkJoin({
      accounts: this.accountService.getAll(),
      recordsDateRange: this.recordService.getRecordsDateRange(),
    })
      .pipe(tap(() => (this.isLoading = true)))
      .subscribe({
        next: ({ accounts, recordsDateRange }) => {
          this.accounts = accounts;
          this.selectedAccountIds = accounts.map((a) => a.id);

          this.recordsDateRange = recordsDateRange;
          if (this.recordsDateRange) {
            this.selectedDate = startOfMonth(new Date(this.recordsDateRange.maxDate));
            this.calculateHasNextMonth();
            this.calculateHasPreviousMonth();
            this.loadData();
          }
        },
        error: (error) => {
          this.toastr.error(error);
        },
      })
      .add(() => (this.isLoading = false));
  }

  onAccountSelected(accountId: number) {
    if (this.selectedAccountIds.some((a) => a == accountId)) {
      this.selectedAccountIds = this.selectedAccountIds.filter((a) => a !== accountId);
    } else {
      this.selectedAccountIds = [...this.selectedAccountIds, accountId];
    }

    this.loadData();
  }

  isAccountSelected(accountId: number): boolean {
    return this.selectedAccountIds.some((a) => a == accountId);
  }

  nextMonth(): any {
    if (!this.hasNextMonth) {
      return;
    }

    this.selectedDate = addMonths(this.selectedDate, 1);
    this.calculateHasPreviousMonth();
    this.calculateHasNextMonth();

    this.loadData();
  }

  calculateHasNextMonth(): void {
    const nextMonth = startOfMonth(addMonths(this.selectedDate, 1));
    const maxRecordDate = startOfMonth(new Date(this.recordsDateRange.maxDate));

    this.hasNextMonth = nextMonth <= maxRecordDate;
  }

  previousMonth(): any {
    if (!this.hasPreviousMonth) {
      return;
    }

    this.selectedDate = addMonths(this.selectedDate, -1);
    this.calculateHasPreviousMonth();
    this.calculateHasNextMonth();

    this.loadData();
  }

  calculateHasPreviousMonth() {
    const previousMonth = startOfMonth(addMonths(this.selectedDate, -1));
    const minRecordDate = startOfMonth(new Date(this.recordsDateRange.minDate));

    this.hasPreviousMonth = previousMonth >= minRecordDate;
  }

  loadData(): void {
    const requestModel = new CashFlowChartRequestModel(
      startOfMonth(this.selectedDate),
      endOfMonth(this.selectedDate),
      this.selectedAccountIds,
    );

    this.cashFlowRequestSubject.next(requestModel);
  }

  onDestroy(): void {
    this.cashFlowRequestSubject.unsubscribe();
    this.accountsSubscription.unsubscribe();
  }
}
