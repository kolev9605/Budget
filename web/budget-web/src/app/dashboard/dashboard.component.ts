import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountModel } from '../shared/models/accounts/account.model';
import { AccountService } from '../shared/services/account.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  public accounts: AccountModel[];
  public isLoading: boolean = false;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.accountService.getAll().subscribe(
      (res) => {
        this.isLoading = false;

        this.accounts = res;
      },
      (err) => {
        this.isLoading = false;

        this.toastr.error(err);
      },
    );
  }

  onAddAccountPressed(): void {
    this.router.navigate(['accounts/create']);
  }
}
