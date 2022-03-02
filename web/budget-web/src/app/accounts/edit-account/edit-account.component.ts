import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  styleUrls: ['./edit-account.component.scss'],
})
export class EditAccountComponent implements OnInit {
  account: AccountModel;
  editAccountForm: FormGroup;
  isLoading: boolean = false;
  currencies: CurrencyModel[];

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private currencyService: CurrencyService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.editAccountForm = this.fb.group({
      accountName: ['', [Validators.required]],
      currency: ['', [Validators.required]],
      initialBalance: [0],
    });

    forkJoin({
      editAccount: this.accountService
        .getById(this.route.snapshot.params['accountId'])
        .pipe(first()),
      currencies: this.currencyService.getAll(),
    }).subscribe(
      ({ editAccount, currencies }) => {
        this.isLoading = false;

        this.currencies = currencies;

        this.account = editAccount;

        console.log(this.account);

        this.editAccountForm.patchValue({
          accountName: editAccount.name,
          currency: editAccount.currency.id,
          initialBalance: editAccount.initialBalance,
        });
      },
      (error) => {
        this.isLoading = false;
      },
    );
  }

  onSubmit(): void {
    this.updateAccount();
  }

  deleteAccount(): void {
    this.accountService.deleteAccount(this.account.id).subscribe(
      (res) => {
        this.isLoading = false;

        this.toastr.success('Account deleted!');
        this.router.navigate(['dashboard']);
      },
      (err) => {
        this.isLoading = false;

        this.toastr.error(err);
      },
    );
  }

  updateAccount() {
    this.isLoading = true;
    const updateAccountModel = new UpdateAccountModel(
      this.account.id,
      this.editAccountForm.value.accountName,
      this.editAccountForm.value.currency,
      this.editAccountForm.value.initialBalance,
    );

    this.accountService.updateAccount(updateAccountModel).subscribe(
      (res) => {
        this.isLoading = false;

        this.toastr.success('Account updated!');
        this.router.navigate(['dashboard']);
      },
      (err) => {
        this.isLoading = false;

        this.toastr.error(err);
      },
    );
  }
}
