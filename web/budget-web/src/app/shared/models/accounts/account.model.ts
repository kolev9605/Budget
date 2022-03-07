import { CurrencyModel } from '../currencies/currency.model';

export class AccountModel {
  public id: number;
  public name: string;
  public initialBalance: number;
  public currency: CurrencyModel;
}
