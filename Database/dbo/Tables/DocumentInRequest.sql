CREATE TABLE [dbo].[DocumentInRequest] (
    [Id]                [dbo].[Identical]       IDENTITY (1, 1) NOT NULL,
    [Id_Request]        [dbo].[IdenticalParent] NOT NULL,
    [Id_Document]       [dbo].[IdenticalChild]  NOT NULL,
    [Id_DocumentState]  [dbo].[IdenticalParent] NOT NULL,
    [Id_RequestElement] [dbo].[Identical]       NULL,
    [Description]       NVARCHAR (MAX)          NULL,
    CONSTRAINT [XPKÏîäàâàåìûé_äîêóìåíò] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_193] FOREIGN KEY ([Id_Request]) REFERENCES [dbo].[LicenseRequest] ([Id]),
    CONSTRAINT [R_195] FOREIGN KEY ([Id_Document]) REFERENCES [dbo].[Document] ([Id]),
    CONSTRAINT [R_199] FOREIGN KEY ([Id_DocumentState]) REFERENCES [dbo].[DocumentState] ([Id]),
    CONSTRAINT [R_204] FOREIGN KEY ([Id_RequestElement]) REFERENCES [dbo].[RequestElement] ([Id])
);

