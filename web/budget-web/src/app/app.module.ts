import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './layout/navigation/navigation.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { OverlayModule } from '@angular/cdk/overlay';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoadingSpinnerComponent } from './shared/components/loading-spinner/loading-spinner.component';
import { AuthInterceptor } from './shared/services/auth.interceptor';
import { CreateAccountComponent } from './accounts/create-account/create-account.component';
import { FormWrapperComponent } from './shared/components/form-wrapper/form-wrapper.component';
import { AccountsComponent } from './accounts/accounts.component';
import { EditAccountComponent } from './accounts/edit-account/edit-account.component';
import { AccountFormComponent } from './accounts/account-form/account-form.component';
import { RecordsComponent } from './records/records.component';
import { CreateRecordComponent } from './records/create-record/create-record.component';
import { RecordFormComponent } from './records/record-form/record-form.component';
import { EditRecordComponent } from './records/edit-record/edit-record.component';
import { ChartComponent } from './shared/components/charts/chart/chart.component';
import { CashFlowChartComponent } from './shared/components/charts/cash-flow-chart/cash-flow-chart.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    DashboardComponent,
    AuthenticationComponent,
    LoadingSpinnerComponent,
    CreateAccountComponent,
    FormWrapperComponent,
    AccountsComponent,
    EditAccountComponent,
    AccountFormComponent,
    RecordsComponent,
    CreateRecordComponent,
    RecordFormComponent,
    EditRecordComponent,
    ChartComponent,
    CashFlowChartComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    OverlayModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ToastrModule.forRoot(),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
