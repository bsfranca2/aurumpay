export interface ProblemDetail {
  type: string
  title: string
  status: number
  detail: string
  instance: string
  validationErrors?: Array<{ path: string, message: string }>
}

export interface Store {
  id: string
  merchantId: string
  name: string
}

// export interface SignInRequest {
//   email: string
//   password: string
// }

// export interface SignInResponse {
//   tokenType: string
//   token: string
// }

// export interface MeResponse {
//   organizationId: string
//   organizationUserId: string
//   isAccountNonExpired: boolean
//   isAccountNonLocked: boolean
//   isCredentialsNonExpired: boolean
//   id: string
//   isEnabled: boolean
//   authorities: Array<{ authority: string }>
//   username: string
// }
