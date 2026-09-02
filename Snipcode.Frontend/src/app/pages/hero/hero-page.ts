import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HlmButtonImports } from '@spartan-ng/helm/button';

@Component({
  selector: 'app-hero-page',
  standalone: true,
  imports: [RouterLink, HlmButtonImports],
  templateUrl: './hero-page.html',
  styleUrl: './hero-page.css',
})
export class HeroPage {}