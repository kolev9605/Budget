import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CategoryService } from 'src/app/shared/services/category.service';
import { PaymentTypeService } from 'src/app/shared/services/payment-type.service';

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
  styleUrls: [],
})
export class CreateCategoryComponent implements OnInit {
  isLoading: boolean;
  createCategoryForm: UntypedFormGroup;
  categoryTypes: string[];

  constructor(
    private fb: UntypedFormBuilder,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.createCategoryForm = this.fb.group({
      name: [null, [Validators.required]],
      categoryType: [null, [Validators.required]],
    });

    this.categoryService.getCategoryTypes().subscribe(
      (response) => {
        this.categoryTypes = response;
      },
      (error) => {
        this.toastr.error(error);
      },
    );
  }

  onSubmit(): void {}
}
