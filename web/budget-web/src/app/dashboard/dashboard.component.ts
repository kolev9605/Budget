import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountModel } from '../shared/models/accounts/account.model';
import { AccountService } from '../shared/services/account.service';
import { Observable, Subject, Subscription } from 'rxjs';
import { CashFlowChartRequestModel } from '../shared/models/charts/cash-flow-chart-request.model';
import { addMonths, startOfMonth } from 'date-fns';
import { concatMap, tap } from 'rxjs/operators';
import { ChartService } from '../shared/services/chart.service';
import { CashFlowChartModel } from '../shared/models/charts/cash-flow-chart.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  accounts: AccountModel[];
  selectedAccountIds: number[];
  isLoading: boolean = false;
  selectedDate: Date;
  cashFlowData: CashFlowChartModel;
  cashFlowDataSubscription: Subscription;
  cashFlowRequestSubject: Subject<CashFlowChartRequestModel> = new Subject();
  cashFlowDateObservable: Observable<CashFlowChartModel> = new Observable();
  accountsSubscription: Subscription = new Subscription();

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router,
    private chartService: ChartService,
  ) {}

  ngOnInit(): void {
    this.selectedDate = startOfMonth(new Date());

    this.cashFlowDateObservable = this.cashFlowRequestSubject.pipe(
      concatMap((request) => this.chartService.getCashFlowData(request)),
    );

    this.cashFlowDateObservable.pipe(tap(() => (this.isLoading = true))).subscribe(
      (response) => {
        this.cashFlowData = response;
        this.isLoading = false;
      },
      (error) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    );

    this.accountsSubscription = this.accountService
      .getAll()
      .pipe(tap(() => (this.isLoading = true)))
      .subscribe(
        (response) => {
          this.isLoading = false;
          this.accounts = response;
          this.selectedAccountIds = response.map((a) => a.id);
          this.loadData();
        },
        (error) => {
          this.isLoading = false;
          this.toastr.error(error);
        },
      );
  }

  onAddAccountPressed(): void {
    this.router.navigate(['accounts/create']);
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
    this.selectedDate = addMonths(this.selectedDate, 1);
    this.loadData();
  }

  previousMonth(): any {
    this.selectedDate = addMonths(this.selectedDate, -1);
    this.loadData();
  }

  loadData(): void {
    const requestModel = new CashFlowChartRequestModel(
      this.selectedDate.getMonth() + 1,
      this.selectedAccountIds,
    );

    this.cashFlowRequestSubject.next(requestModel);
  }

  onDestroy(): void {
    this.cashFlowRequestSubject.unsubscribe();
    this.accountsSubscription.unsubscribe();
  }
}
