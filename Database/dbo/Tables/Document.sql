CREATE TABLE [dbo].[Document] (
    [Id]                [dbo].[IdenticalChild] IDENTITY (1, 1) NOT NULL,
    [Description]       NVARCHAR (MAX)         NULL,
    [Name]              NVARCHAR (MAX)         NULL,
    [Filename]          NVARCHAR (MAX)         NULL,
    [PathToFile]        NVARCHAR (MAX)         NULL,
    [Id_DocumentFormat] [dbo].[IdenticalChild] NOT NULL,
    CONSTRAINT [XPKÄîêóìåíò] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_211] FOREIGN KEY ([Id_DocumentFormat]) REFERENCES [dbo].[Document_Format] ([Id])
);

