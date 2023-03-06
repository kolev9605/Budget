import { Component, Input, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CreateAccountModel } from 'src/app/shared/models/accounts/create-account.model';
import { CurrencyModel } from 'src/app/shared/models/currencies/currency.model';
import { AccountService } from 'src/app/shared/services/account.service';
import { CurrencyService } from 'src/app/shared/services/currency.service';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: [],
})
export class CreateAccountComponent implements OnInit {
  createAccountForm: UntypedFormGroup;
  isLoading: boolean = false;
  currencies: CurrencyModel[];

  constructor(
    private accountService: AccountService,
    private fb: UntypedFormBuilder,
    private currencyService: CurrencyService,
    private toastr: ToastrService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.createAccountForm = this.fb.group({
      accountName: [null, [Validators.required]],
      currency: [null, [Validators.required]],
      initialBalance: [0, [Validators.required]],
    });

    this.currencyService.getAll().subscribe({
      next: (currencies) => {
        this.currencies = currencies;
      },
      error: (error) => {
        this.toastr.error(error);
      },
      complete: () => (this.isLoading = false),
    });
  }

  onSubmit(): void {
    if (!this.createAccountForm.valid) {
      Object.keys(this.createAccountForm.controls).forEach((field) => {
        const control = this.createAccountForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });

      return;
    }

    this.createAccount();
  }

  createAccount() {
    this.isLoading = true;

    const createAccountModel: CreateAccountModel = new CreateAccountModel(
      this.createAccountForm.value.accountName,
      this.createAccountForm.value.currency,
      this.createAccountForm.value.initialBalance,
    );

    this.accountService.createAccount(createAccountModel).subscribe({
      next: (account) => {
        this.toastr.success(`Account ${account.name} created!`);
        this.router.navigate(['dashboard']);
      },
      error: (err) => {
        this.toastr.error(err);
      },
      complete: () => (this.isLoading = false),
    });
  }
}
