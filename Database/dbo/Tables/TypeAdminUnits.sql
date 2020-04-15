CREATE TABLE [dbo].[TypeAdminUnits] (
    [Id]             [dbo].[Identical] IDENTITY (1, 1) NOT NULL,
    [Name_kg]        NVARCHAR (MAX)    NULL,
    [Description_kg] NVARCHAR (MAX)    NULL,
    [Name_ru]        NVARCHAR (MAX)    NULL,
    [Description_ru] NVARCHAR (MAX)    NULL,
    [CODE]           NVARCHAR (MAX)    NULL,
    CONSTRAINT [XPKÒèï_Àäìèíèñòðàòèâíîãî_äåëåíèÿ] PRIMARY KEY CLUSTERED ([Id] ASC)
);



