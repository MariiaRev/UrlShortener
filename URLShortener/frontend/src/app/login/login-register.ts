import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login-register',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './login-register.html',
  styleUrls: ['./login.css']
})

export class Login {
  username = '';
  password = '';
  regUsername = '';
  regPassword = '';
  showLogin = true;

  constructor(private http: HttpClient) {}

  login() {
    const payload = { username: this.username, password: this.password };

    this.http.post<any>('https://localhost:7165/api/auth/login', payload, { withCredentials: true })
      .subscribe({
        next: (response) => {
          localStorage.setItem('token', response.token);
          alert('Login successful!');
          // navigation to other page?
        },
        error: () => alert('Login failed.'),
      });
  }

  register() {
    const payload = { username: this.regUsername, password: this.regPassword };

    this.http.post('https://localhost:7165/api/auth/register', payload, {
      withCredentials: true
    }).subscribe({
      next: () => alert('Registration successful!'),
      error: () => alert('Registration failed.')
    });
  }

  toggleForms() {
    this.showLogin = !this.showLogin;
  }
}
