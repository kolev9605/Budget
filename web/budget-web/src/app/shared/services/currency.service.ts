import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ErrorService } from './error.service';
import { environment } from '../../../environments/environment';
import { CurrencyModel } from '../models/currencies/currency.model';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CurrencyService {
  private controllerName: string = 'Currency';
  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getAll() {
    return this.http
      .get<CurrencyModel[]>(environment.apiUrl + this.controllerName + '/GetAll')
      .pipe(catchError(this.errorService.handleError));
  }
}
