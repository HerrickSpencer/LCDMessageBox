
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/29/2018 17:59:00
-- Generated from EDMX file: E:\Projects\lcdmessagebox\LCDMessage\LCDMessage\DAL\MessageLogDAL.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LCDMessageDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[MessageLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MessageLogs];
GO
IF OBJECT_ID(N'[dbo].[MessageLog1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MessageLog1];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'MessageLogs'
CREATE TABLE [dbo].[MessageLogs] (
    [messageID] int IDENTITY(1,1) NOT NULL,
    [message] varchar(max)  NOT NULL,
    [username] varchar(50)  NOT NULL,
    [messageDate] datetime  NOT NULL
);
GO

-- Creating table 'MessageLog1'
CREATE TABLE [dbo].[MessageLog1] (
    [messageID] int IDENTITY(1,1) NOT NULL,
    [message] varchar(max)  NOT NULL,
    [username] varchar(50)  NOT NULL,
    [messageDate] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [messageID] in table 'MessageLogs'
ALTER TABLE [dbo].[MessageLogs]
ADD CONSTRAINT [PK_MessageLogs]
    PRIMARY KEY CLUSTERED ([messageID] ASC);
GO

-- Creating primary key on [messageID] in table 'MessageLog1'
ALTER TABLE [dbo].[MessageLog1]
ADD CONSTRAINT [PK_MessageLog1]
    PRIMARY KEY CLUSTERED ([messageID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------