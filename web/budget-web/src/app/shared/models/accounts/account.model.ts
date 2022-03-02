import { CurrencyModel } from '../currencies/currency.model';

export class AccountModel {
  public id: number;
  public name: string;
  public currency: CurrencyModel;
  public initialBalance: number;
}
