CREATE TABLE [dbo].[RequestStateHistory] (
    [Id]               [dbo].[Identical]       IDENTITY (1, 1) NOT NULL,
    [Id_Request]       [dbo].[IdenticalParent] NOT NULL,
    [Id_State]         [dbo].[IdenticalParent] NOT NULL,
    [DateStatusChange] DATETIME                NULL,
    [Id_User]          INT                     NULL,
    CONSTRAINT [XPKÈñòîðèÿ_ñòàòóñîâ_çàÿâêè] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RequestStateHistory_ApplicationUser] FOREIGN KEY ([Id_User]) REFERENCES [dbo].[ApplicationUser] ([Id]),
    CONSTRAINT [R_182] FOREIGN KEY ([Id_Request]) REFERENCES [dbo].[LicenseRequest] ([Id]),
    CONSTRAINT [R_183] FOREIGN KEY ([Id_State]) REFERENCES [dbo].[RequestState] ([Id])
);



