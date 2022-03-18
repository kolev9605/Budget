import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RecordModel } from '../shared/models/records/record.model';
import { RecordsGroupModel } from '../shared/models/records/records-group.model';
import { RecordService } from '../shared/services/record.service';

@Component({
  selector: 'app-records',
  templateUrl: './records.component.html',
  styleUrls: ['./records.component.scss'],
})
export class RecordsComponent implements OnInit {
  isLoading: boolean = false;
  recordGroups: RecordsGroupModel[];

  constructor(
    private recordService: RecordService,
    private router: Router,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.isLoading = true;

    this.recordService.getAll().subscribe(
      (response) => {
        this.isLoading = false;

        this.recordGroups = response;
      },
      (error) => {
        this.isLoading = false;

        this.toastr.error(error);
      },
    );
  }

  onAddRecordPressed() {
    this.router.navigate(['records/create']);
  }
}
