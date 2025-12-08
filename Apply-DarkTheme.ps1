# PowerShell script to apply Apple Dark Theme to Blazor components

$files = @(
    "bryx CRM\Components\Pages\Sales.razor",
    "bryx CRM\Components\Pages\Purchase.razor",
    "bryx CRM\Components\Pages\Warehouse.razor",
    "bryx CRM\Components\Pages\Movement.razor"
)

$replacements = @{
    # Backgrounds - White
    'background:\s*white\b' = 'background: var(--apple-gradient-card)'
    'background:\s*#ffffff\b' = 'background: var(--apple-gradient-card)'
    'background:\s*#fff\b' = 'background: var(--apple-gradient-card)'
    'background-color:\s*white\b' = 'background: var(--apple-gradient-card)'
    'background-color:\s*#ffffff\b' = 'background: var(--apple-gradient-card)'
    'background-color:\s*#fff\b' = 'background: var(--apple-gradient-card)'

    # Backgrounds - Light Grays
    'background:\s*#f8f9fa' = 'background: var(--apple-bg-secondary)'
    'background:\s*#f5f5f5' = 'background: var(--apple-bg-tertiary)'
    'background:\s*#e9ecef' = 'background: var(--apple-bg-tertiary)'
    'background:\s*#f0f0f0' = 'background: var(--apple-bg-secondary)'
    'background:\s*#fafafa' = 'background: var(--apple-bg-secondary)'
    'background-color:\s*#f8f9fa' = 'background-color: var(--apple-bg-secondary)'
    'background-color:\s*#f5f5f5' = 'background-color: var(--apple-bg-tertiary)'
    'background-color:\s*#e9ecef' = 'background-color: var(--apple-bg-tertiary)'
    'background-color:\s*#f0f0f0' = 'background-color: var(--apple-bg-secondary)'
    'background-color:\s*#fafafa' = 'background-color: var(--apple-bg-secondary)'

    # Special backgrounds - returns, warnings, etc.
    'background:\s*#fff5f5' = 'background: rgba(255, 69, 58, 0.1)'
    'background-color:\s*#fff5f5' = 'background-color: rgba(255, 69, 58, 0.1)'

    # Borders
    'border:\s*1px solid #dee2e6' = 'border: 1px solid var(--apple-border)'
    'border:\s*2px solid #dee2e6' = 'border: 2px solid var(--apple-border)'
    'border:\s*1px solid #e2e8f0' = 'border: 1px solid var(--apple-border)'
    'border:\s*2px solid #e2e8f0' = 'border: 2px solid var(--apple-border)'
    'border:\s*1px solid #ced4da' = 'border: 1px solid var(--apple-border)'
    'border:\s*1px solid #ffecb5' = 'border: 1px solid rgba(255, 159, 10, 0.3)'
    'border-color:\s*#dee2e6' = 'border-color: var(--apple-border)'
    'border-color:\s*#e2e8f0' = 'border-color: var(--apple-border)'
    'border-color:\s*#ced4da' = 'border-color: var(--apple-border)'
    'border-color:\s*#667eea' = 'border-color: var(--apple-blue)'
    'border-bottom:\s*1px solid #dee2e6' = 'border-bottom: 1px solid var(--apple-border)'
    'border-bottom:\s*2px solid #dee2e6' = 'border-bottom: 2px solid var(--apple-border)'
    'border-top:\s*1px solid #dee2e6' = 'border-top: 1px solid var(--apple-border)'

    # Gradients
    'linear-gradient\\(135deg,\\s*#667eea\\s+0%,\\s*#764ba2\\s+100%\\)' = 'var(--apple-gradient-blue)'
    'linear-gradient\\(135deg,\\s*#764ba2\\s+0%,\\s*#667eea\\s+100%\\)' = 'var(--apple-gradient-blue)'

    # Text colors - Primary (Dark)
    'color:\s*#212529' = 'color: var(--apple-text-primary)'
    'color:\s*#495057' = 'color: var(--apple-text-primary)'
    'color:\s*#2d3748' = 'color: var(--apple-text-primary)'
    'color:\s*#1a202c' = 'color: var(--apple-text-primary)'
    'color:\s*black\b' = 'color: var(--apple-text-primary)'
    'color:\s*#000000' = 'color: var(--apple-text-primary)'
    'color:\s*#000\b' = 'color: var(--apple-text-primary)'

    # Text colors - Secondary (Gray)
    'color:\s*#6c757d' = 'color: var(--apple-text-secondary)'
    'color:\s*#718096' = 'color: var(--apple-text-secondary)'
    'color:\s*#4a5568' = 'color: var(--apple-text-secondary)'
    'color:\s*#999' = 'color: var(--apple-text-secondary)'
    'color:\s*#666' = 'color: var(--apple-text-secondary)'

    # Success/Green
    'background:\s*#198754' = 'background: var(--apple-green)'
    'background-color:\s*#198754' = 'background-color: var(--apple-green)'
    '#198754' = 'var(--apple-green)'
    '#157347' = 'var(--apple-green)'
    '#d1e7dd' = 'rgba(48, 209, 88, 0.2)'
    '#0f5132' = 'var(--apple-green)'

    # Error/Red
    'background:\s*#dc3545' = 'background: var(--apple-red)'
    'background-color:\s*#dc3545' = 'background-color: var(--apple-red)'
    '#dc3545' = 'var(--apple-red)'
    '#bb2d3b' = 'var(--apple-red)'
    '#842029' = 'var(--apple-red)'
    '#f8d7da' = 'rgba(255, 69, 58, 0.15)'
    '#f5c2c7' = 'rgba(255, 69, 58, 0.3)'
    '#e53e3e' = 'var(--apple-red)'
    '#c53030' = 'var(--apple-red)'

    # Warning/Orange
    'background:\s*#fd7e14' = 'background: var(--apple-orange)'
    'background-color:\s*#fd7e14' = 'background-color: var(--apple-orange)'
    '#fd7e14' = 'var(--apple-orange)'
    '#e36209' = 'var(--apple-orange)'
    '#fff3cd' = 'rgba(255, 159, 10, 0.2)'
    '#856404' = 'var(--apple-orange)'
    '#664d03' = 'var(--apple-orange)'

    # Blue
    'background:\s*#0d6efd' = 'background: var(--apple-blue)'
    'background-color:\s*#0d6efd' = 'background-color: var(--apple-blue)'
    '#0d6efd' = 'var(--apple-blue)'
    '#0b5ed7' = 'var(--apple-blue)'
    '#084298' = 'var(--apple-blue)'
    '#cfe2ff' = 'rgba(10, 132, 255, 0.2)'
    '#667eea' = 'var(--apple-blue)'

    # Gray/Neutral
    'background:\s*#6c757d' = 'background: var(--apple-bg-elevated)'
    'background-color:\s*#6c757d' = 'background-color: var(--apple-bg-elevated)'
    '#6c757d' = 'var(--apple-text-secondary)'
    '#5a6268' = 'var(--apple-text-secondary)'

    # Purple
    '#9933cc' = 'var(--apple-purple)'
    '#bf5af2' = 'var(--apple-purple)'

    # Border radius
    'border-radius:\s*4px' = 'border-radius: 8px'
    'border-radius:\s*5px' = 'border-radius: 10px'
}

foreach ($file in $files) {
    if (Test-Path $file) {
        Write-Host "Processing $file..." -ForegroundColor Cyan

        # Backup
        $backup = "$file.backup"
        Copy-Item $file $backup -Force
        Write-Host "  Backup created: $backup" -ForegroundColor Gray

        # Read content
        $content = Get-Content $file -Raw -Encoding UTF8

        # Apply replacements
        $changeCount = 0
        foreach ($pattern in $replacements.Keys) {
            $replacement = $replacements[$pattern]
            $matches = [regex]::Matches($content, $pattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
            if ($matches.Count -gt 0) {
                $content = [regex]::Replace($content, $pattern, $replacement, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
                $changeCount += $matches.Count
            }
        }

        # Save
        Set-Content $file -Value $content -Encoding UTF8 -NoNewline
        Write-Host "  Applied $changeCount replacements" -ForegroundColor Green
    } else {
        Write-Host "File not found: $file" -ForegroundColor Red
    }
}

Write-Host "`nDark theme applied successfully!" -ForegroundColor Green
