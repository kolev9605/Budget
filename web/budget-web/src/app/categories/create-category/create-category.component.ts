import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { forkJoin } from 'rxjs';
import { CategoryModel } from 'src/app/shared/models/categories/category.model';
import { CreateCategoryModel } from 'src/app/shared/models/categories/create-category.model';
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
  primaryCategories: CategoryModel[];

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
      isPrimary: [false],
      parentCategoryId: [null],
    });

    forkJoin({
      categoryTypes: this.categoryService.getCategoryTypes(),
      primaryCategories: this.categoryService.getAllPrimary(),
    }).subscribe(
      ({ categoryTypes: categoryTypes, primaryCategories: primaryCategories }) => {
        this.isLoading = false;

        this.categoryTypes = categoryTypes;
        this.primaryCategories = primaryCategories;
      },
      (error) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    );
  }

  onSubmit(): void {
    const createCategoryModel = new CreateCategoryModel(
      this.createCategoryForm.value.name,
      this.createCategoryForm.value.categoryType,
      this.createCategoryForm.value.parentCategoryId,
    );

    this.isLoading = true;
    this.categoryService.createCategory(createCategoryModel).subscribe(
      (category) => {
        this.isLoading = false;

        this.toastr.success(`Category ${category.name} created!`);
        this.router.navigate(['categories']);
      },
      (error) => {
        this.isLoading = false;

        this.toastr.error(error);
      },
    );
  }
}
