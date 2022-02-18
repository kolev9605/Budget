import { Component, OnInit } from '@angular/core';
import { ColorConstants, CommonConstants } from 'src/app/shared/constants';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  formType: string = CommonConstants.SIGN_IN_FORM_TYPE;
  backgroundColor = ColorConstants.BACKGROUND;

  constructor() {}

  ngOnInit(): void {}
}
