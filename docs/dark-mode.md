# Dark Mode Feature

This document describes the dark mode implementation in the eShop WebApp.

## Overview

The dark mode feature allows users to switch between light and dark themes. The selected theme is stored in a cookie and persists across browser sessions.

## Architecture

### Services

**ThemeService** (`src/WebApp/Services/ThemeService.cs`)
- Manages the current theme state
- Provides methods to toggle and set the theme
- Handles cookie read/write operations
- Includes input validation to prevent XSS attacks

**ThemeMiddleware** (`src/WebApp/Middleware/ThemeMiddleware.cs`)
- Reads the theme from the cookie on each request
- Sets the theme in the ThemeService for the current request

### UI Components

**ThemeToggle** (`src/WebApp/Components/Layout/ThemeToggle.razor`)
- Displays a sun icon when in light mode
- Displays a moon icon when in dark mode
- Toggles the theme when clicked
- Updates the cookie and DOM immediately

### Styling

**CSS Variables** (`src/WebApp/wwwroot/css/app.css`)
- Defines CSS custom properties for colors:
  - `--bg-primary`, `--bg-secondary` - background colors
  - `--text-primary`, `--text-secondary` - text colors
  - `--border-color`, `--border-light` - border colors
  - `--button-bg`, `--button-text` - button colors
  - `--badge-bg`, `--badge-text` - badge colors
  - `--footer-bg`, `--footer-text` - footer colors

**Theme Selector** (`[data-theme="dark"]`)
- Applied to the HTML element
- Overrides CSS variables for dark mode
- Component CSS files reference these variables

## Usage

### For Developers

To use the theme in new components, reference the CSS variables:

```css
.my-component {
    background-color: var(--bg-primary);
    color: var(--text-primary);
    border: 1px solid var(--border-color);
}
```

### For Users

1. Click the sun/moon icon in the header navigation bar
2. The theme will toggle between light and dark mode
3. The preference is saved automatically and persists across sessions

## Security

### Input Validation

All theme values are validated to only allow "light" or "dark":
- `ThemeService.SetTheme()` - validates before setting
- `ThemeService.GetThemeFromCookie()` - validates cookie value
- `ThemeService.SetThemeCookie()` - validates before saving
- JavaScript functions validate before DOM manipulation

### Cookie Security

The theme cookie has the following security settings:
- `HttpOnly=false` - allows JavaScript to read for instant updates (safe due to validation)
- `SameSite=Lax` - protects against CSRF attacks
- `Secure=true` - only sent over HTTPS when available
- `IsEssential=true` - marks as essential for GDPR compliance
- `MaxAge=365 days` - provides long-term persistence

### XSS Prevention

- No use of `eval()` or unsafe JavaScript
- All theme values validated before use
- Safe JavaScript function `setThemeAttribute()` used for DOM updates

## FOUC Prevention

Flash of Unstyled Content (FOUC) is prevented by:
1. Inline JavaScript in `App.razor` that runs immediately on page load
2. Reads the theme cookie before Blazor initializes
3. Applies the `data-theme` attribute to the HTML element
4. CSS variables are applied instantly

## Browser Compatibility

- Works in all modern browsers that support CSS custom properties
- Fallback to light mode if JavaScript is disabled
- Cookie API is universally supported
