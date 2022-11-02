import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CategoryModel } from '../models/categories/category.model';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private controller: string = 'Categories';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getById(categoryId: number): Observable<CategoryModel> {
    return this.http
      .get<CategoryModel>(`${environment.apiUrl}${this.controller}/GetById`, {
        params: {
          categoryId: categoryId,
        },
      })
      .pipe(catchError(this.errorService.handleError));
  }

  getAll(): Observable<CategoryModel[]> {
    return this.http
      .get<CategoryModel[]>(`${environment.apiUrl}${this.controller}/GetAll`)
      .pipe(catchError(this.errorService.handleError));
  }

  getAllPrimary(): Observable<CategoryModel[]> {
    return this.http
      .get<CategoryModel[]>(`${environment.apiUrl}${this.controller}/GetAllPrimary`)
      .pipe(catchError(this.errorService.handleError));
  }

  getAllSubcategories(parentCategoryId: number): Observable<CategoryModel[]> {
    return this.http
      .get<CategoryModel[]>(`${environment.apiUrl}${this.controller}/GetAllSubcategories`, {
        params: {
          parentCategoryId: parentCategoryId,
        },
      })
      .pipe(catchError(this.errorService.handleError));
  }

  getCategoryTypes(): Observable<string[]> {
    return this.http
      .get<string[]>(`${environment.apiUrl}${this.controller}/getCategoryTypes`)
      .pipe(catchError(this.errorService.handleError));
  }
}
