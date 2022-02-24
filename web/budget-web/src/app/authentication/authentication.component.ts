import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ColorConstants } from 'src/app/shared/constants';
import { LoginModel } from 'src/app/shared/models/authentication/login.model';
import { RegisterModel } from 'src/app/shared/models/authentication/register.model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss'],
})
export class AuthenticationComponent implements OnInit {
  form: FormGroup;
  isLogin: boolean = true;
  isLoading: boolean = false;
  backgroundColor = ColorConstants.Background;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', Validators.required],
      email: ['', null],
    });
  }

  onSubmit(): void {
    if (this.isLogin) {
      this.login();
    } else {
      this.register();
    }
  }

  onFormTypeChanged(): void {
    this.isLogin = !this.isLogin;
  }

  login() {
    this.isLoading = true;
    const loginModel: LoginModel = new LoginModel(
      this.form.value.username,
      this.form.value.password,
    );

    this.authService.login(loginModel).subscribe(
      (res) => {
        console.log(res);
        this.isLoading = false;
      },
      (err) => {
        this.toastr.error(err);
        this.isLoading = false;
      },
    );

    this.form.reset();
  }

  register() {
    const registerModel: RegisterModel = new RegisterModel(
      this.form.value.username,
      this.form.value.password,
      this.form.value.email,
    );

    this.authService.register(registerModel).subscribe(
      (res) => {
        this.toastr.success('Registration successful!');
        this.isLogin = !this.isLogin;
      },
      (err) => {
        this.toastr.error(err);
      },
    );

    this.form.reset();
  }
}
