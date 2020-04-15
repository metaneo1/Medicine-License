CREATE TABLE [dbo].[RequestElementType] (
    [Id]             [dbo].[IdenticalParent] IDENTITY (1, 1) NOT NULL,
    [Name_ru]        NVARCHAR (MAX)          NULL,
    [Name_kg]        NVARCHAR (MAX)          NULL,
    [Description_ru] NVARCHAR (MAX)          NULL,
    [Description_kg] NVARCHAR (MAX)          NULL,
    [CODE]           NVARCHAR (MAX)          NULL,
    [IsNeedTemplate] [dbo].[Logical]         NULL,
    CONSTRAINT [XPKÒèï_ýëåìåíòà_çàÿâêè] PRIMARY KEY CLUSTERED ([Id] ASC)
);

