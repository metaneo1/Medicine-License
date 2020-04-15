CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                 NVARCHAR (128) NOT NULL,
    [UserName]           NVARCHAR (MAX) NULL,
    [PasswordHash]       NVARCHAR (MAX) NULL,
    [SecurityStamp]      NVARCHAR (MAX) NULL,
    [Discriminator]      NVARCHAR (128) NOT NULL,
    [Id_ApplicationUser] INT            NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUsers_ApplicationUser] FOREIGN KEY ([Id_ApplicationUser]) REFERENCES [dbo].[ApplicationUser] ([Id])
);



