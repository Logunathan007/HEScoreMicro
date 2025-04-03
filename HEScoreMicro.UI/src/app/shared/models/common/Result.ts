export interface Result<T>{
  failed:boolean,
  message:string
  data?:T
}
