import { Component, Input, OnInit, EventEmitter, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AccountModel } from 'src/app/shared/models/accounts/account.model';
import { CurrencyModel } from 'src/app/shared/models/currencies/currency.model';

@Component({
  selector: 'app-account-form',
  templateUrl: './account-form.component.html',
  styleUrls: ['./account-form.component.scss'],
})
export class AccountFormComponent implements OnInit {
  @Input()
  currencies: CurrencyModel[];

  @Input()
  accountForm: FormGroup;

  @Input()
  inEditMode: boolean;

  @Output()
  formSubmitted = new EventEmitter();

  @Output()
  accountDeleted = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}

  onFormSubmitted(): void {
    this.formSubmitted.emit();
  }

  onAccountDelete(): void {
    this.accountDeleted.emit();
  }
}
