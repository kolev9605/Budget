export class AuthenticatedUserModel {
  constructor(public token: string, public validTo: Date, public roles: string[]) {}
}
