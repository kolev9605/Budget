import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CreateAccountModel } from 'src/app/shared/models/accounts/create-account.model';
import { CurrencyModel } from 'src/app/shared/models/currencies/currency.model';
import { AccountService } from 'src/app/shared/services/account.service';
import { CurrencyService } from 'src/app/shared/services/currency.service';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.scss'],
})
export class CreateAccountComponent implements OnInit {
  public createAccountForm: FormGroup;
  public isLoading: boolean = false;
  public currencies: CurrencyModel[];

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private currencyService: CurrencyService,
    private toastr: ToastrService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.createAccountForm = this.fb.group({
      accountName: ['', [Validators.required]],
      currency: ['', [Validators.required]],
    });

    this.currencyService.getAll().subscribe(
      (res) => {
        this.isLoading = false;

        this.currencies = res;
      },
      (err) => {
        this.isLoading = false;

        this.toastr.error(err);
      },
    );
  }

  onSubmit(): void {
    this.isLoading = true;

    console.log(this.createAccountForm.value.currency);
    const createAccountModel: CreateAccountModel = new CreateAccountModel(
      this.createAccountForm.value.accountName,
      this.createAccountForm.value.currency.id,
    );

    console.log('createAccountModel', createAccountModel);

    this.accountService.createAccount(createAccountModel).subscribe(
      (res) => {
        this.isLoading = false;

        this.toastr.success('Account created!');
        this.router.navigate(['dashboard']);

        console.log(res);
      },
      (err) => {
        this.isLoading = false;

        this.toastr.error(err);
      },
    );
  }
}
