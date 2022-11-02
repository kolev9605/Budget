import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CategoryModel } from '../shared/models/categories/category.model';
import { CategoryService } from '../shared/services/category.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: [],
})
export class CategoriesComponent implements OnInit {
  isLoading: boolean = false;
  categories: CategoryModel[];
  constructor(
    private categoryService: CategoryService,
    private toastr: ToastrService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.categoryService.getAllPrimary().subscribe(
      (response) => {
        this.isLoading = false;
        this.categories = response;
      },
      (error) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    );
  }

  onAddCategoryPressed(): void {
    this.router.navigate(['categories/create']);
  }

  getCollapseTarget(categoryId: number): string {
    return `collapse-${categoryId}`;
  }

  getCollapseId(categoryId: number): string {
    return `#collapse-${categoryId}`;
  }
}
