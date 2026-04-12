import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Auth } from '../shared/services/auth';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {

  private router = inject(Router) 
  private authService = inject(Auth);

  onLogout(){
    this.authService.deleteToken();
    this.router.navigateByUrl('/user/signin');
  }
}
