﻿CREATE TABLE [EPDC].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR(450) NOT NULL,
    [ProviderKey] NVARCHAR(450) NOT NULL,
    [ProviderDisplayName] NVARCHAR(MAX) NULL,
    [UserId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] 
        FOREIGN KEY ([UserId]) REFERENCES [EPDC].[AspNetUsers] ([Id]) 
        ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] 
    ON [EPDC].[AspNetUserLogins] ([UserId]);
GO