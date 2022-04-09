import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserModel } from '../models/users/user-model';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private controller: string = 'Admin';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getUsers(): Observable<UserModel[]> {
    return this.http
      .get<UserModel[]>(`${environment.apiUrl}${this.controller}/getUsers`)
      .pipe(catchError(this.errorService.handleError));
  }
}
