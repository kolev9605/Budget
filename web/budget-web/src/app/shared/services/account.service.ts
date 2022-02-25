import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ErrorService } from './error.service';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AccountModel } from '../models/accounts/account.model';
import { CreateAccountModel } from '../models/accounts/create-account.model';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private controller: string = 'Account';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getAccounts(): Observable<AccountModel[]> {
    return this.http
      .get<AccountModel[]>(`${environment.apiUrl}${this.controller}/GetAll`)
      .pipe(catchError(this.errorService.handleError));
  }

  createAccount(createAccountModel: CreateAccountModel): Observable<number> {
    return this.http
      .post<number>(`${environment.apiUrl}${this.controller}/Create`, createAccountModel)
      .pipe(catchError(this.errorService.handleError));
  }
}
