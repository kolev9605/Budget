import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { exhaustMap, take } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.authService.userSubject.pipe(
      take(1),
      exhaustMap((user) => {
        if (!user || !user.token) {
          return next.handle(req);
        }

        if (!this.authService.isTokenValid(user)) {
          this.authService.logout();
          this.router.navigate(['sign-in']);
          return next.handle(req);
        }

        const modifiedRequest = req.clone({
          setHeaders: {
            Authorization: `Bearer ${user.token}`,
          },
        });

        return next.handle(modifiedRequest);
      }),
    );
  }
}
