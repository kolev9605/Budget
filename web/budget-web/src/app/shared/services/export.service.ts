import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root',
})
export class ExportService {
  private controller: string = 'Export';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  exportRecords(): Observable<any> {
    return this.http
      .get<any>(`${environment.apiUrl}${this.controller}/ExportRecords`)
      .pipe(catchError(this.errorService.handleError));
  }
}
