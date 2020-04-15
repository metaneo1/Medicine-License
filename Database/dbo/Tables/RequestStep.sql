CREATE TABLE [dbo].[RequestStep] (
    [Id]              [dbo].[IdenticalParent] IDENTITY (1, 1) NOT NULL,
    [Id_Parent]       [dbo].[IdenticalParent] NULL,
    [Name_ru]         NVARCHAR (MAX)          NULL,
    [Name_kg]         NVARCHAR (MAX)          NULL,
    [Description_ru]  NVARCHAR (MAX)          NULL,
    [Description_kg]  NVARCHAR (MAX)          NULL,
    [CODE]            NVARCHAR (MAX)          NULL,
    [Url]             NVARCHAR (MAX)          NULL,
    [ActiveImageUrl]  NVARCHAR (MAX)          NULL,
    [DiabledImageUrl] NVARCHAR (MAX)          NULL,
    CONSTRAINT [XPKØàã_çàÿâêè] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_213] FOREIGN KEY ([Id_Parent]) REFERENCES [dbo].[RequestStep] ([Id])
);

