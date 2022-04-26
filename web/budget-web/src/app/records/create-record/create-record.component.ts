import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { forkJoin } from 'rxjs';
import { AccountModel } from 'src/app/shared/models/accounts/account.model';
import { CategoryModel } from 'src/app/shared/models/categories/category.model';
import { PaymentTypeModel } from 'src/app/shared/models/payment-types/payment-type.model';
import { CreateRecordModel } from 'src/app/shared/models/records/create-record.model';
import { AccountService } from 'src/app/shared/services/account.service';
import { CategoryService } from 'src/app/shared/services/category.service';
import { PaymentTypeService } from 'src/app/shared/services/payment-type.service';
import { RecordService } from 'src/app/shared/services/record.service';
import { RecordTypes } from 'src/app/shared/constants/constants';
import { DateService } from 'src/app/shared/services/date.service';
import { RecordsValidations } from 'src/app/shared/constants/validations';

@Component({
  selector: 'app-create-record',
  templateUrl: './create-record.component.html',
  styleUrls: [],
})
export class CreateRecordComponent implements OnInit {
  isLoading: boolean;

  createRecordForm: FormGroup;
  categories: CategoryModel[];
  accounts: AccountModel[];
  paymentTypes: PaymentTypeModel[];
  recordTypes: string[];
  selectedRecordType: string;

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    private accountService: AccountService,
    private paymentTypeService: PaymentTypeService,
    private recordService: RecordService,
    private router: Router,
    private dateService: DateService,
  ) {}

  ngOnInit(): void {
    this.createRecordForm = this.fb.group({
      note: [null, [Validators.maxLength(RecordsValidations.NoteMaxtLength)]],
      amount: [null, [Validators.required]],
      fromAccount: [null, []],
      account: [null, [Validators.required]],
      category: [null, [Validators.required]],
      paymentType: [null, [Validators.required]],
      recordDate: [new Date(), [Validators.required]],
    });

    this.isLoading = true;

    forkJoin({
      categories: this.categoryService.getAll(),
      accounts: this.accountService.getAll(),
      paymentTypes: this.paymentTypeService.getAll(),
      recordTypes: this.recordService.getRecordTypes(),
    }).subscribe(
      ({ categories, accounts, paymentTypes, recordTypes }) => {
        this.isLoading = false;

        this.categories = categories;
        this.accounts = accounts;
        this.paymentTypes = paymentTypes;
        this.recordTypes = recordTypes;

        if (!accounts || accounts.length === 0) {
          this.toastr.warning('You have to create an account first');
          this.router.navigate(['accounts/create']);
          return;
        }

        var expenseRecordType = recordTypes.find((rt) => rt === RecordTypes.Expense);
        if (expenseRecordType) {
          this.selectedRecordType = expenseRecordType;
        }

        this.createRecordForm.patchValue({
          account: this.accounts[0].id,
          paymentType: this.paymentTypes.find((x) => (x.name = 'Debit Card'))?.id,
        });
      },
      (error) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    );
  }

  onSubmit(): void {
    if (!this.createRecordForm.valid) {
      Object.keys(this.createRecordForm.controls).forEach((field) => {
        const control = this.createRecordForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });

      return;
    }

    const createRecordModel = new CreateRecordModel(
      this.createRecordForm.value.note,
      this.createRecordForm.value.amount,
      +this.createRecordForm.value.account,
      +this.createRecordForm.value.category,
      +this.createRecordForm.value.paymentType,
      this.selectedRecordType,
      this.createRecordForm.value.recordDate,
      this.createRecordForm.value.fromAccount,
    );

    this.isLoading = true;
    this.recordService.createRecord(createRecordModel).subscribe(
      (response) => {
        this.isLoading = false;

        this.toastr.success('Record created!');
        this.router.navigate(['records']);
      },
      (error) => {
        this.isLoading = false;

        this.toastr.error(error);
      },
    );
  }
}
