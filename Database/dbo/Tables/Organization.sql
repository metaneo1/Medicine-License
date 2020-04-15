CREATE TABLE [dbo].[Organization] (
    [Id]                 [dbo].[IdenticalParent] IDENTITY (1, 1) NOT NULL,
    [TIN]                NVARCHAR (MAX)          NULL,
    [NSCCode]            NVARCHAR (MAX)          NULL,
    [RegistrationNumber] NVARCHAR (MAX)          NULL,
    [Id_Type]            [dbo].[IdenticalParent] NOT NULL,
    [Address]            NVARCHAR (MAX)          NULL,
    [Id_AdminUnit]       [dbo].[Identical]       NOT NULL,
    [Name_ru]            NVARCHAR (MAX)          NULL,
    [Name_kg]            NVARCHAR (MAX)          NULL,
    [Description_kg]     NVARCHAR (MAX)          NULL,
    [Description_ru]     NVARCHAR (MAX)          NULL,
    [CODE]               NVARCHAR (MAX)          NULL,
    CONSTRAINT [XPKÎðãàíèçàöèÿ] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_184] FOREIGN KEY ([Id_Type]) REFERENCES [dbo].[OrganizationType] ([Id]),
    CONSTRAINT [R_186] FOREIGN KEY ([Id_AdminUnit]) REFERENCES [dbo].[AdminUnits] ([Id])
);



