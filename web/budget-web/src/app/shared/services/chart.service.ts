import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CashFlowChartModel } from '../models/charts/cash-flow-chart.model';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root',
})
export class ChartService {
  private controller: string = 'Chart';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getCashFlowData(month: number): Observable<CashFlowChartModel> {
    return this.http
      .get<CashFlowChartModel>(`${environment.apiUrl}${this.controller}/GetCashFlowData`, {
        params: {
          month: month,
        },
      })
      .pipe(catchError(this.errorService.handleError));
  }
}
