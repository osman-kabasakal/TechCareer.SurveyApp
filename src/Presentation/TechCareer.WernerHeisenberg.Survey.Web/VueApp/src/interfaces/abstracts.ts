export interface IViewModel {
  user?: UserDto;
  model?: any;
  validationError?: {
    [key: string]: string[];
  };
}



export interface UserDto {
  Id: number;
  UserName: string;
  Email: string;
  PhoneNumber: string;
  Firstname: string;
  Lastname: string;
  SystemUser: boolean;
  Deleted: boolean;
  Roles: RoleDto[];
}


export interface RoleDto {
  Id: number;
  Name: string;
  Deleted: boolean;
  SystemRole: boolean;
  AssignedUsers: UserDto[];
}
