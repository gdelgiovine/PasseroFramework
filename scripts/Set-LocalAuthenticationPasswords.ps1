param(
	[string]$IdentityFilePath = (Join-Path $PSScriptRoot "..\PasseroDemo.Views\Security\UsersAndRoles.json")
)

$ErrorActionPreference = "Stop"
$iterations = 210000
$hashSize = 32

$identityPath = [System.IO.Path]::GetFullPath($IdentityFilePath)
if (-not [System.IO.File]::Exists($identityPath)) {
	throw "File identità non trovato: $identityPath"
}

$document = Get-Content -LiteralPath $identityPath -Raw | ConvertFrom-Json
if ($null -eq $document.Users) {
	throw "Il file non contiene l'array Users: $identityPath"
}

foreach ($userName in @("test", "Administrator")) {
	$user = @($document.Users | Where-Object { $_.UserName -ieq $userName }) | Select-Object -First 1
	if ($null -eq $user) {
		throw "L'utente '$userName' non esiste in $identityPath"
	}

	$securePassword = Read-Host "Inserisci la nuova password per '$userName'" -AsSecureString
	if ($securePassword.Length -eq 0) {
		throw "La password per '$userName' non può essere vuota."
	}

	$passwordPointer = [IntPtr]::Zero
	$passwordBytes = $null
	$salt = New-Object byte[] 16
	$derivedHash = $null
	try {
		$passwordPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
		$plainPassword = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($passwordPointer)
		$passwordBytes = [System.Text.Encoding]::UTF8.GetBytes($plainPassword)

		$random = [System.Security.Cryptography.RandomNumberGenerator]::Create()
		try {
			$random.GetBytes($salt)
		}
		finally {
			$random.Dispose()
		}

		$deriveBytes = New-Object System.Security.Cryptography.Rfc2898DeriveBytes(
			$passwordBytes,
			$salt,
			$iterations,
			[System.Security.Cryptography.HashAlgorithmName]::SHA256
		)
		try {
			$derivedHash = $deriveBytes.GetBytes($hashSize)
		}
		finally {
			$deriveBytes.Dispose()
		}

		$user | Add-Member -NotePropertyName PasswordSalt -NotePropertyValue ([Convert]::ToBase64String($salt)) -Force
		$user | Add-Member -NotePropertyName PasswordHash -NotePropertyValue ([Convert]::ToBase64String($derivedHash)) -Force
		$user | Add-Member -NotePropertyName PasswordIterations -NotePropertyValue $iterations -Force
	}
	finally {
		if ($passwordPointer -ne [IntPtr]::Zero) {
			[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($passwordPointer)
		}
		if ($null -ne $passwordBytes) {
			[System.Security.Cryptography.CryptographicOperations]::ZeroMemory($passwordBytes)
		}
		if ($null -ne $salt) {
			[System.Security.Cryptography.CryptographicOperations]::ZeroMemory($salt)
		}
		if ($null -ne $derivedHash) {
			[System.Security.Cryptography.CryptographicOperations]::ZeroMemory($derivedHash)
		}
		$securePassword.Dispose()
	}
}

$json = ConvertTo-Json -InputObject $document -Depth 20
$tempPath = "$identityPath.$([Guid]::NewGuid().ToString('N')).tmp"
try {
	[System.IO.File]::WriteAllText($tempPath, $json, (New-Object System.Text.UTF8Encoding($false)))
	[System.IO.File]::Replace($tempPath, $identityPath, $null)
}
finally {
	if ([System.IO.File]::Exists($tempPath)) {
		[System.IO.File]::Delete($tempPath)
	}
}

$applicationDirectory = Join-Path $PSScriptRoot "..\PasseroDemo.Application"
$outputPaths = @(
	(Join-Path $applicationDirectory "bin\Security\UsersAndRoles.json"),
	(Join-Path $applicationDirectory "bin\Debug\net8.0\Security\UsersAndRoles.json"),
	(Join-Path $applicationDirectory "bin\Debug\net9.0\Security\UsersAndRoles.json"),
	(Join-Path $applicationDirectory "bin\Release\net8.0\Security\UsersAndRoles.json"),
	(Join-Path $applicationDirectory "bin\Release\net9.0\Security\UsersAndRoles.json")
)

foreach ($outputPath in $outputPaths) {
	$fullOutputPath = [System.IO.Path]::GetFullPath($outputPath)
	if ([System.IO.File]::Exists($fullOutputPath) -and
		-not [string]::Equals($fullOutputPath, $identityPath, [System.StringComparison]::OrdinalIgnoreCase)) {
		[System.IO.File]::WriteAllText($fullOutputPath, $json, (New-Object System.Text.UTF8Encoding($false)))
	}
}

Write-Host "Password aggiornate per test e Administrator. Il JSON contiene solo salt e hash PBKDF2-SHA256."
