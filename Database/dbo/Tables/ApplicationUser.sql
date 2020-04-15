CREATE TABLE [dbo].[ApplicationUser] (
    [Id]       [dbo].[IdenticalParent] IDENTITY (1, 1) NOT NULL,
    [FullName] NVARCHAR (MAX)          NULL,
    [phone1]   NVARCHAR (MAX)          NULL,
    [phone2]   NVARCHAR (MAX)          NULL,
    [email]    NVARCHAR (MAX)          NULL,
    [DOB]      DATETIME                NULL,
    [Id_Sex]   [dbo].[IdenticalChild]  NOT NULL,
    [IsActive] [dbo].[Logical]         NULL,
    CONSTRAINT [XPKÏîëüçîâàòåëü] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_179] FOREIGN KEY ([Id_Sex]) REFERENCES [dbo].[Sex] ([Id])
);









