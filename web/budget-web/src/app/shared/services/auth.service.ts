import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { RegisterModel } from '../models/authentication/register.model';
import { LoginModel } from '../models/authentication/login.model';
import { TokenModel } from '../models/authentication/token.model';
import { ErrorService } from './error.service';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient, private errorService: ErrorService) {}

  register(signUpModel: RegisterModel): Observable<Object> {
    return this.http
      .post(environment.apiUrl + 'Authentication/Register', signUpModel)
      .pipe(catchError(this.errorService.handleError));
  }

  login(signUpModel: LoginModel): Observable<TokenModel> {
    return this.http
      .post<TokenModel>(environment.apiUrl + 'Authentication/Login', signUpModel)
      .pipe(catchError(this.errorService.handleError));
  }

  signOut() {}
}
