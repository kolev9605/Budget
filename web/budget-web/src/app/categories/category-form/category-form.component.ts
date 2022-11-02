import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CategoryModel } from 'src/app/shared/models/categories/category.model';

@Component({
  selector: 'app-category-form',
  templateUrl: './category-form.component.html',
  styleUrls: [],
})
export class CategoryFormComponent implements OnInit {
  @Input() categoryForm: FormGroup;
  @Input() inEditMode: boolean;
  @Input() categoryTypes: string[];
  @Input() editCategory: CategoryModel;
  @Output() formSubmitted = new EventEmitter();
  @Output() categoryDeleted = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}

  onFormSubmitted(): void {
    this.formSubmitted.emit();
  }

  onCategoryDelete(): void {
    this.categoryDeleted.emit();
  }
}
