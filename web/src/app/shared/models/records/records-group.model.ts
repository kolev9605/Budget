import { RecordModel } from './record.model';

export class RecordsGroupModel {
  constructor(public date: Date, public sum: number, public records: RecordModel[]) {}
}
