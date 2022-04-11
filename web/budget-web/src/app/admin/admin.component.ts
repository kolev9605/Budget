import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserModel } from '../shared/models/users/user-model';
import { AdminService } from '../shared/services/admin.service';
import { Roles } from '../shared/constants/constants';
import { concatMap, Observable, Subject, tap } from 'rxjs';
import { ChangeUserRoleRequestModel } from '../shared/models/admin/change-user-role-request.model';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: [],
})
export class AdminComponent implements OnInit {
  users: UserModel[];
  isLoading: boolean = false;
  usersSubject: Subject<any> = new Subject();
  usersObservable: Observable<UserModel[]> = new Observable();

  constructor(private adminService: AdminService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.usersObservable = this.usersSubject.pipe(
      tap(() => (this.isLoading = true)),
      concatMap(() => this.adminService.getUsers()),
    );

    this.usersObservable.subscribe(
      (response) => {
        this.users = response;
        this.isLoading = false;
      },
      (error) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    );

    this.usersSubject.next(true);
  }

  isAdmin(roles: string[]): boolean {
    return !!roles.find((r) => r === Roles.Administrator);
  }

  onDeleteButtonPressed(userId: string): void {
    this.adminService.deleteUser(userId).subscribe(
      (response) => {
        this.usersSubject.next(true);
      },
      (error) => {
        this.toastr.error(error);
      },
    );
  }

  makeAdmin(userId: string): void {
    const requestModel = new ChangeUserRoleRequestModel(userId, Roles.Administrator);
    this.adminService.changeUserRole(requestModel).subscribe(
      (response) => {
        this.usersSubject.next(true);
      },
      (error) => {
        this.toastr.error(error);
      },
    );
  }
  makeUser(userId: string): void {
    const requestModel = new ChangeUserRoleRequestModel(userId, Roles.User);
    this.adminService.changeUserRole(requestModel).subscribe(
      (response) => {
        this.usersSubject.next(true);
      },
      (error) => {
        this.toastr.error(error);
      },
    );
  }
}