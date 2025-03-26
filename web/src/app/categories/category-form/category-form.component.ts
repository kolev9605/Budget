import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { CategoryModel } from 'src/app/shared/models/categories/category.model';

@Component({
  selector: 'app-category-form',
  templateUrl: './category-form.component.html',
  styleUrls: [],
})
export class CategoryFormComponent implements OnInit, OnDestroy {
  @Input() categoryForm: UntypedFormGroup;
  @Input() inEditMode: boolean;
  @Input() categoryTypes: string[];
  @Input() primaryCategories: CategoryModel[];
  @Input() editCategory: CategoryModel;
  @Output() formSubmitted = new EventEmitter();
  @Output() categoryDeleted = new EventEmitter();

  isPrimary: boolean = false;

  checkboxChangeSubscription: Subscription | undefined;

  constructor() {}

  ngOnDestroy(): void {
    if (this.checkboxChangeSubscription) {
      this.checkboxChangeSubscription.unsubscribe();
    }
  }

  ngOnInit(): void {
    if (this.inEditMode && this.editCategory) {
      this.isPrimary = !this.editCategory.parentCategoryId;
    }

    // TODO: Can this change detection be achieved without subscribing to valueChanges?
    this.checkboxChangeSubscription = this.categoryForm
      .get('isPrimary')
      ?.valueChanges.subscribe((value) => {
        this.isPrimary = value;

        if (this.isPrimary) {
          this.categoryForm.controls['parentCategoryId'].setValue(null);
        }
      });
  }

  onFormSubmitted(): void {
    this.formSubmitted.emit();
  }

  onCategoryDelete(): void {
    this.categoryDeleted.emit();
  }
}
