import { AccountModel } from '../accounts/account.model';
import { CategoryModel } from '../categories/category.model';
import { PaymentTypeModel } from '../payment-types/payment-type.model';

export class RecordModel {
  public id: number;
  public note: string;
  public fromAccount: AccountModel;
  public account: AccountModel;
  public recordType: string;
  public paymentType: PaymentTypeModel;
  public category: CategoryModel;
  public dateAdded: Date;
  public recordDate: Date;
  public amount: number;
}
