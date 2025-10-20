﻿CREATE TABLE [EPDC].[AspNetUserRoles] (
    [UserId] NVARCHAR(450) NOT NULL,
    [RoleId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] 
        FOREIGN KEY ([UserId]) REFERENCES [EPDC].[AspNetUsers] ([Id]) 
        ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] 
        FOREIGN KEY ([RoleId]) REFERENCES [EPDC].[AspNetRoles] ([Id]) 
        ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] 
    ON [EPDC].[AspNetUserRoles] ([RoleId]);
GO
