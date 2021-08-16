param (
  [string] $secretPath,
  [string] $outPath,
  [string] $passphrase
);

$local:key = [Byte[]]($passphrase.PadRight(24).Substring(0, 24).ToCharArray());
$local:content = Get-Content -Path $secretPath -Raw;

$local:secret = $local:content    |
  ConvertTo-SecureString -AsPlainText -Force |
  ConvertFrom-SecureString -Key $local:key   |
  Out-File -FilePath $outPath;