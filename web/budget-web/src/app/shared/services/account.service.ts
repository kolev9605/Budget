import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ErrorService } from './error.service';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AccountModel } from '../models/accounts/account.model';
import { CreateAccountModel } from '../models/accounts/create-account.model';
import { UpdateAccountModel } from '../models/accounts/update-account.model';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private controller: string = 'Account';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getById(accountId: number): Observable<AccountModel> {
    return this.http
      .get<AccountModel>(`${environment.apiUrl}${this.controller}/GetById`, {
        params: {
          accountId: accountId,
        },
      })
      .pipe(catchError(this.errorService.handleError));
  }

  getAll(): Observable<AccountModel[]> {
    return this.http
      .get<AccountModel[]>(`${environment.apiUrl}${this.controller}/GetAll`)
      .pipe(catchError(this.errorService.handleError));
  }

  createAccount(createAccountModel: CreateAccountModel): Observable<number> {
    return this.http
      .post<number>(`${environment.apiUrl}${this.controller}/Create`, createAccountModel)
      .pipe(catchError(this.errorService.handleError));
  }

  updateAccount(updateAccountModel: UpdateAccountModel): Observable<number> {
    return this.http
      .post<number>(`${environment.apiUrl}${this.controller}/Update`, updateAccountModel)
      .pipe(catchError(this.errorService.handleError));
  }

  deleteAccount(accountId: number) {
    return this.http
      .get<number>(`${environment.apiUrl}${this.controller}/Delete`, {
        params: {
          accountId: accountId,
        },
      })
      .pipe(catchError(this.errorService.handleError));
  }
}
