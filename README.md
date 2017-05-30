# database-cd

-- Aplicação de Migração Arquivo de Configuração

dotnet run --UseConfigurationFile

-- Aplicação de Migração Com Variável de Ambiente

dotnet run --ConnectionString "%DefaultConnection%"

-- Aplicar Rollback
dotnet run --UseConfigurationFile --Rollback --RollbackVersion 2017.01.12

-- Aplicação de Rollback Com Variável de Ambiente
dotnet run --ConnectionString "%DefaultConnection%" --Rollback --RollbackVersion 2017.01.12

