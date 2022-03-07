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

  getAll(): Observable<CategoryModel[]> {
    return this.http
      .get<CategoryModel[]>(`${environment.apiUrl}${this.controller}/GetAll`)
      .pipe(catchError(this.errorService.handleError));
  }
}
