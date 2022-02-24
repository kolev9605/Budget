import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { RegisterModel } from '../models/authentication/register.model';
import { LoginModel } from '../models/authentication/login.model';
import { LoginResultModel } from '../models/authentication/login-result.model';
import { ErrorService } from './error.service';
import { catchError, tap } from 'rxjs/operators';
import { Observable, Subject } from 'rxjs';
import { User } from '../models/authentication/user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  userSubject = new Subject<User>();

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
          this.userSubject.next(new User(resData.token));
        }),
      );
  }

  signOut() {}
}
