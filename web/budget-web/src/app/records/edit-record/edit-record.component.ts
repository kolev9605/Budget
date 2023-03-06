import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
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
import { Formats } from '../../shared/constants/constants';
import { RecordsValidations } from 'src/app/shared/constants/validations';

@Component({
  selector: 'app-edit-record',
  templateUrl: './edit-record.component.html',
  styleUrls: [],
})
export class EditRecordComponent implements OnInit {
  isLoading: boolean;

  record: RecordModel;
  editRecordForm: UntypedFormGroup;
  categories: CategoryModel[];
  accounts: AccountModel[];
  paymentTypes: PaymentTypeModel[];
  recordTypes: string[];
  selectedRecordType: string;

  constructor(
    private fb: UntypedFormBuilder,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    private accountService: AccountService,
    private paymentTypeService: PaymentTypeService,
    private recordService: RecordService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.editRecordForm = this.fb.group({
      note: [null, [Validators.maxLength(RecordsValidations.NoteMaxtLength)]],
      amount: [null, [Validators.required]],
      fromAccount: [null, []],
      account: [null, [Validators.required]],
      category: [null, [Validators.required]],
      paymentType: [null, [Validators.required]],
      recordDate: [new Date(), [Validators.required]],
    });

    forkJoin({
      editRecord: this.recordService
        .getByIdForUpdate(this.route.snapshot.params['accountId'])
        .pipe(first()),
      categories: this.categoryService.getAll(),
      accounts: this.accountService.getAll(),
      paymentTypes: this.paymentTypeService.getAll(),
      recordTypes: this.recordService.getRecordTypes(),
    }).subscribe({
      next: ({ editRecord, categories, accounts, paymentTypes, recordTypes }) => {
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
          fromAccount: this.record.fromAccount?.id,
          category: this.record.category.id,
          paymentType: this.record.paymentType.id,
          recordDate: format(new Date(this.record.recordDate), Formats.DateTimeFormt),
        });
      },
      error: (error) => {
        this.toastr.error(error);
      },
      complete: () => (this.isLoading = false),
    });
  }

  onSubmit(): void {
    if (!this.editRecordForm.valid) {
      Object.keys(this.editRecordForm.controls).forEach((field) => {
        const control = this.editRecordForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });

      return;
    }

    const updateRecordModel = new UpdateRecordModel(
      this.record.id,
      this.editRecordForm.value.note,
      this.editRecordForm.value.amount,
      +this.editRecordForm.value.account,
      +this.editRecordForm.value.category,
      +this.editRecordForm.value.paymentType,
      this.selectedRecordType,
      new Date(this.editRecordForm.value.recordDate),
      this.editRecordForm.value.fromAccount,
    );

    this.isLoading = true;
    this.recordService.updateRecord(updateRecordModel).subscribe({
      next: (record) => {
        this.toastr.success(`Record updated from ${record.category.name}!`);
        this.router.navigate(['records']);
      },
      error: (error) => {
        this.toastr.error(error);
      },
      complete: () => (this.isLoading = false),
    });
  }

  deleteRecord(): void {
    this.recordService.deleteRecord(this.record.id).subscribe({
      next: (record) => {
        this.toastr.success(`Record deleted from ${record.category.name}!`);
        this.router.navigate(['records']);
      },
      error: (err) => {
        this.toastr.error(err);
      },
      complete: () => (this.isLoading = false),
    });
  }
}
