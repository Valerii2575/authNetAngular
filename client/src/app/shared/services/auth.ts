import { HttpClient } from '@angular/common/http';
import { inject, Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class Auth {

  baseUrl = environment.apiUrl;
 private http = inject(HttpClient);

  constructor(){ }

  createUser(formData: any) : Observable<any>{
    return this.http.post<any>(this.baseUrl + '/signup', formData);
  }
}
