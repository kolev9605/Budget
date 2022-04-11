import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ChangeUserRoleRequestModel } from '../models/admin/change-user-role-request.model';
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
      .get<UserModel[]>(`${environment.apiUrl}${this.controller}/GetUsers`)
      .pipe(catchError(this.errorService.handleError));
  }

  deleteUser(userId: string): Observable<boolean> {
    return this.http
      .get<boolean>(`${environment.apiUrl}${this.controller}/DeleteUser`, {
        params: {
          userId: userId,
        },
      })
      .pipe(catchError(this.errorService.handleError));
  }

  changeUserRole(requestModel: ChangeUserRoleRequestModel): Observable<boolean> {
    return this.http
      .post<boolean>(`${environment.apiUrl}${this.controller}/ChangeUserRole`, requestModel)
      .pipe(catchError(this.errorService.handleError));
  }
}
