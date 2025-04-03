import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap(
        (res: any) => {
          if (res?.body) {
            const message = res?.body?.message || 'No message';
            const status = res?.status || 'No status';

            if (res?.body?.failed) {
              this.toastr.warning(`${status} ${message}`);
            } else {
              this.toastr.success(`${status} ${message}`);
            }
          }
        },
        (error: any) => {
          this.toastr.error(`ERROR Occurred: ${error.message || error}`);
        }
      )
    );
  }

}
