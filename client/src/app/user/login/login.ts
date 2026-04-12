import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Auth } from '../../shared/services/auth';
import {Router, RouterLink} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TOKEN_KEY } from '../../shared/constants';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {

  form: FormGroup;

  isSubmitted: boolean = false;

  authService = inject(Auth);
  private router = inject(Router);
  toastr = inject(ToastrService);

  constructor(public formBuilder: FormBuilder
  ){
  this.form = this.formBuilder.group({
    email: ['', {validators: [Validators.required, Validators.email]}],
    password: ['', {validators: [Validators.required]}]    
  });
}
  ngOnInit(): void {
    if(this.authService.isLoggedIn()){
      this.router.navigateByUrl('/dashboard');
    }
  }

onSubmit() {
  this.isSubmitted = true;
  if(!this.form.valid)
    return;

  this.authService.signin(this.form.value).subscribe({
    next:  (res: any) =>{
      this.authService.saveToken(res.token);
      this.router.navigateByUrl('/dashboard');
    },
    error: (res:any) => {
      if(res.status == 400)
        this.toastr.error('incorrect email or password', 'Login failed');
      else
        console.log(res.value);
    }
  })

}

hasDisplayableError(controlName: string) : Boolean {
  const control = this.form.get(controlName);
  return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched));
}

}
