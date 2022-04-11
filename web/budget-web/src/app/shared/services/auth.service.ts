import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { RegisterModel } from '../models/authentication/register.model';
import { LoginModel } from '../models/authentication/login.model';
import { TokenModel } from '../models/authentication/token.model';
import { ErrorService } from './error.service';
import { catchError, tap } from 'rxjs/operators';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { AuthenticatedUserModel } from '../models/authentication/authenticated-user.model';
import { LocalStorageKeys } from 'src/app/shared/constants/constants';
import { RegistrationResultModel } from '../models/authentication/registration-result.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private controller: string = 'Authentication';

  public userSubject = new BehaviorSubject<AuthenticatedUserModel>(null!);

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  register(signUpModel: RegisterModel): Observable<RegistrationResultModel> {
    return this.http
      .post<RegistrationResultModel>(
        `${environment.apiUrl}${this.controller}/Register`,
        signUpModel,
      )
      .pipe(catchError(this.errorService.handleError));
  }

  login(signUpModel: LoginModel): Observable<TokenModel> {
    return this.http
      .post<TokenModel>(`${environment.apiUrl}${this.controller}/Login`, signUpModel)
      .pipe(
        catchError(this.errorService.handleError),
        tap((resData) => {
          this.userSubject.next(
            new AuthenticatedUserModel(resData.token, resData.validTo, resData.roles),
          );
          localStorage.setItem(LocalStorageKeys.UserData, JSON.stringify(resData));
        }),
      );
  }

  tryLogin(): void {
    const userData = JSON.parse(localStorage.getItem(LocalStorageKeys.UserData)!);
    if (userData) {
      const user = new AuthenticatedUserModel(
        userData.token,
        new Date(userData.validTo),
        userData.roles,
      );
      // TODO: implement refreshing the token when it expires
      if (!this.isTokenValid(user)) {
        this.logout();
        return;
      }

      this.userSubject.next(user);
    }
  }

  logout() {
    this.userSubject.next(null!);
    localStorage.removeItem(LocalStorageKeys.UserData);
  }

  isTokenValid(user: AuthenticatedUserModel) {
    const nowUtc = new Date(new Date().toUTCString());
    const validToDate = new Date(user.validTo);

    return validToDate >= nowUtc;
  }
}
