import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaymentTypeModel } from '../models/payment-types/payment-type.model';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root',
})
export class PaymentTypeService {
  private controller: string = 'PaymentTypes';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getAll(): Observable<PaymentTypeModel[]> {
    return this.http
      .get<PaymentTypeModel[]>(`${environment.apiUrl}${this.controller}/GetAll`)
      .pipe(catchError(this.errorService.handleError));
  }
}
