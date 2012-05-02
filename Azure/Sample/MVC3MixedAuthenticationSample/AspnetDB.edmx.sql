
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/02/2012 12:21:45
-- Generated from EDMX file: D:\proyectos\src\Azure\Sample\MVC3MixedAuthenticationSample\AspnetDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MembershipDatabase2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__aspnet_Me__UserI__22AA2996]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[aspnet_Membership] DROP CONSTRAINT [FK__aspnet_Me__UserI__22AA2996];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[aspnet_Membership]', 'U') IS NOT NULL
    DROP TABLE [dbo].[aspnet_Membership];
GO
IF OBJECT_ID(N'[dbo].[aspnet_Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[aspnet_Users];
GO
IF OBJECT_ID(N'[dbo].[UserIdentity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserIdentity];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'aspnet_Membership'
CREATE TABLE [dbo].[aspnet_Membership] (
    [ApplicationId] uniqueidentifier  NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [Password] nvarchar(128)  NOT NULL,
    [PasswordFormat] int  NOT NULL,
    [PasswordSalt] nvarchar(128)  NOT NULL,
    [MobilePIN] nvarchar(16)  NULL,
    [Email] nvarchar(256)  NULL,
    [LoweredEmail] nvarchar(256)  NULL,
    [PasswordQuestion] nvarchar(256)  NULL,
    [PasswordAnswer] nvarchar(128)  NULL,
    [IsApproved] bit  NOT NULL,
    [IsLockedOut] bit  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [LastLoginDate] datetime  NOT NULL,
    [LastPasswordChangedDate] datetime  NOT NULL,
    [LastLockoutDate] datetime  NOT NULL,
    [FailedPasswordAttemptCount] int  NOT NULL,
    [FailedPasswordAttemptWindowStart] datetime  NOT NULL,
    [FailedPasswordAnswerAttemptCount] int  NOT NULL,
    [FailedPasswordAnswerAttemptWindowStart] datetime  NOT NULL,
    [Comment] nvarchar(max)  NULL
);
GO

-- Creating table 'aspnet_Users'
CREATE TABLE [dbo].[aspnet_Users] (
    [ApplicationId] uniqueidentifier  NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL,
    [LoweredUserName] nvarchar(256)  NOT NULL,
    [MobileAlias] nvarchar(16)  NULL,
    [IsAnonymous] bit  NOT NULL,
    [LastActivityDate] datetime  NOT NULL
);
GO

-- Creating table 'UserIdentities'
CREATE TABLE [dbo].[UserIdentities] (
    [IdentityID] int IDENTITY(1,1) NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [IdentityProvider] nvarchar(255)  NOT NULL,
    [IdentityValue] nvarchar(255)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UserId] in table 'aspnet_Membership'
ALTER TABLE [dbo].[aspnet_Membership]
ADD CONSTRAINT [PK_aspnet_Membership]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [UserId] in table 'aspnet_Users'
ALTER TABLE [dbo].[aspnet_Users]
ADD CONSTRAINT [PK_aspnet_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [IdentityID] in table 'UserIdentities'
ALTER TABLE [dbo].[UserIdentities]
ADD CONSTRAINT [PK_UserIdentities]
    PRIMARY KEY CLUSTERED ([IdentityID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'aspnet_Membership'
ALTER TABLE [dbo].[aspnet_Membership]
ADD CONSTRAINT [FK__aspnet_Me__UserI__22AA2996]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[aspnet_Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------