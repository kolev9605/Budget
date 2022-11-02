import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedRequestModel } from '../models/pagination/paginated-request.model';
import { PaginationModel } from '../models/pagination/pagination.model';
import { CreateRecordModel } from '../models/records/create-record.model';
import { RecordModel } from '../models/records/record.model';
import { RecordsDateRangeModel } from '../models/records/records-date-range.model';
import { RecordsGroupModel } from '../models/records/records-group.model';
import { UpdateRecordModel } from '../models/records/update-record.model';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root',
})
export class RecordService {
  private controller: string = 'Records';

  constructor(private http: HttpClient, private errorService: ErrorService) {}

  getById(recordId: number): Observable<RecordModel> {
    return this.http
      .get<RecordModel>(`${environment.apiUrl}${this.controller}/GetById`, {
        params: {
          recordId: recordId,
        },
      })
      .pipe(catchError(this.errorService.handleError));
  }

  getByIdForUpdate(recordId: number): Observable<RecordModel> {
    return this.http
      .get<RecordModel>(`${environment.apiUrl}${this.controller}/GetByIdForUpdate`, {
        params: {
          recordId: recordId,
        },
      })
      .pipe(catchError(this.errorService.handleError));
  }

  getAllPaginated(
    paginatedRequestModel: PaginatedRequestModel,
  ): Observable<PaginationModel<RecordsGroupModel>> {
    return this.http
      .get<PaginationModel<RecordsGroupModel>>(
        `${environment.apiUrl}${this.controller}/GetAllPaginated`,
        {
          params: {
            pageNumber: paginatedRequestModel.pageNumber,
            pageSize: paginatedRequestModel.pageSize,
          },
        },
      )
      .pipe(catchError(this.errorService.handleError));
  }

  createRecord(createRecordModel: CreateRecordModel): Observable<number> {
    return this.http
      .post<number>(`${environment.apiUrl}${this.controller}/Create`, createRecordModel)
      .pipe(catchError(this.errorService.handleError));
  }

  updateRecord(updateRecordModel: UpdateRecordModel): Observable<number> {
    return this.http
      .post<number>(`${environment.apiUrl}${this.controller}/Update`, updateRecordModel)
      .pipe(catchError(this.errorService.handleError));
  }

  deleteRecord(recordId: number): Observable<number> {
    return this.http
      .get<number>(`${environment.apiUrl}${this.controller}/Delete`, {
        params: {
          recordId: recordId,
        },
      })
      .pipe(catchError(this.errorService.handleError));
  }

  getRecordTypes(): Observable<string[]> {
    return this.http
      .get<string[]>(`${environment.apiUrl}${this.controller}/GetRecordTypes`)
      .pipe(catchError(this.errorService.handleError));
  }

  getRecordsDateRange(): Observable<RecordsDateRangeModel> {
    return this.http
      .get<RecordsDateRangeModel>(`${environment.apiUrl}${this.controller}/GetRecordsDateRange`)
      .pipe(catchError(this.errorService.handleError));
  }
}
