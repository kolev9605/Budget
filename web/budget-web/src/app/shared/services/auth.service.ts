import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { RegisterModel } from '../models/authentication/register.model';
import { LoginModel } from '../models/authentication/login.model';
import { LoginResultModel } from '../models/authentication/login-result.model';
import { ErrorService } from './error.service';
import { catchError, tap } from 'rxjs/operators';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { User } from '../models/authentication/user.model';
import { LocalStorageKeys } from 'src/app/shared/constants';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  userSubject = new BehaviorSubject<User>(null!);

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  register(signUpModel: RegisterModel): Observable<Object> {
    return this.http
      .post(environment.apiUrl + 'Authentication/Register', signUpModel)
      .pipe(catchError(this.errorService.handleError));
  }

  login(signUpModel: LoginModel): Observable<LoginResultModel> {
    return this.http
      .post<LoginResultModel>(environment.apiUrl + 'Authentication/Login', signUpModel)
      .pipe(
        catchError(this.errorService.handleError),
        tap((resData) => {
          this.userSubject.next(new User(resData.token, resData.validTo));
          localStorage.setItem(LocalStorageKeys.UserData, JSON.stringify(resData));
        }),
      );
  }

  tryLogin(): void {
    const userData = JSON.parse(localStorage.getItem(LocalStorageKeys.UserData)!);
    const user = new User(userData.token, new Date(userData.validTo));

    // TODO: implement refreshing the token when it expires

    this.userSubject.next(user);
  }

  logout() {
    this.userSubject.next(null!);
    localStorage.removeItem(LocalStorageKeys.UserData);
  }
}