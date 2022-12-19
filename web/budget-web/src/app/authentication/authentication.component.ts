import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ColorConstants } from 'src/app/shared/constants/constants';
import { LoginModel } from 'src/app/shared/models/authentication/login.model';
import { RegisterModel } from 'src/app/shared/models/authentication/register.model';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: [],
})
export class AuthenticationComponent implements OnInit {
  public isLogin: boolean = true;
  public isLoading: boolean = false;
  public backgroundColor = ColorConstants.Background;
  public form: UntypedFormGroup;

  constructor(
    private fb: UntypedFormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      username: [null, [Validators.required]],
      password: [null, [Validators.required]],
      email: [null, [Validators.email]],
    });
  }

  onSubmit(): void {
    if (!this.form.valid) {
      Object.keys(this.form.controls).forEach((field) => {
        const control = this.form.get(field);
        control?.markAsTouched({ onlySelf: true });
      });

      return;
    }

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

    this.authService.login(loginModel).subscribe({
      next: (res) => {
        this.toastr.success('Login successful!');
        this.router.navigate(['dashboard']);
      },
      error: (err) => {
        this.toastr.error(err);
      },
      complete: () => (this.isLoading = false),
    });

    this.form.reset();
  }

  register() {
    this.isLoading = true;
    const registerModel: RegisterModel = new RegisterModel(
      this.form.value.username,
      this.form.value.password,
      this.form.value.email,
    );

    this.authService.register(registerModel).subscribe({
      next: (res) => {
        this.toastr.success('Registration successful!');
        this.isLogin = !this.isLogin;
      },
      error: (err) => {
        this.toastr.error(err);
      },
      complete: () => (this.isLoading = false),
    });

    this.form.reset();
  }
}
