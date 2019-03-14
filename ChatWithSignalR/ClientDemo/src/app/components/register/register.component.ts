import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  username = '';
  password = '';

  constructor(private userService: UserService, private router: Router) {}

  register() {
    if (this.username && this.password) {
      this.userService
        .register(this.username, this.password)
        .then(r => this.router.navigateByUrl('/chat'));
    }
  }
}
