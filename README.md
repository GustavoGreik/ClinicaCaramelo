# Clínica Caramelo

Sistema simples de gestão para uma clínica veterinária, desenvolvido com ASP.NET Core MVC (.NET 8), Entity Framework Core, MySQL e Bootstrap.

## Acesso demonstrativo

- E-mail: `admin@caramelo.com.br`
- Senha: `Caramelo@123`

A conta é criada automaticamente pela aplicação na primeira execução, com a senha armazenada em hash. Use apenas para demonstração acadêmica e troque a senha antes de qualquer uso real.

## Funcionalidades

- Cadastro, edição e exclusão de tutores, com validação de CPF e e-mail duplicados.
- Cadastro, edição e exclusão de animais, cada um com um tutor principal.
- Cadastro e manutenção de veterinários, com validação de CRMV duplicado.
- Agendamento e manutenção de consultas/serviços veterinários.
- Login com autenticação por cookie e acesso protegido às telas de gestão.
- Relatório de consulta com dados do animal, tutor/cliente e veterinário, com opção de imprimir ou salvar em PDF.
- Página inicial com indicadores e próximas consultas.
- Validações de campos e proteção contra exclusão de registros que ainda possuem dependências.

O projeto **não implementa associação muitos-para-muitos entre tutores e animais**. Cada animal possui um único `TutorId`, mantendo a lógica direta e adequada a um projeto introdutório.

## Requisitos

- .NET 8 SDK
- MySQL 8 ou compatível
- Visual Studio 2022 ou VS Code
- Acesso à internet para carregar Bootstrap e scripts de validação por CDN

## Configuração

1. Execute o arquivo `database/clinica_caramelo.sql` no MySQL Workbench ou phpMyAdmin. Ele cria as tabelas e os relacionamentos necessários.
2. Abra `appsettings.json` e substitua `SUA_SENHA` pela senha do seu MySQL. A conexão já está configurada para `localhost` na porta `3306`.
3. No terminal, entre na pasta do projeto e restaure os pacotes:
   ```bash
   dotnet restore
   ```
4. Instale a ferramenta EF Core, se ainda não estiver instalada:
   ```bash
   dotnet tool install --global dotnet-ef
   ```
5. Crie as tabelas e aplique a migração:
   ```bash
   dotnet ef migrations add CriacaoInicial
   dotnet ef database update
   ```
6. Execute:
   ```bash
   dotnet run
   ```

> Se `dotnet-ef` já estiver instalado, não é necessário instalá-lo novamente. Caso o comando de migração não funcione, confira se o terminal está na pasta que contém `ClinicaCaramelo.csproj`.

## Fluxo recomendado para testar

1. Cadastre um tutor.
2. Cadastre um veterinário.
3. Cadastre um animal selecionando o tutor.
4. Agende uma consulta para o animal com o veterinário.
5. Confira a consulta na página inicial.
6. Tente cadastrar CPF, e-mail ou CRMV repetidos para conferir as validações.

## Histórico Git em commits organizados

O arquivo `scripts/criar-historico-git.ps1` cria um repositório Git local e registra commits em etapas temáticas, usando os arquivos já incluídos no projeto. Execute no PowerShell, dentro da pasta do projeto:

```powershell
Set-ExecutionPolicy -Scope Process Bypass
.\scripts\criar-historico-git.ps1
```

O script para se a pasta já contiver um repositório Git. Ele cria commits reais com conteúdo correspondente a cada etapa; não inventa alterações. Revise o histórico e faça o push para o seu repositório GitHub quando estiver pronto.

## Observações

- A conexão usa configurações locais de exemplo. Não publique senhas reais.
- O projeto foi preparado como base didática. Execute a restauração, as migrações e os testes no seu ambiente antes de considerar a aplicação pronta para uso real.
