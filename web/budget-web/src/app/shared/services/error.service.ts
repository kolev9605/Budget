import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  public handleError(errorResponse: HttpErrorResponse) {
    console.log('subscription error: ', errorResponse);
    let errorMessage;
    if (!errorResponse.error || !errorResponse.error.message) {
      errorMessage = 'Something went wrong :/';
    } else {
      errorMessage = errorResponse.error.message;
    }

    return throwError(errorMessage);
  }
}
