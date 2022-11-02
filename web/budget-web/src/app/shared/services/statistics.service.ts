import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ErrorService } from './error.service';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';
import { StatisticsResultModel } from '../models/statistics/statistics-request.model';
import { StatisticsRequestModel } from '../models/statistics/statistics-response.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StatisticsService {
  private controllerName: string = 'Statistics';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getStatistics(request: StatisticsRequestModel): Observable<StatisticsResultModel> {
    return this.http
      .post<StatisticsResultModel>(
        `${environment.apiUrl}${this.controllerName}/GetStatistics`,
        request,
      )
      .pipe(catchError(this.errorService.handleError));
  }
}
