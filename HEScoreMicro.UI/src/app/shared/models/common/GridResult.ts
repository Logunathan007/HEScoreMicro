export interface GridResult<T>{
  failed:boolean,
  message:string
  data?:T[]
}
