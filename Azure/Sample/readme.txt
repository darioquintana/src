1) configure the ACS namespaces in web.config.
2) get the thumbprint on ACS and paste it into the web.config
3) run the following script into a membership provider database to create the table: UserIdentities
4) change the connectionString in the web.config

<sql>

SET QUOTED_IDENTIFIER OFF;
GO
USE [MembershipDatabase];
GO

-- Creating table 'UserIdentities'
CREATE TABLE [dbo].[UserIdentities] (
    [IdentityID] int IDENTITY(1,1) NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [IdentityProvider] nvarchar(255)  NOT NULL,
    [IdentityValue] nvarchar(255)  NOT NULL
);
GO


-- Creating primary key on [IdentityID] in table 'UserIdentities'
ALTER TABLE [dbo].[UserIdentities]
ADD CONSTRAINT [PK_UserIdentities]
    PRIMARY KEY CLUSTERED ([IdentityID] ASC);
GO

</sql>