import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './layout/navigation/navigation.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './authentication/login/login.component';
import { OverlayModule, OverlayRef } from '@angular/cdk/overlay';
import { ProgressSpinnerComponent } from './shared/components/progress-spinner/progress-spinner.component';
@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    DashboardComponent,
    HomeComponent,
    LoginComponent,
    ProgressSpinnerComponent,
  ],
  imports: [BrowserModule, AppRoutingModule, OverlayModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
