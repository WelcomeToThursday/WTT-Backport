@echo off
setlocal

git rev-parse --show-toplevel >nul 2>&1
if errorlevel 1 (
    echo Run this batch file from a folder inside a Git working tree.
    exit /b 1
)

if not exist ".gitattributes" type nul > ".gitattributes"
if errorlevel 1 (
    echo Could not create .gitattributes in the current folder.
    exit /b 1
)

powershell -NoProfile -ExecutionPolicy Bypass -Command ^
  "$ErrorActionPreference = 'Stop';" ^
  "$root = (Get-Location).Path;" ^
  "$limit = 98000000;" ^
  "Write-Host ('Scanning: ' + $root);" ^
  "$dirs = New-Object 'System.Collections.Generic.Stack[string]';" ^
  "$dirs.Push($root);" ^
  "$rules = New-Object 'System.Collections.Generic.List[string]';" ^
  "$scanned = 0; $oversized = 0; $alreadyMarked = 0;" ^
  "while ($dirs.Count -gt 0) {" ^
  "  $dir = $dirs.Pop();" ^
  "  foreach ($file in (Get-ChildItem -LiteralPath $dir -File -Force)) {" ^
  "    $scanned++;" ^
  "    if ($file.Length -gt $limit) {" ^
  "      $oversized++;" ^
  "      $relative = $file.FullName.Substring($root.Length).TrimStart('\').Replace('\','/');" ^
  "      $attribute = & git check-attr filter -- $relative;" ^
  "      if ($LASTEXITCODE -ne 0) { throw ('git check-attr failed for ' + $relative) };" ^
  "      if ($attribute -match ': filter: lfs$') { $alreadyMarked++ } else {" ^
  "        $pattern = '/' + $relative.Replace('[','\[').Replace(']','\]').Replace(' ','[[:space:]]');" ^
  "        $rules.Add($pattern + ' filter=lfs diff=lfs merge=lfs -text');" ^
  "        Write-Host ('Marking: ' + $relative)" ^
  "      }" ^
  "    }" ^
  "  }" ^
  "  foreach ($child in (Get-ChildItem -LiteralPath $dir -Directory -Force)) {" ^
  "    if ($child.Name -ne '.git' -and -not ($child.Attributes -band [IO.FileAttributes]::ReparsePoint)) {" ^
  "      $dirs.Push($child.FullName)" ^
  "    }" ^
  "  }" ^
  "};" ^
  "if ($rules.Count -gt 0) {" ^
  "  $attributes = Join-Path $root '.gitattributes';" ^
  "  $needsNewline = $false;" ^
  "  if ((Test-Path -LiteralPath $attributes) -and (Get-Item -LiteralPath $attributes).Length -gt 0) {" ^
  "    $stream = [IO.File]::OpenRead($attributes);" ^
  "    try { [void]$stream.Seek(-1, [IO.SeekOrigin]::End); $needsNewline = ($stream.ReadByte() -ne 10) } finally { $stream.Dispose() }" ^
  "  };" ^
  "  $writer = New-Object IO.StreamWriter($attributes, $true, (New-Object Text.UTF8Encoding($false)));" ^
  "  try { if ($needsNewline) { $writer.WriteLine() }; foreach ($rule in $rules) { $writer.WriteLine($rule) } } finally { $writer.Dispose() }" ^
  "};" ^
  "Write-Host ('Files scanned: ' + $scanned + '; over 95 MB: ' + $oversized + '; already marked: ' + $alreadyMarked + '.');" ^
  "Write-Host ('Added ' + $rules.Count + ' rule(s) to .gitattributes.')"

exit /b %ERRORLEVEL%