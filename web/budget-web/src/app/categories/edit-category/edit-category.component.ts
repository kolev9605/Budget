import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { first, forkJoin } from 'rxjs';
import { CategoryModel } from 'src/app/shared/models/categories/category.model';
import { CategoryService } from 'src/app/shared/services/category.service';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: [],
})
export class EditCategoryComponent implements OnInit {
  isLoading: boolean;
  editCategoryForm: FormGroup;
  categoryTypes: string[];
  category: CategoryModel;

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.editCategoryForm = this.fb.group({
      name: [null, [Validators.required]],
      categoryType: [null, [Validators.required]],
    });

    forkJoin({
      editCategory: this.categoryService
        .getById(this.route.snapshot.params['categoryId'])
        .pipe(first()),
      categoryTypes: this.categoryService.getCategoryTypes(),
    }).subscribe(
      ({ editCategory: editCategory, categoryTypes: categoryTypes }) => {
        this.isLoading = false;

        this.category = editCategory;
        this.categoryTypes = categoryTypes;

        this.editCategoryForm.patchValue({
          name: editCategory.name,
          categoryType: editCategory.categoryType,
        });
      },
      (error) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    );
  }

  onSubmit(): void {}
}
