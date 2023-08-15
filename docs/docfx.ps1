function Parse-Args() {
    param (
      [Parameter(Mandatory=$true)]
      [string]$Command,
      [switch]$Install,
      [switch]$Build,
      [switch]$Server,
      [switch]$Help
    )
  
    # Check if the help flag was passed
    if ($Help) {
      Write-Host "Usage: docfx [command] [options]"
      Write-Host ""
      Write-Host "Commands:"
      Write-Host "  install - Install and initialize DocFX"
      Write-Host "  build - Build the .NET project and generate documentation using DocFX"
      Write-Host "  server - Start the DocFX server"
      Write-Host ""
      Write-Host "Options:"
      Write-Host "  -h, --help - Show this help message"
      exit 0
    }
  
    # Check if the command was specified
    if (-not $Command) {
      Write-Error "No command specified."
      exit 1
    }
  
    # Check if the install flag was passed
    if ($Install) {
      Write-Host "Installing DocFX..."
      Install-Package DocFX
      Write-Host "DocFX installed successfully."
    }
  
    # Check if the build flag was passed
    if ($Build) {
      # Check if .NET 6 SDK is installed
      if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        Write-Host "Installing .NET 6 SDK..."
        dotnet --version
        Write-Host ".NET 6 SDK installed successfully."
      }
  
      # Install DocFX components
      dotnet tool install DocFxTocGenerator -g
      dotnet tool install DocLanguageTranslator -g
      dotnet tool install DocLinkChecker -g
      dotnet tool install DocFxOpenApi -g
  
      # Create folders if they don't exist
      if (-not (Test-Path docs)) {
        New-Item docs
      }
      if (-not (Test-Path docs/articles)) {
        New-Item docs/articles
      }
      if (-not (Test-Path docs/articles/FAQs)) {
        New-Item docs/articles/FAQs
      }
  
      # Create files in docs folder if they don't exist
      if (-not (Test-Path docs/CHANGELOG.md)) {
        New-Item docs/CHANGELOG.md
      }
      if (-not (Test-Path docs/CONTRIBUTING.md)) {
        New-Item docs/CONTRIBUTING.md
      }
      if (-not (Test-Path docs/README.md)) {
        New-Item docs/README.md
      }
    }
  
    # Check if the server flag was passed
    if ($Server) {
      Write-Host "Starting the DocFX server..."
      docfx start
      Write-Host "DocFX server started successfully."
    }
  }



# Install docfx and initialize documentation

if ($args[0] -eq "-i" -or $args[0] -eq "--install") {
    # Check if .NET 6 is installed
    if (-not (Get-Command dotnet --version)) {
        Write-Host "O .NET 6 não está instalado. Instalando..."
        Invoke-WebRequest https://dotnet.microsoft.com/download/dotnet/6.0 | Out-Null
    }

    # Install docfx
    dotnet tool install docfx -g
    dotnet tool install DocFxTocGenerator -g
    dotnet tool install DocLanguageTranslator -g
    dotnet tool install DocLinkChecker -g
    dotnet tool install DocFxOpenApi -g

    # Initialize documentation
    if (-not (Test-Path docs)) {
        mkdir docs
    }

    cd docs

    docfx init -o . -f

    # Create Docs directory
    mkdir Docs

    # Create FAQs directory
    mkdir Docs/FAQs

    # Create Markdown files
    New-Item -Type File -Path Docs/CHANGELOG.md -Value "This is the CHANGELOG file."
    New-Item -Type File -Path Docs/CONTRIBUTING.md -Value "This is the CONTRIBUTING file."
    New-Item -Type File -Path Docs/FAQs/FAQ.md -Value "This is the FAQ file."
    New-Item -Type File -Path Docs/README.md -Value "This is the README file."
}

# Build documentation
if ($args[0] -eq "-b" -or $args[0] -eq "--build") {
    docfx build .\docfx.json
}