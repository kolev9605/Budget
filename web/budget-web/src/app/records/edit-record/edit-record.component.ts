import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { forkJoin } from 'rxjs';
import { first } from 'rxjs/operators';
import { AccountModel } from 'src/app/shared/models/accounts/account.model';
import { CategoryModel } from 'src/app/shared/models/categories/category.model';
import { PaymentTypeModel } from 'src/app/shared/models/payment-types/payment-type.model';
import { RecordModel } from 'src/app/shared/models/records/record.model';
import { UpdateRecordModel } from 'src/app/shared/models/records/update-record.model';
import { AccountService } from 'src/app/shared/services/account.service';
import { CategoryService } from 'src/app/shared/services/category.service';
import { PaymentTypeService } from 'src/app/shared/services/payment-type.service';
import { RecordService } from 'src/app/shared/services/record.service';
import { format } from 'date-fns';
import { DateService } from 'src/app/shared/services/date.service';
import { Formats } from '../../shared/constants';

@Component({
  selector: 'app-edit-record',
  templateUrl: './edit-record.component.html',
  styleUrls: ['./edit-record.component.scss'],
})
export class EditRecordComponent implements OnInit {
  isLoading: boolean;

  record: RecordModel;
  editRecordForm: FormGroup;
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
    private route: ActivatedRoute,
    private dateService: DateService,
  ) {}

  ngOnInit(): void {
    this.editRecordForm = this.fb.group({
      note: ['', [Validators.required]],
      amount: [null, [Validators.required]],
      account: [null, [Validators.required]],
      category: [null, [Validators.required]],
      paymentType: [null, [Validators.required]],
      recordDate: [null, [Validators.required]],
    });

    forkJoin({
      editRecord: this.recordService.getById(this.route.snapshot.params['accountId']).pipe(first()),
      categories: this.categoryService.getAll(),
      accounts: this.accountService.getAll(),
      paymentTypes: this.paymentTypeService.getAll(),
      recordTypes: this.recordService.getRecordTypes(),
    }).subscribe(
      ({ editRecord, categories, accounts, paymentTypes, recordTypes }) => {
        this.isLoading = false;

        this.categories = categories;
        this.accounts = accounts;
        this.paymentTypes = paymentTypes;
        this.recordTypes = recordTypes;
        this.record = editRecord;

        this.selectedRecordType = this.record.recordType;

        this.editRecordForm.patchValue({
          note: this.record.note,
          amount: this.record.amount,
          account: this.record.account.id,
          category: this.record.category.id,
          paymentType: this.record.paymentType.id,
          recordDate: format(new Date(this.record.recordDate), Formats.DateFormt),
        });
      },
      (error) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    );
  }

  onSubmit(): void {
    const date = this.dateService.subtractUserTimezoneOffset(
      new Date(this.editRecordForm.value.recordDate),
    );

    const updateRecordModel = new UpdateRecordModel(
      this.record.id,
      this.editRecordForm.value.note,
      this.editRecordForm.value.amount,
      +this.editRecordForm.value.account,
      +this.editRecordForm.value.category,
      +this.editRecordForm.value.paymentType,
      this.selectedRecordType,
      date,
    );

    this.isLoading = true;
    this.recordService.updateRecord(updateRecordModel).subscribe(
      (response) => {
        this.isLoading = false;

        this.toastr.success('Record updated!');
        this.router.navigate(['records']);
      },
      (error) => {
        this.isLoading = false;

        this.toastr.error(error);
      },
    );
  }

  deleteRecord(): void {
    this.recordService.deleteRecord(this.record.id).subscribe(
      (res) => {
        this.isLoading = false;

        this.toastr.success('Record deleted!');
        this.router.navigate(['records']);
      },
      (err) => {
        this.isLoading = false;

        this.toastr.error(err);
      },
    );
  }
}
