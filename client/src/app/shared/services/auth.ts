import { HttpClient } from '@angular/common/http';
import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { isPlatformBrowser } from '@angular/common';
import { TOKEN_KEY } from '../constants';

@Injectable({
  providedIn: 'root',
})
export class Auth {

  baseUrl = environment.apiUrl;
 private http = inject(HttpClient);
 private platformId = inject(PLATFORM_ID);

  constructor(){ }

  createUser(formData: any) : Observable<any>{
    return this.http.post<any>(this.baseUrl + '/signup', formData);
  }

  signin(formData: any){
    return this.http.post<any>(this.baseUrl + `/signin`, formData);
  }

  saveToken(token: string){
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.setItem(TOKEN_KEY, token);
    }
  }

  deleteToken(){
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.removeItem(TOKEN_KEY);
    }
  }

  isLoggedIn(){
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(TOKEN_KEY) != null;
    }
    return false; // SSR safe fallback
  }
}
