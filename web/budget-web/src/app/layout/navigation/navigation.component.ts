import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/shared/services/auth.service';
import { Roles } from 'src/app/shared/constants/constants';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class NavigationComponent implements OnInit, OnDestroy {
  isAuthenticated = false;
  isAdmin = false;

  private userSubscription: Subscription;
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.userSubscription = this.authService.userSubject.pipe().subscribe((user) => {
      this.isAuthenticated = !!user;
      if (this.isAuthenticated) {
        this.isAdmin = !!user.roles.find((r) => r === Roles.Administrator);
      }
    });
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['sign-in']);
  }
}
