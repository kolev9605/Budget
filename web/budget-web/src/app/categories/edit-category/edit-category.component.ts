import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { first, forkJoin } from 'rxjs';
import { CategoryModel } from 'src/app/shared/models/categories/category.model';
import { UpdateCategoryModel } from 'src/app/shared/models/categories/update-category.model';
import { CategoryService } from 'src/app/shared/services/category.service';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: [],
})
export class EditCategoryComponent implements OnInit {
  isLoading: boolean;
  editCategoryForm: UntypedFormGroup;
  categoryTypes: string[];
  primaryCategories: CategoryModel[];
  category: CategoryModel;

  constructor(
    private fb: UntypedFormBuilder,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.editCategoryForm = this.fb.group({
      name: [null, [Validators.required]],
      categoryType: [null, [Validators.required]],
      isPrimary: [false],
      parentCategoryId: [null],
    });

    forkJoin({
      editCategory: this.categoryService
        .getById(this.route.snapshot.params['categoryId'])
        .pipe(first()),
      categoryTypes: this.categoryService.getCategoryTypes(),
      primaryCategories: this.categoryService.getAllPrimary(),
    }).subscribe({
      next: ({
        editCategory: editCategory,
        categoryTypes: categoryTypes,
        primaryCategories: primaryCategories,
      }) => {
        this.category = editCategory;
        this.categoryTypes = categoryTypes;
        this.primaryCategories = primaryCategories;

        this.editCategoryForm.patchValue({
          name: editCategory.name,
          categoryType: editCategory.categoryType,
          parentCategoryId: editCategory.parentCategoryId,
          isPrimary: editCategory.parentCategoryId === null,
        });
      },
      error: (error) => {
        this.toastr.error(error);
      },
      complete: () => (this.isLoading = false),
    });
  }

  onSubmit(): void {
    if (!this.editCategoryForm.valid) {
      Object.keys(this.editCategoryForm.controls).forEach((field) => {
        const control = this.editCategoryForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });

      return;
    }

    const updateCategoryModel = new UpdateCategoryModel(
      this.category.id,
      this.editCategoryForm.value.name,
      this.editCategoryForm.value.categoryType,
      this.editCategoryForm.value.parentCategoryId,
    );

    this.isLoading = true;
    this.categoryService.updateCategory(updateCategoryModel).subscribe({
      next: (category) => {
        this.toastr.success(`Category ${category.name} updated!`);
        this.router.navigate(['categories']);
      },
      error: (error) => {
        this.toastr.error(error);
      },
      complete: () => (this.isLoading = false),
    });
  }

  deleteCategory(): void {
    this.categoryService.deleteCategory(this.category.id).subscribe({
      next: (category) => {
        this.toastr.success(`Category ${category.name} deleted!`);
        this.router.navigate(['categories']);
      },
      error: (err) => {
        this.toastr.error(err);
      },
      complete: () => (this.isLoading = false),
    });
  }
}
