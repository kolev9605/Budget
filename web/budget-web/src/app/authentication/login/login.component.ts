import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ColorConstants } from 'src/app/shared/constants';
import { LoginModel } from 'src/app/shared/models/authentication/login.model';
import { RegisterModel } from 'src/app/shared/models/authentication/register.model';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  isLogin: boolean = false;
  backgroundColor = ColorConstants.BACKGROUND;

  constructor(private fb: FormBuilder, private authService: AuthService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', Validators.required],
      email: ['', null],
    });
  }

  onSubmit(): void {
    console.log('form', this.form);

    if (this.isLogin) {
      this.login();
    } else {
      this.register();
    }
  }

  onFormTypeChanged(): void {
    console.log('here', this.isLogin);

    this.isLogin = !this.isLogin;
  }

  login() {
    const loginModel: LoginModel = new LoginModel(
      this.form.value.username,
      this.form.value.password,
    );

    this.authService.login(loginModel).subscribe((x) => {
      console.log(x);
    });
  }

  register() {
    const registerModel: RegisterModel = new RegisterModel(
      this.form.value.username,
      this.form.value.password,
      this.form.value.email,
    );

    this.authService.register(registerModel).subscribe((x) => {
      console.log(x);
    });
  }
}
