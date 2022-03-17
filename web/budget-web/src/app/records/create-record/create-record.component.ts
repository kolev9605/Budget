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
import { RecordTypes } from 'src/app/shared/constants';
import { DateService } from 'src/app/shared/services/date.service';

@Component({
  selector: 'app-create-record',
  templateUrl: './create-record.component.html',
  styleUrls: ['./create-record.component.scss'],
})
export class CreateRecordComponent implements OnInit {
  isLoading: boolean;

  createRecordForm: FormGroup;
  categories: CategoryModel[];
  accounts: AccountModel[];
  paymentTypes: PaymentTypeModel[];
  recordTypes: string[];

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
      note: ['', [Validators.required]],
      amount: [null, [Validators.required]],
      account: [null, [Validators.required]],
      category: [null, [Validators.required]],
      paymentType: [null, [Validators.required]],
      recordType: [RecordTypes.Expense, [Validators.required]],
      recordDate: [null, [Validators.required]],
    });

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
      },
      (error) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    );
  }

  onSubmit(): void {
    this.isLoading = true;

    const date = this.dateService.subtractUserTimezoneOffset(
      new Date(this.createRecordForm.value.recordDate),
    );

    const createRecordModel = new CreateRecordModel(
      this.createRecordForm.value.note,
      this.createRecordForm.value.amount,
      +this.createRecordForm.value.account,
      +this.createRecordForm.value.category,
      +this.createRecordForm.value.paymentType,
      this.createRecordForm.value.recordType,
      date,
    );

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
