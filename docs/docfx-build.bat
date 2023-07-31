

@echo off

REM Definição das variáveis padrão
set server_flag=0
set build_flag=0
set clean_flag=0

REM Loop para verificar os argumentos passados na linha de comando
:parse_args
if "%~1"=="" goto end_parse_args

if "%~1"=="-s" (
    set server_flag=1
    shift
    goto parse_args
)

if "%~1"=="--server" (
    set server_flag=1
    shift
    goto parse_args
)

if "%~1"=="-b" (
    set build_flag=1
    shift
    goto parse_args
)

if "%~1"=="--build" (
    set build_flag=1
    shift
    goto parse_args
)

if "%~1"=="-c" (
    set clean_flag=1
    shift
    goto parse_args
)

if "%~1"=="--clean" (
    set clean_flag=1
    shift
    goto parse_args
)

:end_parse_args

REM Agora, você pode realizar ações com base nas flags definidas

REM Exemplo de utilização:
if %server_flag%==1 (
    echo Executando acao do servidor...
    copy README.md index.md
    rmdir /s /q _site
    docfx build
    docfx --serve
)

if %build_flag%==1 (
    echo Executando acao de build...
    copy README.md index.md
    rmdir /s /q _site
    docfx build
)

if %clean_flag%==1 (
    echo Executando acao de limpeza...
    rmdir /s /q _site
)

REM Se nenhuma flag for passada, faça algo padrão
if %server_flag%==0 if %build_flag%==0 if %clean_flag%==0 (
    echo Nenhuma flag foi passada. Realizando acao padrao...
    copy README.md index.md
    rmdir /s /q _site
    docfx build
)

REM Finalização do script
echo Script concluido.