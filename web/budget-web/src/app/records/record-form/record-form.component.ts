import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { AccountModel } from 'src/app/shared/models/accounts/account.model';
import { CategoryModel } from 'src/app/shared/models/categories/category.model';
import { PaymentTypeModel } from 'src/app/shared/models/payment-types/payment-type.model';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-record-form',
  templateUrl: './record-form.component.html',
  styleUrls: [],
})
export class RecordFormComponent implements OnInit {
  @Input() recordForm: UntypedFormGroup;
  @Input() inEditMode: boolean;
  @Input() categories: CategoryModel[];
  @Input() accounts: AccountModel[];
  @Input() paymentTypes: PaymentTypeModel[];
  @Input() recordTypes: string[];
  @Input() selectedRecordType: string;
  @Output() selectedRecordTypeChange = new EventEmitter();

  @Output() formSubmitted = new EventEmitter();
  @Output() recordDeleted = new EventEmitter();

  constructor() {}

  ngOnInit(): void {
    flatpickr('#recordDate', {
      enableTime: true,
      altInput: true,
      altFormat: 'F j, Y H:i',
      dateFormat: 'Y/m/d H:i',
      defaultDate: this.recordForm.value.recordDate,
      time_24hr: true,
    });
  }

  onRecordDelete(): void {
    this.recordDeleted.emit();
  }

  onFormSubmitted(): void {
    this.formSubmitted.emit();
  }

  recordTypeChanged(recordType: string) {
    this.selectedRecordType = recordType;
    this.selectedRecordTypeChange.emit(recordType);
  }
}
