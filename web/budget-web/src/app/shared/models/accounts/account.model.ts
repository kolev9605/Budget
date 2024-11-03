import { CurrencyModel } from '../currencies/currency.model';

export class AccountModel {
  public id: string;
  public name: string;
  public initialBalance: number;
  public currency: CurrencyModel;
  public balance: number;
}
