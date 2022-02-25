import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateAccountModel } from 'src/app/shared/models/accounts/create-account.model';
import { AccountService } from 'src/app/shared/services/account.service';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.scss'],
})
export class CreateAccountComponent implements OnInit {
  public createAccountForm: FormGroup;
  public isLoading: boolean = false;

  constructor(private accountService: AccountService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.createAccountForm = this.fb.group({
      accountName: ['', [Validators.required]],
    });
  }

  onSubmit(): void {
    console.log(this.createAccountForm);
    const createAccountModel: CreateAccountModel = new CreateAccountModel(
      this.createAccountForm.value.accountName,
    );

    this.accountService.createAccount(createAccountModel).subscribe((res) => {
      console.log(res);
    });
  }
}
