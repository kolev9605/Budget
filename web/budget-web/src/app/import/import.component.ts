import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ExportService } from '../shared/services/export.service';
import { ImportService } from '../shared/services/import.service';

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.scss'],
})
export class ImportComponent implements OnInit {
  constructor(
    private exportService: ExportService,
    private importService: ImportService,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {}

  onExportRecordsPressed() {
    this.exportService.exportRecords().subscribe({
      next: (response) => this.downloadFile(JSON.stringify(response)),
      error: (error) => this.toastr.error(error),
    });
  }

  onImportRecordsPressed($event: any) {
    let fileList: FileList = $event.target.files;
    if (fileList.length > 0) {
      let file: File = fileList[0];
      this.importService.importRecords(file).subscribe({
        next: (response) => ($event.target.value = null),
        error: (error) => {
          this.toastr.error(error);
          $event.target.value = null;
        },
      });
    }
  }

  onImportWalletRecordsPressed($event: any) {
    let fileList: FileList = $event.target.files;
    if (fileList.length > 0) {
      let file: File = fileList[0];
      this.importService.importWalletRecords(file).subscribe({
        next: (response) => ($event.target.value = null),
        error: (error) => {
          this.toastr.error(error);
          $event.target.value = null;
        },
      });
    }
  }

  downloadFile(data: any) {
    const blob = new Blob([data], { type: 'application/json' });
    const url = window.URL.createObjectURL(blob);
    window.open(url);
  }
}
