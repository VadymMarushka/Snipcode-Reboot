import { Component } from '@angular/core';
import { Header } from './core/layout/header/header';
import { RouterOutlet } from '@angular/router';
import { Footer } from "./core/layout/footer/footer";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [Header, RouterOutlet, Footer],
  templateUrl: './app.html',
})
export class App {}