import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { concatMap, Observable, Subject, tap } from 'rxjs';
import { PaginatedRequestModel } from '../shared/models/pagination/paginated-request.model';
import { PaginationModel } from '../shared/models/pagination/pagination.model';
import { RecordModel } from '../shared/models/records/record.model';
import { RecordsGroupModel } from '../shared/models/records/records-group.model';
import { ExportService } from '../shared/services/export.service';
import { RecordService } from '../shared/services/record.service';

@Component({
  selector: 'app-records',
  templateUrl: './records.component.html',
  styleUrls: [],
})
export class RecordsComponent implements OnInit {
  isLoading: boolean = false;
  recordGroups: RecordsGroupModel[] = [];
  recordsRequestSubject: Subject<PaginatedRequestModel> = new Subject();
  recordsObservable: Observable<PaginationModel<RecordsGroupModel>> = new Observable();
  pageNumber: number = 1;
  hasNextPage: boolean;

  constructor(
    private recordService: RecordService,
    private router: Router,
    private toastr: ToastrService,
    private exportService: ExportService,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;

    this.recordsObservable = this.recordsRequestSubject.pipe(
      tap(() => (this.isLoading = true)),
      concatMap((request: PaginatedRequestModel) => this.recordService.getAllPaginated(request)),
    );

    this.recordsObservable.subscribe({
      next: (response: PaginationModel<RecordsGroupModel>) => {
        this.isLoading = false;
        this.recordGroups.push(...response.items);
        this.pageNumber = response.pageNumber;
        this.hasNextPage = response.hasNextPage;
      },
      error: (error: string) => {
        this.isLoading = false;
        this.toastr.error(error);
      },
    });

    var paginatedRequestModel: PaginatedRequestModel = new PaginatedRequestModel(this.pageNumber);
    this.recordsRequestSubject.next(paginatedRequestModel);
  }

  onAddRecordPressed() {
    this.router.navigate(['records/create']);
  }

  onExportRecordsPressed() {
    this.exportService.exportRecords().subscribe(
      (response) => this.downloadFile(JSON.stringify(response)),
      (error) => this.toastr.error(error),
    );
  }

  downloadFile(data: any) {
    const blob = new Blob([data], { type: 'application/json' });
    const url = window.URL.createObjectURL(blob);
    window.open(url);
  }

  scrolled($event: any) {
    if (this.hasNextPage) {
      this.pageNumber++;
      this.recordsRequestSubject.next(new PaginatedRequestModel(this.pageNumber));
    }
  }

  onScrollUp($event: any) {}
}
