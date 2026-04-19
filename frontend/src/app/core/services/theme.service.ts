import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private renderer: Renderer2;
  private isDarkMode = true;

  constructor(rendererFactory: RendererFactory2) {
    this.renderer = rendererFactory.createRenderer(null, null);
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'light') {
      this.setLightMode();
    } else {
      this.setDarkMode();
    }
  }

  toggleTheme() {
    if (this.isDarkMode) {
      this.setLightMode();
    } else {
      this.setDarkMode();
    }
  }

  private setLightMode() {
    this.isDarkMode = false;
    this.renderer.addClass(document.body, 'light-mode');
    localStorage.setItem('theme', 'light');
  }

  private setDarkMode() {
    this.isDarkMode = true;
    this.renderer.removeClass(document.body, 'light-mode');
    localStorage.setItem('theme', 'dark');
  }

  getIsDarkMode() {
    return this.isDarkMode;
  }
}
