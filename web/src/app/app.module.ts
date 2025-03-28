import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './layout/navigation/navigation.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthenticationComponent } from './authentication/authentication.component';
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
import { AuthService } from './shared/services/auth.service';
import { Router } from '@angular/router';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryFormComponent } from './categories/category-form/category-form.component';
import { CreateCategoryComponent } from './categories/create-category/create-category.component';
import { EditCategoryComponent } from './categories/edit-category/edit-category.component';
import { AdminComponent } from './admin/admin.component';
import { NgxScrollTopModule } from 'ngx-scrolltop';
import { ImportComponent } from './import/import.component';

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
    CategoriesComponent,
    CategoryFormComponent,
    CreateCategoryComponent,
    EditCategoryComponent,
    AdminComponent,
    ImportComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ToastrModule.forRoot(),
    InfiniteScrollModule,
    NgxScrollTopModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
      deps: [AuthService, Router],
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
