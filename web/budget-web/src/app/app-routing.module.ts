import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountsComponent } from './accounts/accounts.component';
import { CreateAccountComponent } from './accounts/create-account/create-account.component';
import { EditAccountComponent } from './accounts/edit-account/edit-account.component';
import { AdminComponent } from './admin/admin.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { CategoriesComponent } from './categories/categories.component';
import { CreateCategoryComponent } from './categories/create-category/create-category.component';
import { EditCategoryComponent } from './categories/edit-category/edit-category.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ImportComponent } from './import/import.component';
import { CreateRecordComponent } from './records/create-record/create-record.component';
import { EditRecordComponent } from './records/edit-record/edit-record.component';
import { RecordsComponent } from './records/records.component';
import { AdminGuard } from './shared/services/admin.guard';
import { AuthGuard } from './shared/services/auth.guard';

const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'sign-in', component: AuthenticationComponent },
  {
    path: 'accounts',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: AccountsComponent },
      { path: 'create', component: CreateAccountComponent, pathMatch: 'full' },
      { path: 'edit/:accountId', component: EditAccountComponent, pathMatch: 'full' },
    ],
  },
  {
    path: 'records',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: RecordsComponent },
      { path: 'create', component: CreateRecordComponent, pathMatch: 'full' },
      { path: 'edit/:accountId', component: EditRecordComponent, pathMatch: 'full' },
    ],
  },
  {
    path: 'categories',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: CategoriesComponent },
      { path: 'create', component: CreateCategoryComponent, pathMatch: 'full' },
      { path: 'edit/:categoryId', component: EditCategoryComponent, pathMatch: 'full' },
    ],
  },
  { path: 'import', component: ImportComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminComponent, canActivate: [AdminGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
