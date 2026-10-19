$ErrorActionPreference = "Stop"

if (Test-Path ".git") {
    throw "Esta pasta já contém um repositório Git. Por segurança, o script não altera o histórico existente."
}

git init
if ($LASTEXITCODE -ne 0) { throw "Não foi possível inicializar o Git." }

git config user.name "Desenvolvimento Clínica Caramelo"
git config user.email "dev@clinica-caramelo.local"

git add .gitignore ClinicaCaramelo.csproj Program.cs appsettings.json
git commit -m "chore: criar estrutura inicial ASP.NET Core MVC"
if ($LASTEXITCODE -ne 0) { throw "Falha ao criar o commit inicial." }

git add Data Models
git commit -m "feat: adicionar modelos e contexto do banco de dados"
if ($LASTEXITCODE -ne 0) { throw "Falha ao registrar os modelos." }

git add Controllers
git commit -m "feat: implementar operações e validações do backend"
if ($LASTEXITCODE -ne 0) { throw "Falha ao registrar os controllers." }

git add Views
git commit -m "feat: criar telas de cadastro e gestão"
if ($LASTEXITCODE -ne 0) { throw "Falha ao registrar as views." }

git add wwwroot
git commit -m "style: aplicar identidade visual responsiva da Clínica Caramelo"
if ($LASTEXITCODE -ne 0) { throw "Falha ao registrar os estilos." }

git add database README.md scripts
git commit -m "docs: documentar configuração do MySQL e histórico do projeto"
if ($LASTEXITCODE -ne 0) { throw "Falha ao registrar a documentação." }

Write-Host ""
Write-Host "Histórico criado. Confira com: git log --oneline"
