import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-wrapper',
  templateUrl: './form-wrapper.component.html',
  styleUrls: [],
})
export class FormWrapperComponent implements OnInit {
  @Input()
  public formTitle: string;

  constructor() {}

  ngOnInit(): void {}
}
