import { Component } from '@angular/core';
import { Registration } from './registration/registration';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-user',
  imports: [RouterOutlet, Registration],
  templateUrl: './user.html',
  styleUrl: './user.css'
})
export class User {}

