import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { concatMap, Observable, Subject, tap } from 'rxjs';
import { PaginatedRequestModel } from '../shared/models/pagination/paginated-request.model';
import { PaginationModel } from '../shared/models/pagination/pagination.model';
import { RecordsGroupModel } from '../shared/models/records/records-group.model';
import { RecordService } from '../shared/services/record.service';
import { RecordModel } from '../shared/models/records/record.model';

@Component({
  selector: 'app-records',
  templateUrl: './records.component.html',
  styleUrls: [],
})
export class RecordsComponent implements OnInit {
  isLoading: boolean = false;
  recordGroups: RecordsGroupModel[] = [];
  recordsRequestSubject: Subject<PaginatedRequestModel> = new Subject();
  recordsObservable: Observable<PaginationModel<RecordModel>> = new Observable();
  pageNumber: number = 1;
  hasNextPage: boolean;

  constructor(
    private recordService: RecordService,
    private router: Router,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.recordsObservable = this.recordsRequestSubject.pipe(
      tap(() => (this.isLoading = true)),
      concatMap((request: PaginatedRequestModel) => this.recordService.getAllPaginated(request)),
      tap(() => (this.isLoading = false)),
    );

    this.recordsObservable.subscribe({
      next: (response: PaginationModel<RecordModel>) => {
        const groupedData = response.items.reduce((groups, item) => {
          const date = new Date(item.recordDate).toDateString();

          if (!groups.has(date)) {
            groups.set(date, []);
          }

          groups.get(date)?.push(item);

          return groups;
        }, new Map<string, RecordModel[]>());

        // Convert the Map to an array of grouped objects
        const result = Array.from(groupedData, ([date, items]) => (new RecordsGroupModel(new Date(date), items.reduce((sum, current) => sum + current.amount, 0), items)));

        result.forEach(group => {
          let existingGroup = this.recordGroups.find((g) => g.date.getTime() === group.date.getTime());
          if (existingGroup) {
            existingGroup.records = [...existingGroup.records, ...group.records];
          } else {
            this.recordGroups = [...this.recordGroups, group];
          }
        });

        this.pageNumber = response.pageNumber;
        this.hasNextPage = response.hasNextPage;
      },
      error: (error: string) => {
        this.toastr.error(error);
      },
    });

    var paginatedRequestModel: PaginatedRequestModel = new PaginatedRequestModel(this.pageNumber);
    this.recordsRequestSubject.next(paginatedRequestModel);
  }

  onAddRecordPressed() {
    this.router.navigate(['records/create']);
  }

  scrolled($event: any) {
    if (this.hasNextPage) {
      this.pageNumber++;
      this.recordsRequestSubject.next(new PaginatedRequestModel(this.pageNumber));
    }
  }

  onScrollUp($event: any) {}
}
