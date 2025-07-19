import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-short-url-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './short-url-list.html',
  styleUrl: './short-url-list.css'
})

export class ShortUrlList implements OnInit {
  urls: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any[]>('https://localhost:7165/api/shorturls', { withCredentials: true })
      .subscribe({
        next: data => this.urls = data,
        error: () => alert('Failed to load URLs')
      });
  }
}