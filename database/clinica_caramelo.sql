-- Clínica Caramelo - banco de dados MySQL
-- Execute este arquivo no MySQL Workbench ou phpMyAdmin.
-- A aplicação usa localhost:3306; ajuste usuário/senha em appsettings.json.

CREATE DATABASE IF NOT EXISTS clinica_caramelo
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE clinica_caramelo;

CREATE TABLE IF NOT EXISTS `Tutores` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Cpf` varchar(14) NOT NULL,
  `Email` varchar(120) NOT NULL,
  `Telefone` varchar(20) NOT NULL,
  `Endereco` varchar(180) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Tutores_Cpf` (`Cpf`),
  UNIQUE KEY `IX_Tutores_Email` (`Email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `Animais` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(80) NOT NULL,
  `Especie` varchar(40) NOT NULL,
  `Raca` varchar(50) NULL,
  `Idade` int NULL,
  `Sexo` varchar(10) NULL,
  `TutorId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Animais_TutorId` (`TutorId`),
  CONSTRAINT `FK_Animais_Tutores_TutorId` FOREIGN KEY (`TutorId`) REFERENCES `Tutores` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `Veterinarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Crmv` varchar(20) NOT NULL,
  `Especialidade` varchar(60) NOT NULL,
  `Email` varchar(120) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Veterinarios_Crmv` (`Crmv`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `Consultas` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AnimalId` int NOT NULL,
  `VeterinarioId` int NOT NULL,
  `DataHora` datetime(6) NOT NULL,
  `Motivo` varchar(500) NOT NULL,
  `Observacoes` varchar(1000) NULL,
  `Status` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Consultas_AnimalId` (`AnimalId`),
  KEY `IX_Consultas_VeterinarioId` (`VeterinarioId`),
  CONSTRAINT `FK_Consultas_Animais_AnimalId` FOREIGN KEY (`AnimalId`) REFERENCES `Animais` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Consultas_Veterinarios_VeterinarioId` FOREIGN KEY (`VeterinarioId`) REFERENCES `Veterinarios` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS `Usuarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Email` varchar(120) NOT NULL,
  `SenhaHash` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Usuarios_Email` (`Email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- O usuário administrador demonstrativo é criado pela aplicação na primeira execução,
-- com senha armazenada em hash. Login: admin@caramelo.com.br | Senha: Caramelo@123
