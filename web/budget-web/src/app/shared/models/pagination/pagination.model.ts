export class PaginationModel<T> {
  public items: T[];
  public pageNumber: number;
  public totalPages: number;
  public hasPreviousPage: boolean;
  public hasNextPage: boolean;
}
