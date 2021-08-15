param (
  [string] $secretPath,
  [string] $outPath,
  [string] $passphrase
);

$local:key = [Byte[]]($passphrase.PadRight(24).Substring(0, 24).ToCharArray());
$local:encryptedContent = Get-Content -Path $secretPath -Raw;

try {
  $local:decryptedAsSecureString = $local:encryptedContent | ConvertTo-SecureString -Key $local:key -ErrorAction Stop;

  $local:credentials = New-Object -TypeName System.Management.Automation.PSCredential('dummy', $local:decryptedAsSecureString);
  $local:decryptedText = $local:credentials.GetNetworkCredential().Password;

  $local:decryptedText | Out-File -FilePath $outPath;

  $local:result = 'success.'
}
catch {
  $local:result = '(wrong key)';
}

Write-Host $local:result;

