import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { tap } from 'rxjs/operators';
import { AccountModel } from '../shared/models/accounts/account.model';
import { AccountService } from '../shared/services/account.service';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss'],
})
export class AccountsComponent implements OnInit {
  isLoading: boolean;
  accounts: AccountModel[];

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.accountService.getAll().subscribe(
      (response) => {
        this.isLoading = false;
        this.accounts = response;
      },
      (error) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    );
  }

  onAddAccountPressed(): void {
    this.router.navigate(['accounts/create']);
  }
}
