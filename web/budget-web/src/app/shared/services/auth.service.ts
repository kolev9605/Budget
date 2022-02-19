import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { RegisterModel } from '../models/authentication/register.model';
import { LoginModel } from '../models/authentication/login.model';
import { TokenModel } from '../models/authentication/token.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  register(signUpModel: RegisterModel) {
    return this.http.post(environment.apiUrl + 'Authentication/Register', signUpModel);
  }

  login(signUpModel: LoginModel) {
    return this.http.post<TokenModel>(environment.apiUrl + 'Authentication/Login', signUpModel);
  }

  signOut() {}
}
