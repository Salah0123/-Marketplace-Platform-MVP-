import { Component } from '@angular/core';
import { Sidebar } from "../../shared/sidebar/sidebar";
import { RouterOutlet } from "@angular/router";

@Component({
  selector: 'app-app-layout',
  imports: [Sidebar, RouterOutlet],
  templateUrl: './app-layout.html',
  styleUrl: './app-layout.scss',
})
export class AppLayout {

}
