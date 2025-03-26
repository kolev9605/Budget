import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  public handleError(errorResponse: HttpErrorResponse) {
    let errorMessage;
    console.log(errorResponse);

    if (errorResponse.error && errorResponse.error.title) {
      errorMessage = errorResponse.error.title;
    } else if (errorResponse && errorResponse.status === 401) {
      errorMessage = 'Your session has expired, please log in again.';
    } else {
      errorMessage = 'Something went wrong.';
    }

    return throwError(errorMessage);
  }
}
