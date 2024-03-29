import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { forkJoin } from 'rxjs';
import { first } from 'rxjs/operators';
import { AccountModel } from 'src/app/shared/models/accounts/account.model';
import { UpdateAccountModel } from 'src/app/shared/models/accounts/update-account.model';
import { CurrencyModel } from 'src/app/shared/models/currencies/currency.model';
import { AccountService } from 'src/app/shared/services/account.service';
import { CurrencyService } from 'src/app/shared/services/currency.service';

@Component({
  selector: 'app-edit-account',
  templateUrl: './edit-account.component.html',
  styleUrls: [],
})
export class EditAccountComponent implements OnInit {
  account: AccountModel;
  editAccountForm: UntypedFormGroup;
  isLoading: boolean = false;
  currencies: CurrencyModel[];

  constructor(
    private accountService: AccountService,
    private fb: UntypedFormBuilder,
    private currencyService: CurrencyService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.editAccountForm = this.fb.group({
      accountName: [null, [Validators.required]],
      currency: [null, [Validators.required]],
      initialBalance: [0, [Validators.required]],
    });

    forkJoin({
      editAccount: this.accountService
        .getById(this.route.snapshot.params['accountId'])
        .pipe(first()),
      currencies: this.currencyService.getAll(),
    })
      .subscribe({
        next: ({ editAccount, currencies }) => {
          this.currencies = currencies;
          this.account = editAccount;

          this.editAccountForm.patchValue({
            accountName: editAccount.name,
            currency: editAccount.currency.id,
            initialBalance: editAccount.initialBalance,
          });
        },
        error: (error) => {
          this.toastr.error(error);
        },
      })
      .add(() => (this.isLoading = false));
  }

  onSubmit(): void {
    if (!this.editAccountForm.valid) {
      Object.keys(this.editAccountForm.controls).forEach((field) => {
        const control = this.editAccountForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });

      return;
    }

    this.updateAccount();
  }

  deleteAccount(): void {
    this.accountService
      .deleteAccount(this.account.id)
      .subscribe({
        next: (account) => {
          this.toastr.success(`Account ${account.name} deleted!`);
          this.router.navigate(['dashboard']);
        },
        error: (err) => {
          this.toastr.error(err);
        },
      })
      .add(() => (this.isLoading = false));
  }

  updateAccount() {
    this.isLoading = true;
    const updateAccountModel = new UpdateAccountModel(
      this.account.id,
      this.editAccountForm.value.accountName,
      this.editAccountForm.value.initialBalance,
      this.editAccountForm.value.currency,
    );

    this.accountService
      .updateAccount(updateAccountModel)
      .subscribe({
        next: (account) => {
          this.toastr.success(`Account ${account.name} updated!`);
          this.router.navigate(['dashboard']);
        },
        error: (err) => {
          this.toastr.error(err);
        },
      })
      .add(() => (this.isLoading = false));
  }
}
