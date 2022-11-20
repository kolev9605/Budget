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

    this.currencyService.getAll().subscribe(
      (currencies) => {
        this.isLoading = false;

        this.currencies = currencies;
      },
      (error) => {
        this.isLoading = false;

        this.toastr.error(error);
      },
    );
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

    this.accountService.createAccount(createAccountModel).subscribe(
      (res) => {
        this.isLoading = false;

        this.toastr.success('Account created!');
        this.router.navigate(['dashboard']);
      },
      (err) => {
        this.isLoading = false;

        this.toastr.error(err);
      },
    );
  }
}
