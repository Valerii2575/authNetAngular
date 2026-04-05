import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { ɵEmptyOutletComponent, RouterLink } from "@angular/router";
import { FirstKeyPipe } from '../../shared/pipes/first-key-pipe';
import { Auth } from '../../shared/services/auth';
import { error } from 'console';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './registration.html',
  styleUrl: './registration.css',
})
export class Registration {

  form: FormGroup;

  isSubmitted: boolean = false;

  authService = inject(Auth);
  toastr = inject(ToastrService);

constructor(public formBuilder: FormBuilder){
  this.form = this.formBuilder.group({
    fullName: ['', Validators.required],
    email: ['', {validators: [Validators.required, Validators.email]}],
    password: ['', {validators: [Validators.required, Validators.minLength(6), Validators.pattern(/(?=.*[^a-zA-Z0-9 |])/)]}],
    confirmPassword: ['',{validators: [Validators.required]}]
  }, {Validators: this.passwordMatchValidator})
}

passwordMatchValidator: ValidatorFn = (control: AbstractControl) : null => {
  const password = control.get('password');
  const confirmPassword = control.get('confirmPassword');

  if(password && confirmPassword && password.value != confirmPassword.value){
    confirmPassword.setErrors({passwordMisswatch: true})
  }
  else{
    confirmPassword?.setErrors(null);
  }
  return null;
}

onSubmit(){
  this.isSubmitted = true;
  if(!this.form.valid)
  {
    return;
  }

  console.log(this.form.value);
  this.authService.createUser(this.form.value).subscribe({
    next: (res : any) => {
      if(res.succeeded){
        this.form.reset();
        this.isSubmitted = false;
        
      }
      console.log(res);
      this.toastr.success(res);
    },
    error: err => {
      console.log("Error ", err);
      this.toastr.error(err.error, "Error ");
    }
  })
}

hasDisplayableError(controlName: string) : Boolean {
  const control = this.form.get(controlName);
  return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched));
}

}
