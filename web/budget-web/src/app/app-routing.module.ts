import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountsComponent } from './accounts/accounts.component';
import { CreateAccountComponent } from './accounts/create-account/create-account.component';
import { EditAccountComponent } from './accounts/edit-account/edit-account.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CreateRecordComponent } from './records/create-record/create-record.component';
import { RecordsComponent } from './records/records.component';
import { AuthGuard } from './shared/services/auth.guard';

const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
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
      // { path: 'edit/:accountId', component: EditAccountComponent, pathMatch: 'full' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
