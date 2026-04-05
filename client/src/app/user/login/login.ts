import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Auth } from '../../shared/services/auth';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {


  form: FormGroup;

  isSubmitted: boolean = false;

  authService = inject(Auth);

  constructor(public formBuilder: FormBuilder){
  this.form = this.formBuilder.group({
    email: ['', {validators: [Validators.required, Validators.email]}],
    password: ['', {validators: [Validators.required]}]    
  });
}

onSubmit() {
  this.isSubmitted = true;
}

hasDisplayableError(controlName: string) : Boolean {
  const control = this.form.get(controlName);
  return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched));
}

}
