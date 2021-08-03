param  (
	[Parameter(Mandatory=$false)]
	[string] $Name = "MG.Pwsh.Rdp",

	[Parameter(Mandatory=$false)]
	[string[]] $CopyToOutput = @('MG.Attributes/1.0.0.2', 'MG.Posh.Extensions/1.4.1')
)
$depFile = "$PSScriptRoot\$Name.deps.json"
$json = Get-Content -Path $depFile -Raw | ConvertFrom-Json
$netStd = $json.targets.'.NETStandard,Version=v2.0/'
foreach ($toCopy in $CopyToOutput)
{
	$pso = $netStd."$toCopy".runtime
	if ($null -eq $pso) {
		continue;
	}
	$mems = $pso | Get-Member -MemberType NoteProperty | Where { $_.Name -clike "lib/*" }
	foreach ($mem in $mems)
	{
		$fileName = [System.IO.Path]::GetFileName($mem.Name)
		if (-not (Test-Path -Path "$PSScriptRoot\$fileName" -PathType Leaf))
		{
			$file = "$env:USERPROFILE\.nuget\packages\$toCopy\$($mem.Name)"
			Copy-Item -Path $file -Destination "$PSScriptRoot"
		}
		else
		{
			Write-Host "`"$fileName`" already copied..." -f Yellow
		}
	}
}

Import-Module "$PSScriptRoot\$Name.dll" -ErrorAction Stop
Set-Location "$env:DESK"
$file = "C:\Users\Mike\Desktop\test.rdp"