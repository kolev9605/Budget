import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  CanActivate,
  Router,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { take, map } from 'rxjs/operators';
import { Roles } from 'src/app/shared/constants/constants';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.authService.userSubject.pipe(
      take(1),
      map((user) => {
        console.log('user', user);

        const isAdmin = user && user.roles.find((r) => r === Roles.Administrator);
        if (isAdmin) {
          return true;
        } else {
          return this.router.createUrlTree(['']);
        }
      }),
    );
  }
}
