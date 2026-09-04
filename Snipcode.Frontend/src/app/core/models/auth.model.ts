export interface AuthResponseDto {
  accessToken: string;
  refreshToken: string;
  username: string;
  email: string;
  accessTokenExpiration: string;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  username: string;
  email: string;
  password: string;
}

export interface RefreshTokenRequestDto {
  accessToken: string;
  refreshToken: string;
}

export interface UserProfileDto {
  id: string; // Guid
  username: string;
  email: string;
  snippetCount: number;
  groupCount: number;
}