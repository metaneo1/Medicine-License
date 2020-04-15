CREATE TABLE [dbo].[MessageForSentDocument] (
    [Id]              [dbo].[Identical]      IDENTITY (1, 1) NOT NULL,
    [Id_SentDocument] [dbo].[Identical]      NOT NULL,
    [Id_Message]      [dbo].[IdenticalChild] NOT NULL,
    CONSTRAINT [XPKÏåðåïèñêà_ïî_äîêóìåíòó] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [R_197] FOREIGN KEY ([Id_SentDocument]) REFERENCES [dbo].[DocumentInRequest] ([Id]),
    CONSTRAINT [R_198] FOREIGN KEY ([Id_Message]) REFERENCES [dbo].[Message] ([Id])
);

