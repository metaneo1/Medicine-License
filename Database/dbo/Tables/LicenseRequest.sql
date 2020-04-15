CREATE TABLE [dbo].[LicenseRequest] (
    [Id]             [dbo].[IdenticalParent] IDENTITY (1, 1) NOT NULL,
    [DateCreate]     DATETIME                NULL,
    [ClinicName]     NVARCHAR (MAX)          NULL,
    [Address]        NVARCHAR (MAX)          NULL,
    [Phone1]         NVARCHAR (MAX)          NULL,
    [Phone2]         NVARCHAR (MAX)          NULL,
    [Email]          NVARCHAR (MAX)          NULL,
    [Id_Company]     [dbo].[IdenticalParent] NULL,
    [Id_AdminUnit]   [dbo].[Identical]       NULL,
    [Id_RequestType] [dbo].[IdenticalParent] NULL,
    [Id_User]        [dbo].[IdenticalParent] NOT NULL,
    [IsDraft]        BIT                     CONSTRAINT [DF_LicenseRequest_IsDraft] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [XPKÇàÿâêà] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_185] FOREIGN KEY ([Id_Company]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [R_187] FOREIGN KEY ([Id_AdminUnit]) REFERENCES [dbo].[AdminUnits] ([Id]),
    CONSTRAINT [R_188] FOREIGN KEY ([Id_RequestType]) REFERENCES [dbo].[RequestType] ([Id]),
    CONSTRAINT [R_192] FOREIGN KEY ([Id_User]) REFERENCES [dbo].[ApplicationUser] ([Id])
);





