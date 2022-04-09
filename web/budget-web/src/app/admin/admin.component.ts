import { Component, OnInit } from '@angular/core';
import { UserModel } from '../shared/models/users/user-model';
import { AdminService } from '../shared/services/admin.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: [],
})
export class AdminComponent implements OnInit {
  users: UserModel[];

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.adminService.getUsers().subscribe((response) => {
      this.users = response;
      console.log(this.users);
    });
  }
}
