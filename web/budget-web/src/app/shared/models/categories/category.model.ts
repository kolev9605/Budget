export class CategoryModel {
  public id: string;
  public name: string;
  public categoryType: string;
  public parentCategoryId: string;
  public subCategories: CategoryModel[];
  public isInitial: boolean;
}
