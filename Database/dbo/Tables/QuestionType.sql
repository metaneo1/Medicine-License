CREATE TABLE [dbo].[QuestionType] (
    [Id]             [dbo].[IdenticalParent] IDENTITY (1, 1) NOT NULL,
    [Id_Parent]      [dbo].[IdenticalParent] NULL,
    [Name_ru]        NVARCHAR (MAX)          NULL,
    [Name_kg]        NVARCHAR (MAX)          NULL,
    [Description_ru] NVARCHAR (MAX)          NULL,
    [Description_kg] NVARCHAR (MAX)          NULL,
    [CODE]           NVARCHAR (MAX)          NULL,
    CONSTRAINT [XPKÊàòåãîðèÿ_âîïðîñà] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_190] FOREIGN KEY ([Id_Parent]) REFERENCES [dbo].[QuestionType] ([Id])
);

