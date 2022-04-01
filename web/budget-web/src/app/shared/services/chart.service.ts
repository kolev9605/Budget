import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CashFlowChartRequestModel } from '../models/charts/cash-flow-chart-request.model';
import { CashFlowChartModel } from '../models/charts/cash-flow-chart.model';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root',
})
export class ChartService {
  private controller: string = 'Chart';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getCashFlowData(requestModel: CashFlowChartRequestModel): Observable<CashFlowChartModel> {
    return this.http
      .post<CashFlowChartModel>(
        `${environment.apiUrl}${this.controller}/GetCashFlowData`,
        requestModel,
      )
      .pipe(catchError(this.errorService.handleError));
  }
}
