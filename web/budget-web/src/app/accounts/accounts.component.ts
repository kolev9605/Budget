import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { tap } from 'rxjs/operators';
import { AccountModel } from '../shared/models/accounts/account.model';
import { AccountService } from '../shared/services/account.service';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: [],
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
    this.accountService.getAll().subscribe({
      next: (response) => {
        this.accounts = response;
        this.isLoading = false;
      },
      error: (error) => {
        this.toastr.error(error);
        this.isLoading = false;
      },
    });
  }

  onAddAccountPressed(): void {
    this.router.navigate(['accounts/create']);
  }
}
