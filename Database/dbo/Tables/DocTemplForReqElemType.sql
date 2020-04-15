CREATE TABLE [dbo].[DocTemplForReqElemType] (
    [Id]                    [dbo].[IdenticalParent] IDENTITY (1, 1) NOT NULL,
    [Name_ru]               NVARCHAR (MAX)          NULL,
    [Name_kg]               NVARCHAR (MAX)          NULL,
    [Description_ru]        NVARCHAR (MAX)          NULL,
    [Description_kg]        NVARCHAR (MAX)          NULL,
    [CODE]                  NVARCHAR (MAX)          NULL,
    [Id_Document]           [dbo].[IdenticalChild]  NOT NULL,
    [Id_RequestElementType] [dbo].[IdenticalParent] NOT NULL,
    CONSTRAINT [XPKØàáëîíû_äîêóìåíòîâ_çàÿâêè] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_173] FOREIGN KEY ([Id_Document]) REFERENCES [dbo].[Document] ([Id]),
    CONSTRAINT [R_212] FOREIGN KEY ([Id_RequestElementType]) REFERENCES [dbo].[RequestElementType] ([Id])
);

