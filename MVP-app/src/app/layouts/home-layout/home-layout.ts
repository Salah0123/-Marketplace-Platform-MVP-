import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { Sidebar } from '../../shared/sidebar/sidebar';

@Component({
  selector: 'app-home-layout',
  imports: [Sidebar, RouterOutlet],
  templateUrl: './home-layout.html',
  styleUrl: './home-layout.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeLayoutComponent {}

