import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root',
})
export class ImportService {
  private controller: string = 'Import';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  importRecords(file: File): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('uploadFile', file, file.name);
    return this.http
      .post(`${environment.apiUrl}${this.controller}/ImportRecords`, formData)
      .pipe(catchError(this.errorService.handleError));
  }

  importWalletRecords(file: File): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('uploadFile', file, file.name);
    return this.http
      .post(`${environment.apiUrl}${this.controller}/ImportWalletRecords`, formData)
      .pipe(catchError(this.errorService.handleError));
  }
}
