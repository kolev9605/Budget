import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AccountModel } from 'src/app/shared/models/accounts/account.model';
import { CategoryModel } from 'src/app/shared/models/categories/category.model';
import { PaymentTypeModel } from 'src/app/shared/models/payment-types/payment-type.model';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-record-form',
  templateUrl: './record-form.component.html',
  styleUrls: ['./record-form.component.scss'],
})
export class RecordFormComponent implements OnInit {
  @Input() recordForm: FormGroup;
  @Input() inEditMode: boolean;
  @Input() categories: CategoryModel[];
  @Input() accounts: AccountModel[];
  @Input() paymentTypes: PaymentTypeModel[];
  @Input() recordTypes: string[];

  @Output() formSubmitted = new EventEmitter();
  @Output() recordDeleted = new EventEmitter();

  constructor() {}

  ngOnInit(): void {
    flatpickr('#date', {
      enableTime: true,
      dateFormat: 'Y/m/d H:i',
    });
  }

  onRecordDelete(): void {
    this.recordDeleted.emit();
  }

  onFormSubmitted(): void {
    this.formSubmitted.emit();
  }

  recordTypeChanged(recordType: string) {
    this.recordForm.value.recordType = recordType;
  }
}
