#!/usr/bin/env python3
"""
Script to apply Apple Dark Theme to Blazor component style blocks
"""

import re
import os

def apply_dark_theme(file_path):
    """Apply dark theme color replacements to a file"""

    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    # Backup original
    backup_path = file_path + '.backup'
    with open(backup_path, 'w', encoding='utf-8') as f:
        f.write(content)

    # Color replacements
    replacements = [
        # Backgrounds
        (r'background:\s*#f8f9fa', 'background: var(--apple-bg-secondary)'),
        (r'background:\s*#f5f5f5', 'background: var(--apple-bg-tertiary)'),
        (r'background:\s*white\b', 'background: var(--apple-bg-secondary)'),
        (r'background:\s*#ffffff', 'background: var(--apple-bg-secondary)'),
        (r'background-color:\s*#f8f9fa', 'background-color: var(--apple-bg-secondary)'),
        (r'background-color:\s*#f5f5f5', 'background-color: var(--apple-bg-tertiary)'),
        (r'background-color:\s*white\b', 'background-color: var(--apple-bg-secondary)'),
        (r'background-color:\s*#ffffff', 'background-color: var(--apple-bg-secondary)'),
        (r'background-color:\s*#e9ecef', 'background-color: var(--apple-bg-tertiary)'),

        # Borders
        (r'border:\s*1px solid #dee2e6', 'border: 1px solid var(--apple-border)'),
        (r'border:\s*2px solid #dee2e6', 'border: 2px solid var(--apple-border)'),
        (r'border:\s*1px solid #e2e8f0', 'border: 1px solid var(--apple-border)'),
        (r'border:\s*2px solid #e2e8f0', 'border: 2px solid var(--apple-border)'),
        (r'border:\s*1px solid #ced4da', 'border: 1px solid var(--apple-border)'),
        (r'border-color:\s*#dee2e6', 'border-color: var(--apple-border)'),
        (r'border-color:\s*#e2e8f0', 'border-color: var(--apple-border)'),
        (r'border-color:\s*#ced4da', 'border-color: var(--apple-border)'),
        (r'border-bottom:\s*1px solid #dee2e6', 'border-bottom: 1px solid var(--apple-border)'),
        (r'border-bottom:\s*2px solid #dee2e6', 'border-bottom: 2px solid var(--apple-border)'),
        (r'border-left:\s*4px solid #c53030', 'border-left: 4px solid var(--apple-red)'),

        # Gradients
        (r'linear-gradient\(135deg,\s*#667eea\s+0%,\s*#764ba2\s+100%\)', 'var(--apple-gradient-blue)'),
        (r'linear-gradient\(135deg,\s*#764ba2\s+0%,\s*#667eea\s+100%\)', 'var(--apple-gradient-blue)'),
        (r'background:\s*linear-gradient\([^)]+#667eea[^)]+\)', 'background: var(--apple-gradient-blue)'),

        # Text colors
        (r'color:\s*#212529', 'color: var(--apple-text-primary)'),
        (r'color:\s*#495057', 'color: var(--apple-text-primary)'),
        (r'color:\s*#2d3748', 'color: var(--apple-text-primary)'),
        (r'color:\s*#6c757d', 'color: var(--apple-text-secondary)'),
        (r'color:\s*#718096', 'color: var(--apple-text-secondary)'),
        (r'color:\s*#4a5568', 'color: var(--apple-text-secondary)'),
        (r'color:\s*#999', 'color: var(--apple-text-secondary)'),

        # Success/Green
        (r'background:\s*#198754', 'background: var(--apple-green)'),
        (r'background-color:\s*#198754', 'background-color: var(--apple-green)'),
        (r'#198754', 'var(--apple-green)'),
        (r'#157347', 'var(--apple-green)'),
        (r'#d1e7dd', 'rgba(48, 209, 88, 0.2)'),
        (r'#0f5132', 'var(--apple-green)'),

        # Error/Red
        (r'background:\s*#dc3545', 'background: var(--apple-red)'),
        (r'background-color:\s*#dc3545', 'background-color: var(--apple-red)'),
        (r'#dc3545', 'var(--apple-red)'),
        (r'#bb2d3b', 'var(--apple-red)'),
        (r'#842029', 'var(--apple-red)'),
        (r'#f8d7da', 'rgba(255, 69, 58, 0.15)'),
        (r'#f5c2c7', 'rgba(255, 69, 58, 0.3)'),
        (r'#e53e3e', 'var(--apple-red)'),
        (r'#c53030', 'var(--apple-red)'),

        # Warning/Orange
        (r'#fff3cd', 'rgba(255, 159, 10, 0.2)'),
        (r'#856404', 'var(--apple-orange)'),
        (r'#664d03', 'var(--apple-orange)'),

        # Blue
        (r'#0d6efd', 'var(--apple-blue)'),
        (r'#0b5ed7', 'var(--apple-blue)'),
        (r'#667eea', 'var(--apple-blue)'),
        (r'#764ba2', 'var(--apple-purple)'),
        (r'#084298', 'var(--apple-blue)'),
        (r'#cfe2ff', 'rgba(10, 132, 255, 0.2)'),

        # Purple
        (r'#9933cc', 'var(--apple-purple)'),

        # Shadows
        (r'box-shadow:\s*0 2px 8px rgba\(0,\s*0,\s*0,\s*0\.05\)', 'box-shadow: var(--apple-shadow-sm)'),
        (r'box-shadow:\s*0 4px 12px rgba\(0,\s*0,\s*0,\s*0\.1\)', 'box-shadow: var(--apple-shadow-md)'),
        (r'box-shadow:\s*0 4px 20px rgba\(0,\s*0,\s*0,\s*0\.3\)', 'box-shadow: var(--apple-shadow-lg)'),
        (r'box-shadow:\s*0 4px 15px rgba\(102,\s*126,\s*234,\s*0\.4\)', 'box-shadow: var(--apple-shadow-md)'),

        # Border radius
        (r'border-radius:\s*4px', 'border-radius: 8px'),
        (r'border-radius:\s*5px', 'border-radius: 8px'),
    ]

    for pattern, replacement in replacements:
        content = re.sub(pattern, replacement, content, flags=re.IGNORECASE)

    # Write updated content
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(content)

    print(f"✅ Updated: {file_path}")
    print(f"   Backup saved to: {backup_path}")

if __name__ == "__main__":
    files_to_update = [
        r"bryx CRM\Components\Pages\Sales.razor",
        r"bryx CRM\Components\Pages\Purchase.razor",
        r"bryx CRM\Components\Pages\Warehouse.razor"
    ]

    for file_path in files_to_update:
        if os.path.exists(file_path):
            print(f"\nProcessing {file_path}...")
            apply_dark_theme(file_path)
        else:
            print(f"❌ File not found: {file_path}")

    print("\n✨ Dark theme application complete!")
