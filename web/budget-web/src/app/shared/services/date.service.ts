import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DateService {
  constructor() {}

  subtractUserTimezoneOffset(date: Date): Date {
    const userTimezoneOffset = date.getTimezoneOffset() * 60000;
    const dateWithoutOffset = new Date(date.getTime() - userTimezoneOffset);

    return dateWithoutOffset;
  }
}
