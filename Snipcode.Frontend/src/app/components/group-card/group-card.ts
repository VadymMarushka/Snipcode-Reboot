import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { 
  lucideFolderOpen, 
  lucideFiles, 
  lucideUser, 
  lucideCalendarDays,
  lucideArrowRight,
} from '@ng-icons/lucide';

// Spartan UI Imports
import { HlmCardImports } from '@spartan-ng/helm/card';
import { HlmBadgeImports } from '@spartan-ng/helm/badge';
import { GroupResponseDto } from '../../core/models/group.model';

@Component({
  selector: 'app-group-card',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    NgIconComponent,
    HlmCardImports,
    HlmBadgeImports
],
  providers: [
    provideIcons({
      lucideFolderOpen,
      lucideFiles,
      lucideUser,
      lucideCalendarDays,
      lucideArrowRight
    })
  ],
  templateUrl: './group-card.html'
})
export class GroupCard {
  group = input.required<GroupResponseDto>();
}